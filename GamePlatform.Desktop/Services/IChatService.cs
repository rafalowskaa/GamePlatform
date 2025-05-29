using GamePlatform.Shared.Models;
using System;
using System.Collections.Generic;

namespace GamePlatform.Desktop.Services;

/// <summary>Minimalny kontrakt czatu – wystarcza do lokalnego “in-memory”.</summary>
public interface IChatService
{
    event Action<ChatMessage>? MessageReceived;
    event Action<Guid, bool>? PresenceChanged;

    void Send(ChatMessage msg);

    void SetOnline(Guid userId);
    void SetOffline(Guid userId);
    bool IsOnline(Guid userId);

    /// <summary>Ostatnie <paramref name="last"/> wiadomości pomiędzy <paramref name="a"/> a <paramref name="b"/>.</summary>
    IReadOnlyList<ChatMessage> GetHistory(Guid a, Guid b, int last = 50);
}
