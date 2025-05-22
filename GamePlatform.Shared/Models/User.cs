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

    public DateTime? BlockedUntilUtc { get; set; }
    public string? BlockReason { get; set; }
    public DateTime? LastLoginUtc { get; set; }

    /* —— przyjaźnie —— */
    public ICollection<Friend> FriendsInitiated { get; set; } = [];
    public ICollection<Friend> FriendsAccepted { get; set; } = [];

    /* —— blokady —— */
    public ICollection<BlockedUser> BlockedUsers { get; set; } = []; // ja blokuję
    public ICollection<BlockedUser> BlockedBy { get; set; } = []; // ktoś blokuje mnie
}
