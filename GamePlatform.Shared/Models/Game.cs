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

    public ICollection<GameVersion> Versions { get; set; } = [];
    public ICollection<Score> Scores { get; set; } = [];
}