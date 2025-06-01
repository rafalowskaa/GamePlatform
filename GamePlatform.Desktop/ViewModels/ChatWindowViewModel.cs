using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GamePlatform.Desktop.Services;
using GamePlatform.Shared.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System;

namespace GamePlatform.Desktop.ViewModels;

/// <summary>VM pojedynczego okna czatu (me ⇄ mate).</summary>
public partial class ChatWindowViewModel : ObservableObject, IDisposable
{
    private readonly IChatService _chat;

    public User Me { get; }
    public User Mate { get; }

    

    public ObservableCollection<ChatMessageView> Messages { get; } = new();

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

    public void Dispose()
    {
        _chat.MessageReceived -= OnIncoming;
    }

    private ChatMessageView CreateView(ChatMessage m, ChatMessageView? prev)
    {
        bool mine = m.FromId == Me.Id;
        bool cont = prev != null && prev.IsMine == mine;
        string login = mine ? Me.Login : Mate.Login;
        return new ChatMessageView(m.Text, mine, cont, login, m.WhenUtc.ToLocalTime());
    }

    private async Task LoadHistoryAsync()
    {
        var history = await _chat.GetHistoryAsync(Me.Id, Mate.Id);
        ChatMessageView? prev = null;
        foreach (var m in history)
        {
            var view = CreateView(m, prev);

            if (prev != null && prev.IsMine == view.IsMine)
                prev.ShowMeta = false;

            Messages.Add(view);
            prev = view;
        }
        await _chat.MarkAsSeenAsync(Me.Id, Mate.Id);
    }

    /* ---- incoming ---- */
    private void OnIncoming(ChatMessage msg)
    {
        if ((msg.FromId == Me.Id && msg.ToId == Mate.Id) ||
            (msg.FromId == Mate.Id && msg.ToId == Me.Id))
        {
            var prev = Messages.Count > 0 ? Messages[^1] : null;
            var view = CreateView(msg, prev);

            App.Current.Dispatcher.Invoke(() =>
            {
                if (prev != null && prev.IsMine == view.IsMine)
                    prev.ShowMeta = false;
                Messages.Add(view);
            });
            if (msg.ToId == Me.Id)
                _ = _chat.MarkAsSeenAsync(Me.Id, Mate.Id);
        }
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
        await _chat.MarkAsSeenAsync(Me.Id, Mate.Id);

    }
}
