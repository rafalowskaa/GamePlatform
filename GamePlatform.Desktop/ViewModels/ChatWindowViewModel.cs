using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GamePlatform.Desktop.Services;
using GamePlatform.Shared.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace GamePlatform.Desktop.ViewModels;

/// <summary>VM pojedynczego okna czatu (me ⇄ mate).</summary>
public partial class ChatWindowViewModel : ObservableObject
{
    private readonly IChatService _chat;

    public User Me { get; }
    public User Mate { get; }

    public ObservableCollection<ChatMessage> Messages { get; } = new();

    /* ==========  BINDINGS  ========== */
    [ObservableProperty] private string _text = "";

    public ChatWindowViewModel(IChatService chat, User me, User mate)
    {
        _chat = chat;
        Me = me;
        Mate = mate;

        _ = LoadHistoryAsync();

        _chat.MessageReceived += OnIncoming;
        _ = _chat.SetOnlineAsync(Me.Id);
    }

    private async Task LoadHistoryAsync()
    {
        var history = await _chat.GetHistoryAsync(Me.Id, Mate.Id);
        foreach (var m in history)
            Messages.Add(m);
    }

    /* ---- incoming ---- */
    private void OnIncoming(ChatMessage msg)
    {
        if ((msg.FromId == Me.Id && msg.ToId == Mate.Id) ||
            (msg.FromId == Mate.Id && msg.ToId == Me.Id))
            App.Current.Dispatcher.Invoke(() => Messages.Add(msg));
    }

    /* ---- outgoing ---- */
    [RelayCommand]
    private async Task SendAsync()
    {
        var txt = Text?.Trim();
        if (string.IsNullOrEmpty(txt)) return;

        var msg = new ChatMessage(Me.Id, Mate.Id, txt);
        await _chat.SendAsync(msg);
        Text = "";
        
    }
}
