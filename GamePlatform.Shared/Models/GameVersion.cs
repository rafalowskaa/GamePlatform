using GamePlatform.Shared.Models;

public class GameVersion
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Number { get; set; } = "1.0.0";
    public string FolderPath { get; set; } = string.Empty;

    public Guid GameId { get; set; }
    public Game? Game { get; set; }
}
