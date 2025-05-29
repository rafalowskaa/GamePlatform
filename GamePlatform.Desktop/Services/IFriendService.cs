using GamePlatform.Shared.DTO;
using GamePlatform.Shared.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GamePlatform.Desktop.Services;

public interface IFriendService
{
    /* zapytania */
    Task<IReadOnlyList<User>> GetFriendsAsync(Guid userId);
    Task<IReadOnlyList<FriendRequestDto>> GetPendingAsync(Guid userId);
    Task<IReadOnlyList<User>> FindUsersAsync(string term, Guid currentId);

    Task<bool> AreFriendsAsync(Guid userA, Guid userB);
    Task<bool> IsBlockedAsync(Guid blocker, Guid other);

    /* komendy */
    Task SendRequestAsync(Guid fromUserId, Guid toUserId);
    Task AcceptRequestAsync(Guid requestId);
    Task DeclineRequestAsync(Guid requestId);
    Task RemoveFriendAsync(Guid userId, Guid mateId);

    Task BlockAsync(Guid blockerId, Guid blockedId);
    Task UnblockAsync(Guid blockerId, Guid blockedId);

    /// <summary>Czy dany użytkownik jest on-line.  ⌚ Stub – w DB brak statusów.</summary>
    Task<bool> IsUserOnlineAsync(Guid userId);

    /// <summary>
    /// Dodaj znajomego jeśli go brak, w przeciwnym wypadku usuń relację.
    /// Wysyła zaproszenie jeśli użytkownicy nie są połączeni.
    /// </summary>
    Task ToggleFriendAsync(Guid userId, Guid mateId);
}
