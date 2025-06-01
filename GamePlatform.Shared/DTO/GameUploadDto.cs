using GamePlatform.Shared.Enums;

namespace GamePlatform.Shared.DTO;

public record GameUploadDto(
    string Title,
    string Version,
    string ZipPath,
    GameCategory Category,
    GameMode Mode,
    int? MaxPlayers,
    bool IsLive,
    string? Description,
    string? CoverPath);