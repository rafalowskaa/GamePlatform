using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace GamePlatform.Desktop.ViewModels;

/// <summary>View model representing a single chat message bubble.</summary>
public partial class ChatMessageView : ObservableObject
{
    public string Text { get; }
    public bool IsMine { get; }
    public bool ContinuePrev { get; }
    public string SenderLogin { get; }
    public DateTime WhenLocal { get; }

    [ObservableProperty]
    private bool _showMeta = true;


    public ChatMessageView(string text, bool isMine, bool continuePrev, string senderLogin, DateTime whenLocal)
    {
        Text = text;
        IsMine = isMine;
        ContinuePrev = continuePrev;
        SenderLogin = senderLogin;
        WhenLocal = whenLocal;
    }
}