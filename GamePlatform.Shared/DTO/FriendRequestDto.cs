using System;

namespace GamePlatform.Shared.DTO;

/// <summary>
///  Dane pojedynczego zaproszenia do znajomych (bez logiki EF).
/// </summary>
public record FriendRequestDto(Guid FromUserId, Guid ToUserId, DateTime SentUtc);
