using GamePlatform.Shared.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

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

    public void SetOnline(Guid id)
    {
        if (_online.TryAdd(id, true))
            PresenceChanged?.Invoke(id, true);
    }

    public void SetOffline(Guid id)
    {
        if (_online.TryRemove(id, out _))
            PresenceChanged?.Invoke(id, false);
    }

    public bool IsOnline(Guid id) => _online.ContainsKey(id);

    public IReadOnlyList<ChatMessage> GetHistory(Guid a, Guid b, int last = 50) =>
        _log.Where(m => (m.FromId == a && m.ToId == b) || (m.FromId == b && m.ToId == a))
            .OrderByDescending(m => m.WhenUtc)
            .Take(last)
            .OrderBy(m => m.WhenUtc)
            .ToList();
}
