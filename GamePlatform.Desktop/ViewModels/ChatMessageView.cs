namespace GamePlatform.Desktop.ViewModels;

/// <summary>Representation of a message in the chat view.</summary>
public sealed record ChatMessageView(Guid FromId, Guid ToId, string Text, DateTime WhenUtc, bool ContinuePrev);