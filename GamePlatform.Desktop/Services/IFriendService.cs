using GamePlatform.Shared.DTO;
using GamePlatform.Shared.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GamePlatform.Desktop.Services;

public interface IFriendService
{
    /* ------- zapytania ------- */
    Task<IReadOnlyList<User>> GetFriendsAsync(Guid userId);
    Task<IReadOnlyList<FriendRequestDto>> GetPendingAsync(Guid userId);

    /* ------- polecenia  ------- */
    Task SendRequestAsync(Guid fromUserId, Guid toUserId);
    Task AcceptRequestAsync(Guid requestId);
    Task DeclineRequestAsync(Guid requestId);
    Task RemoveFriendAsync(Guid userId, Guid friendId);
}
