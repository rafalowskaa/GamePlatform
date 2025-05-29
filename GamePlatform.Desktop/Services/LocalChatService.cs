using GamePlatform.Shared.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamePlatform.Desktop.Services;

/// <summary>Jedno-procesowy, współdzielony czat (hash-maps + zdarzenia).</summary>
public sealed class LocalChatService : IChatService
{
    public event Action<ChatMessage>? MessageReceived;
    public event Action<Guid, bool>? PresenceChanged;

    private readonly ConcurrentDictionary<Guid, bool> _online = new();
    private readonly ConcurrentBag<ChatMessage> _log = new();

    /* ---------------- API ---------------- */

    public void Send(ChatMessage msg)
    {
        _log.Add(msg);
        MessageReceived?.Invoke(msg);
    }

    public Task SendAsync(ChatMessage m)
    {
        Send(m);
        return Task.CompletedTask;
    }

    public void SetOnline(Guid id)
    {
        if (_online.TryAdd(id, true))
            PresenceChanged?.Invoke(id, true);
    }

    public Task SetOnlineAsync(Guid userId)
    {
        SetOnline(userId);
        return Task.CompletedTask;
    }

    public void SetOffline(Guid id)
    {
        if (_online.TryRemove(id, out _))
            PresenceChanged?.Invoke(id, false);
    }

    public Task SetOfflineAsync(Guid userId)
    {
        SetOffline(userId);
        return Task.CompletedTask;
    }

    public bool IsOnline(Guid id) => _online.ContainsKey(id);

    public IReadOnlyList<ChatMessage> GetHistory(Guid a, Guid b, int last = 50) =>
        _log.Where(m => (m.FromId == a && m.ToId == b) || (m.FromId == b && m.ToId == a))
            .OrderByDescending(m => m.WhenUtc)
            .Take(last)
            .OrderBy(m => m.WhenUtc)
            .ToList();

    public Task<IReadOnlyList<ChatMessage>> GetHistoryAsync(Guid a, Guid b, int take = 100)
        => Task.FromResult(GetHistory(a, b, take) as IReadOnlyList<ChatMessage>);
}
