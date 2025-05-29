public class ChatMessage
{
    public int Id { get; set; }
    public Guid From { get; set; }
    public Guid To { get; set; }
    public string Text { get; set; } = "";
    public DateTime SentUtc { get; set; } = DateTime.UtcNow;
    public bool SeenByTo { get; set; }
}
