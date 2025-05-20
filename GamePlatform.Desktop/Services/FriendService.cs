using GamePlatform.Shared.Data;
using GamePlatform.Shared.DTO;
using GamePlatform.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamePlatform.Desktop.Services;

/// <summary>
/// Logika zaproszeń i listy znajomych.
/// </summary>
public sealed class FriendService : IFriendService
{
    private readonly AppDbContext _db;
    private readonly INotificationService? _notif;  // może być null w testach

    public FriendService(AppDbContext db, INotificationService? notif = null)
    {
        _db = db;
        _notif = notif;
    }

    /* ======== zapytania ======== */

    public async Task<IReadOnlyList<User>> GetFriendsAsync(Guid userId) =>
        await _db.Friends
                 .Where(f => f.UserId == userId || f.MateId == userId)
                 .Select(f => f.UserId == userId ? f.Mate! : f.User!)
                 .AsNoTracking()
                 .ToListAsync();

    public async Task<IReadOnlyList<FriendRequestDto>> GetPendingAsync(Guid userId) =>
        await _db.FriendRequests
                 .Where(r => r.ToUserId == userId)
                 .Select(r => new FriendRequestDto(r.FromUserId, r.ToUserId, r.SentUtc))
                 .AsNoTracking()
                 .ToListAsync();

    /// <summary>Wyszukuje użytkowników pasujących do <paramref name="term"/> (login / e-mail).</summary>
    public async Task<IReadOnlyList<User>> FindUsersAsync(string term, Guid currentId) =>
        await _db.Users
                 .Where(u => (u.Login.Contains(term) || u.Email.Contains(term)) &&
                             u.Id != currentId)
                 .OrderBy(u => u.Login)
                 .Take(20)
                 .AsNoTracking()
                 .ToListAsync();

    /* ======== komendy ======== */

    public async Task SendRequestAsync(Guid fromUserId, Guid toUserId)
    {
        if (fromUserId == toUserId) return;  // do siebie

        bool alreadyFriends = await _db.Friends.AnyAsync(f =>
                                (f.UserId == fromUserId && f.MateId == toUserId) ||
                                (f.UserId == toUserId && f.MateId == fromUserId));
        if (alreadyFriends) return;

        bool pending = await _db.FriendRequests.AnyAsync(r =>
                        r.FromUserId == fromUserId && r.ToUserId == toUserId);
        if (pending) return;

        _db.FriendRequests.Add(new FriendRequest
        {
            FromUserId = fromUserId,
            ToUserId = toUserId
        });
        await _db.SaveChangesAsync();

        _notif?.Raise("Wysłano zaproszenie do znajomych");
    }

    public async Task AcceptRequestAsync(Guid requestId)
    {
        var req = await _db.FriendRequests.FindAsync(requestId);
        if (req is null) return;

        _db.Friends.Add(new Friend { UserId = req.FromUserId, MateId = req.ToUserId });
        _db.FriendRequests.Remove(req);
        await _db.SaveChangesAsync();

        _notif?.Raise("Zaproszenie zaakceptowane");
    }

    public async Task DeclineRequestAsync(Guid requestId)
    {
        var req = await _db.FriendRequests.FindAsync(requestId);
        if (req is null) return;

        _db.FriendRequests.Remove(req);
        await _db.SaveChangesAsync();

        _notif?.Raise("Zaproszenie odrzucone");
    }

    public async Task RemoveFriendAsync(Guid userId, Guid mateId)
    {
        var rel = await _db.Friends.FirstOrDefaultAsync(f =>
                     (f.UserId == userId && f.MateId == mateId) ||
                     (f.UserId == mateId && f.MateId == userId));
        if (rel is null) return;

        _db.Friends.Remove(rel);
        await _db.SaveChangesAsync();

        _notif?.Raise("Usunięto z listy znajomych");
    }
}
