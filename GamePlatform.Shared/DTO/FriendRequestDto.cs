using System;

/// <summary>
/// Dane jednego zaproszenia do znajomych (warstwa DTO – bez EF).
/// </summary>
public record FriendRequestDto(
    Guid Id,            //  ★ identyfikator zaproszenia  (NOWE)
    Guid FromUserId,
    Guid ToUserId,
    DateTime SentUtc);
