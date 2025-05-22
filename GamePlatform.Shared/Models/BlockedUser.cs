using System;

namespace GamePlatform.Shared.Models;

public class BlockedUser
{
    public Guid BlockerId { get; set; }           // kto blokuje
    public User? Blocker { get; set; }

    public Guid BlockedId { get; set; }           // kto jest blokowany
    public User? Blocked { get; set; }

    public DateTime BlockedUtc { get; set; } = DateTime.UtcNow;
}
