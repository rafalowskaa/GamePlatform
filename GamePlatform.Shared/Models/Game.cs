using GamePlatform.Shared.Enums;

namespace GamePlatform.Shared.Models;

public class Game
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }

    public GameCategory Category { get; set; }
    public GameMode Mode { get; set; }
    public int? MaxPlayers { get; set; }
    public bool IsLive { get; set; }
    public bool IsApproved { get; set; }

    public Guid SubmittedBy { get; set; }
    public DateTime SubmittedAt { get; set; }
    public User? SubmittedByUser { get; set; }

    [System.ComponentModel.DataAnnotations.Schema.NotMapped]
    public string? CoverPath { get; set; }

    public ICollection<GameVersion> Versions { get; set; } = [];
    public ICollection<Score> Scores { get; set; } = [];
    public ICollection<FavoriteGame> FavoritedBy { get; set; } = [];

    [System.ComponentModel.DataAnnotations.Schema.NotMapped]
    public bool IsFavorite { get; set; }
}