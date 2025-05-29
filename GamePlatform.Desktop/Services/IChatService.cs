using GamePlatform.Shared.Models;
using System;
using System.Collections.Generic;

namespace GamePlatform.Desktop.Services;

/// <summary>Minimalny kontrakt czatu – wystarcza do lokalnego “in-memory”.</summary>
public interface IChatService
{
    event Action<ChatMessage>? MessageReceived;
    event Action<Guid, bool>? PresenceChanged;           // (userId, isOnline)

    Task SendAsync(ChatMessage m);
    Task<IReadOnlyList<ChatMessage>> GetHistoryAsync(Guid a, Guid b, int take = 100);

    Task SetOnlineAsync(Guid userId);
    Task SetOfflineAsync(Guid userId);
    bool IsOnline(Guid userId);
}

