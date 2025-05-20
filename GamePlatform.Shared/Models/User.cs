using GamePlatform.Shared.Enums;
using System;
using System.Collections.Generic;

namespace GamePlatform.Shared.Models;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Login { get; set; } = "";
    public string Email { get; set; } = "";
    public string RecoveryMail { get; set; } = "";
    public string AvatarPath { get; set; } = "";
    public string PasswordHash { get; set; } = "";
    public UserRole Role { get; set; } = UserRole.User;

    /* ——— nowe pola ——— */
    public DateTime? BlockedUntilUtc { get; set; }
    public string? BlockReason { get; set; }
    public DateTime? LastLoginUtc { get; set; }

    /* — relacje — */
    public ICollection<Friend> FriendsInitiated { get; set; } = [];
    public ICollection<Friend> FriendsAccepted { get; set; } = [];
}
