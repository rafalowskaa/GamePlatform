using GamePlatform.Shared.Models;

namespace GamePlatform.Desktop.ViewModels;

public class PlayWindowViewModel
{
    public Uri Url { get; }
    public string Title { get; }

    public PlayWindowViewModel(Game game)
    {
        var latest = game.Versions.OrderByDescending(v => v.Number).First();
        Url = new Uri(System.IO.Path.Combine(latest.FolderPath, "index.html"));
        Title = game.Title;
    }
}