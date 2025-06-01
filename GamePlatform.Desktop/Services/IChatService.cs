using GamePlatform.Shared.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GamePlatform.Desktop.Services;

/// <summary>Minimalny kontrakt czatu – wystarcza do lokalnego “in-memory”.</summary>
public interface IChatService
{
    event Action<ChatMessage>? MessageReceived;
    event Action<Guid, bool>? PresenceChanged;           // (userId, isOnline)
    event Action<Guid>? ConversationFlashed;

    Task SendAsync(ChatMessage m);
    Task<IReadOnlyList<ChatMessage>> GetHistoryAsync(Guid a, Guid b, int take = 100);

    Task SetOnlineAsync(Guid userId);
    Task SetOfflineAsync(Guid userId);
    bool IsOnline(Guid userId);

    Task<IReadOnlyList<ChatMessage>> GetUnseenForAsync(Guid userId);
    Task MarkAsSeenAsync(Guid userId, Guid mateId);
    Task<IReadOnlyList<Guid>> GetConversationsAsync(Guid userId);
}

