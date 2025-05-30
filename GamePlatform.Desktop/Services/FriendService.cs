using GamePlatform.Shared.Data;
using GamePlatform.Shared.DTO;
using GamePlatform.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamePlatform.Desktop.Services;

public sealed class FriendService : IFriendService
{
    private readonly AppDbContext _db;
    private readonly INotificationService? _notify;

    public FriendService(AppDbContext db, INotificationService? notif = null)
    {
        _db = db;
        _notify = notif;
    }

    /* ─────────────  zapytania  ───────────── */

    public async Task<IReadOnlyList<User>> GetFriendsAsync(Guid userId) =>
        await _db.Friends
                 .Where(f => f.UserId == userId || f.MateId == userId)
                 .Select(f => f.UserId == userId ? f.Mate! : f.User!)
                 .AsNoTracking()
                 .ToListAsync();

    public async Task<IReadOnlyList<FriendRequestDto>> GetPendingAsync(Guid userId) =>
        await _db.FriendRequests
                 .Where(r => r.ToUserId == userId)
                 .Select(r => new FriendRequestDto(r.Id, r.FromUserId, r.ToUserId, r.SentUtc))
                 .AsNoTracking()
                 .ToListAsync();

    public async Task<IReadOnlyList<User>> FindUsersAsync(string term, Guid currentId) =>
        await _db.Users
                 .Where(u => (u.Login.Contains(term) || u.Email.Contains(term)) &&
                             u.Id != currentId)
                 .OrderBy(u => u.Login)
                 .Take(20)
                 .AsNoTracking()
                 .ToListAsync();

    public async Task<bool> AreFriendsAsync(Guid a, Guid b) =>
        await _db.Friends.AnyAsync(f =>
               (f.UserId == a && f.MateId == b) ||
               (f.UserId == b && f.MateId == a));

    public async Task<bool> IsBlockedAsync(Guid blocker, Guid other) =>
        await _db.BlockedUsers.AnyAsync(bu => bu.BlockerId == blocker && bu.BlockedId == other);

    public async Task<IReadOnlyList<User>> GetBlockedAsync(Guid userId) =>
        await _db.BlockedUsers
                 .Where(b => b.BlockerId == userId)
                 .Select(b => b.Blocked!)
                 .AsNoTracking()
                 .ToListAsync();

    /* ─────────────  polecenia  ───────────── */

    public async Task SendRequestAsync(Guid fromUserId, Guid toUserId)
    {
        if (fromUserId == toUserId) return;
        if (await AreFriendsAsync(fromUserId, toUserId)) return;
        if (await IsBlockedAsync(toUserId, fromUserId)) return;   // zablokowany nie zaprosi

        bool dup = await _db.FriendRequests.AnyAsync(r =>
                    (r.FromUserId == fromUserId && r.ToUserId == toUserId) ||
                    (r.FromUserId == toUserId && r.ToUserId == fromUserId));
        if (dup) return;

        _db.FriendRequests.Add(new FriendRequest
        {
            FromUserId = fromUserId,
            ToUserId = toUserId
        });
        await _db.SaveChangesAsync();
        _notify?.Info("Wysłano zaproszenie");
    }

    public async Task AcceptRequestAsync(Guid requestId)
    {
        var r = await _db.FriendRequests.FindAsync(requestId);
        if (r is null) return;

        if (!await AreFriendsAsync(r.FromUserId, r.ToUserId))
            _db.Friends.Add(new Friend { UserId = r.FromUserId, MateId = r.ToUserId });

        // remove both requests in case users invited each other
        var reverseReq = await _db.FriendRequests.FirstOrDefaultAsync(fr =>
            fr.FromUserId == r.ToUserId && fr.ToUserId == r.FromUserId); ;
        _db.FriendRequests.Remove(r);
        if (reverseReq is not null) _db.FriendRequests.Remove(reverseReq);
        await _db.SaveChangesAsync();
        _notify?.Info("Zaproszenie zaakceptowane");
    }

    public async Task DeclineRequestAsync(Guid requestId)
    {
        var r = await _db.FriendRequests.FindAsync(requestId);
        if (r is null) return;

        _db.FriendRequests.Remove(r);
        await _db.SaveChangesAsync();
        _notify?.Info("Zaproszenie odrzucone");
    }

    public async Task RemoveFriendAsync(Guid userId, Guid mateId)
    {
        var rel = await _db.Friends.FirstOrDefaultAsync(f =>
                     (f.UserId == userId && f.MateId == mateId) ||
                     (f.UserId == mateId && f.MateId == userId));
        if (rel is null) return;

        _db.Friends.Remove(rel);
        await _db.SaveChangesAsync();
        _notify?.Info("Usunięto znajomego");
    }

    public async Task BlockAsync(Guid blockerId, Guid blockedId)
    {
        if (blockerId == blockedId) return;
        if (await IsBlockedAsync(blockerId, blockedId)) return;

        if (!await IsBlockedAsync(blockerId, blockedId))
            _db.BlockedUsers.Add(new BlockedUser { BlockerId = blockerId, BlockedId = blockedId });
        if (!await IsBlockedAsync(blockedId, blockerId))
            _db.BlockedUsers.Add(new BlockedUser { BlockerId = blockedId, BlockedId = blockerId });

        var req1 = await _db.FriendRequests.FirstOrDefaultAsync(fr => fr.FromUserId == blockerId && fr.ToUserId == blockedId);
        var req2 = await _db.FriendRequests.FirstOrDefaultAsync(fr => fr.FromUserId == blockedId && fr.ToUserId == blockerId);
        if (req1 is not null) _db.FriendRequests.Remove(req1);
        if (req2 is not null && req2 != req1) _db.FriendRequests.Remove(req2);

        await RemoveFriendAsync(blockerId, blockedId);   // blokada rozłącza znajomych
        await _db.SaveChangesAsync();
        _notify?.Info("Użytkownik zablokowany");
    }

    public async Task UnblockAsync(Guid blockerId, Guid blockedId)
    {
        var block1 = await _db.BlockedUsers.FindAsync(blockerId, blockedId);
        var block2 = await _db.BlockedUsers.FindAsync(blockedId, blockerId);

        if (block1 is not null) _db.BlockedUsers.Remove(block1);
        if (block2 is not null) _db.BlockedUsers.Remove(block2);

        
        await _db.SaveChangesAsync();
        _notify?.Info("Blokada usunięta");
    }
    public async Task ToggleFriendAsync(Guid userId, Guid mateId)
    {
        if (await AreFriendsAsync(userId, mateId))
        {
            await RemoveFriendAsync(userId, mateId);
        }
        else
        {
            await SendRequestAsync(userId, mateId);
        }
    }

    public Task<bool> IsUserOnlineAsync(Guid userId)
        => Task.FromResult(false);
}
