using GamePlatform.Shared.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace GamePlatform.Desktop.Services;

/// <summary>Najprostszy „bus” w pamięci – zastępnik SignalR.</summary>
public sealed class ChatService : IChatService
{
    private readonly ConcurrentDictionary<Guid, bool> _online = new();
    private readonly ConcurrentDictionary<(Guid, Guid), List<ChatMessage>> _hist =
        new();

    public event Action<ChatMessage>? MessageReceived;
    public event Action<Guid, bool>? PresenceChanged;

    public void Send(ChatMessage msg)
    {
        var key = msg.FromId < msg.ToId ? (msg.FromId, msg.ToId) : (msg.ToId, msg.FromId);
        var list = _hist.GetOrAdd(key, _ => new());
        lock (list) list.Add(msg);          // historia

        MessageReceived?.Invoke(msg);       // echo dla UI
    }

    public Task SendAsync(ChatMessage m)
    {
        Send(m);
        return Task.CompletedTask;
    }

    public IReadOnlyList<ChatMessage> GetHistory(Guid me, Guid mate, int top = 100)
    {
        var key = me < mate ? (me, mate) : (mate, me);
        return _hist.TryGetValue(key, out var lst)
               ? lst.TakeLast(top).ToList()
               : Array.Empty<ChatMessage>();
    }

    public Task<IReadOnlyList<ChatMessage>> GetHistoryAsync(Guid a, Guid b, int take = 100)
      => Task.FromResult(GetHistory(a, b, take) as IReadOnlyList<ChatMessage>);


    /* ——— presence ——— */
    public void SetOnline(Guid id)
    {
        if (_online.TryGetValue(id, out var was) && was) return;
        _online[id] = true;
        PresenceChanged?.Invoke(id, true);
    }
    public Task SetOnlineAsync(Guid userId)
    {
        SetOnline(userId);
        return Task.CompletedTask;
    }

    public void SetOffline(Guid id)
    {
        if (_online.TryGetValue(id, out var was) && !was) return;
        _online[id] = false;
        PresenceChanged?.Invoke(id, false);
    }

    public Task SetOfflineAsync(Guid userId)
    {
        SetOffline(userId);
        return Task.CompletedTask;
    }

    public bool IsOnline(Guid id) => _online.TryGetValue(id, out var on) && on;
}
