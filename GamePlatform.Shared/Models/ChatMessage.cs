using System;

namespace GamePlatform.Shared.Models;

public record ChatMessage(Guid From, Guid To, string Text, DateTime WhenUtc)
{
    public ChatMessage(Guid from, Guid to, string text)
        : this(from, to, text, DateTime.UtcNow) { }
}
