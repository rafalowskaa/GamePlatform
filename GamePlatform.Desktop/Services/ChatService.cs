using GamePlatform.Shared.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using GamePlatform.Shared.Data;
using GamePlatform.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace GamePlatform.Desktop.Services;

/// <summary>Najprostszy „bus” w pamięci – zastępnik SignalR.</summary>
public sealed class ChatService : IChatService
{
    private readonly AppDbContext _db;
    private readonly ConcurrentDictionary<Guid, bool> _online = new();
    public ChatService(AppDbContext db) => _db = db;

    public event Action<ChatMessage>? MessageReceived;
    public event Action<Guid, bool>? PresenceChanged;

    public async void Send(ChatMessage msg)
    {
        _db.ChatMessages.Add(msg);
        await _db.SaveChangesAsync();
        MessageReceived?.Invoke(msg);
    }

    public Task SendAsync(ChatMessage m)
    {
        Send(m);
        return Task.CompletedTask;
    }

    private async Task<IReadOnlyList<ChatMessage>> GetHistoryAsyncInternal(Guid a, Guid b, int take)
        => await _db.ChatMessages
                     .Where(m => (m.FromId == a && m.ToId == b) || (m.FromId == b && m.ToId == a))
                     .OrderByDescending(m => m.WhenUtc)
                     .Take(take)
                     .OrderBy(m => m.WhenUtc)
                     .AsNoTracking()
                     .ToListAsync();

    public Task<IReadOnlyList<ChatMessage>> GetHistoryAsync(Guid a, Guid b, int take = 100)
      => GetHistoryAsyncInternal(a, b, take);

    public IReadOnlyList<ChatMessage> GetHistory(Guid me, Guid mate, int top = 100)
        => GetHistoryAsyncInternal(me, mate, top).Result;


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
