using System;

namespace GamePlatform.Shared.Models;

public class Friend
{
    public int Id { get; set; }
    public Guid OwnerId { get; set; }
    /// <summary>
    /// Identifier of the user who initiated the relation.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Navigation to the initiating user.
    /// </summary>
    public User? User { get; set; }

    /// <summary>
    /// Identifier of the second user in the relation.
    /// </summary>
    public Guid MateId { get; set; }

    /// <summary>
    /// Navigation to the second user in the relation.
    /// </summary>
    public User? Mate { get; set; }

    // ↓ pomocnicze
    public bool IsOnline { get; set; }
    public bool IsBlocked { get; set; }
}
