namespace GamePlatform.Shared.Models;

public class Score
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Player { get; set; } = string.Empty;
    public int Value { get; set; }

    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

    // FK
    public Guid GameId { get; set; }
    public Game? Game { get; set; }
}
