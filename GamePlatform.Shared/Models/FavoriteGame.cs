using System;

namespace GamePlatform.Shared.Models;

public class FavoriteGame
{
    public Guid UserId { get; set; }
    public Guid GameId { get; set; }

    public User? User { get; set; }
    public Game? Game { get; set; }
}