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

    public int Xp { get; set; } = 0;
    public UserRank Rank { get; set; } = UserRank.Bronze;

    public DateTime? BlockedUntilUtc { get; set; }
    public string? BlockReason { get; set; }
    public DateTime? LastLoginUtc { get; set; }

    /// <summary>
    /// Runtime flag used by the client application to indicate whether
    /// the user is currently online. Not mapped to the database.
    /// </summary>
    [System.ComponentModel.DataAnnotations.Schema.NotMapped]
    public bool IsOnline { get; set; }

    /// <summary>
    /// Indicates whether the currently logged in user has this
    /// person on their friends list. Runtime only, not stored.
    /// </summary>
    [System.ComponentModel.DataAnnotations.Schema.NotMapped]
    public bool IsMyFriend { get; set; }

    [System.ComponentModel.DataAnnotations.Schema.NotMapped]
    public bool HasUnread { get; set; }



    /* —— przyjaźnie —— */
    public ICollection<Friend> FriendsInitiated { get; set; } = [];
    public ICollection<Friend> FriendsAccepted { get; set; } = [];

    public ICollection<FavoriteGame> Favorites { get; set; } = [];

    /* —— blokady —— */
    public ICollection<BlockedUser> BlockedUsers { get; set; } = []; // ja blokuję
    public ICollection<BlockedUser> BlockedBy { get; set; } = []; // ktoś blokuje mnie
}
