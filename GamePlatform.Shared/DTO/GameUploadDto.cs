using GamePlatform.Shared.Enums;

namespace GamePlatform.Shared.DTO;

public record GameUploadDto(
    string TitlePl,
    string TitleEn,
    string Version,
    string ZipPath,
    GameCategory Category,
    GameMode Mode,
    int? MaxPlayers,
    bool IsLive,
    string? DescriptionPl,
    string? DescriptionEn,
    string? CoverPath);