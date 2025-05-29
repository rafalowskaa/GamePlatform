using System;

namespace GamePlatform.Shared.Models;

public class ChatMessage
{
    public ChatMessage() { }

    public ChatMessage(Guid fromId, Guid toId, string text)
    {
        FromId = fromId;
        ToId = toId;
        Text = text;
    }

    public int Id { get; set; }
    public Guid FromId { get; set; }
    public Guid ToId { get; set; }
    public string Text { get; set; } = "";
    public DateTime WhenUtc { get; set; } = DateTime.UtcNow;
    public bool SeenByTo { get; set; }
}