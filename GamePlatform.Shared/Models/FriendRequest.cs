using System;

namespace GamePlatform.Shared.Models;

public class FriendRequest
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid FromUserId { get; set; }
    public User? FromUser { get; set; }     // ⬅ nawigacja

    public Guid ToUserId { get; set; }
    public User? ToUser { get; set; }     // ⬅ nawigacja

    public DateTime SentUtc { get; set; } = DateTime.UtcNow;
}
