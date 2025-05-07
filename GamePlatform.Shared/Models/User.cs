using GamePlatform.Shared.Enums;
using System;
using System.Collections.Generic;

namespace GamePlatform.Shared.Models;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Login { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string RecoveryMail { get; set; } = string.Empty;
    public string AvatarPath { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public UserRole Role { get; set; } = UserRole.User;

    /* —— nawigacje —— */
    public ICollection<Friend> FriendsInitiated { get; set; } = [];
    public ICollection<Friend> FriendsAccepted { get; set; } = [];
}
