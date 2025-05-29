using System;

namespace GamePlatform.Shared.Models;

public class Friend
{
    public int Id { get; set; }
    public Guid OwnerId { get; set; }
    public Guid MateId { get; set; }

    // ↓ pomocnicze
    public bool IsOnline { get; set; }
    public bool IsBlocked { get; set; }
}
