using System;
using System.Linq;
using GamePlatform.Shared.Models;

namespace GamePlatform.Desktop.ViewModels
{
    public class PlayWindowViewModel
    {
        public Uri Url { get; }
        public string Title { get; }

        public PlayWindowViewModel(Game game, string htmlPath)
        {
            Url = new Uri(System.IO.Path.GetFullPath(htmlPath));
            Title = game.Title;
        }
    }
}
