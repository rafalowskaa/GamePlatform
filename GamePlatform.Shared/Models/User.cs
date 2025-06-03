using GamePlatform.Shared.Enums;
using System;
using System.Collections.Generic;

namespace GamePlatform.Shared.Models;

using System.ComponentModel;
using System.Runtime.CompilerServices;

public class User : INotifyPropertyChanged
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Login { get; set; } = "";
    public string Email { get; set; } = "";
    public string RecoveryMail { get; set; } = "";
    public string AvatarPath { get; set; } = "";
    public string PasswordHash { get; set; } = "";
    public UserRole Role { get; set; } = UserRole.User;

    public int Xp { get; set; } = 0;
    public UserRank Rank { get; set; } = UserRank.WoodV;

    public DateTime? BlockedUntilUtc { get; set; }
    public string? BlockReason { get; set; }
    public DateTime? LastLoginUtc { get; set; }

    /// <summary>
    /// Runtime flag used by the client application to indicate whether
    /// the user is currently online. Not mapped to the database.
    /// </summary>
    [System.ComponentModel.DataAnnotations.Schema.NotMapped]
    private bool _isOnline;
    public bool IsOnline
    {
        get => _isOnline;
        set
        {
            if (_isOnline != value)
            {
                _isOnline = value;
                OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Indicates whether the currently logged in user has this
    /// person on their friends list. Runtime only, not stored.
    /// </summary>
    [System.ComponentModel.DataAnnotations.Schema.NotMapped]
    private bool _isMyFriend;
    public bool IsMyFriend
    {
        get => _isMyFriend;
        set
        {
            if (_isMyFriend != value)
            {
                _isMyFriend = value;
                OnPropertyChanged();
            }
        }
    }

    [System.ComponentModel.DataAnnotations.Schema.NotMapped]
    private bool _hasUnread;
    public bool HasUnread
    {
        get => _hasUnread;
        set
        {
            if (_hasUnread != value)
            {
                _hasUnread = value;
                OnPropertyChanged();
            }
        }
    }



    /* —— przyjaźnie —— */
    public ICollection<Friend> FriendsInitiated { get; set; } = [];
    public ICollection<Friend> FriendsAccepted { get; set; } = [];

    public ICollection<FavoriteGame> Favorites { get; set; } = [];

    /* —— blokady —— */
    public ICollection<BlockedUser> BlockedUsers { get; set; } = []; // ja blokuję
    public ICollection<BlockedUser> BlockedBy { get; set; } = []; // ktoś blokuje mnie

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
