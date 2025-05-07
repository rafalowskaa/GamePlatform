using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GamePlatform.Shared.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace GamePlatform.Desktop.ViewModels;

/// <summary>
/// Bardzo uproszczony „pokój gry on-line” – tylko szkic.
/// </summary>
public partial class GameRoomViewModel : ObservableObject
{
    public GameRoomViewModel(Game game) => Game = game;

    public Game Game { get; }

    public ObservableCollection<string> Chat { get; } = new();

    [ObservableProperty] private string _message = "";

    [RelayCommand]
    private Task SendAsync()
    {
        if (string.IsNullOrWhiteSpace(Message)) return Task.CompletedTask;
        Chat.Add($"Me: {Message}");
        Message = "";
        // tu należałoby wysyłać wiadomość przez WebSocket / SignalR
        return Task.CompletedTask;
    }
}
