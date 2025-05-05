using System;
using System.Collections.Generic;

namespace GamePlatform.Shared.Enums;

public static class EnumExtensions
{
    public static IEnumerable<GameCategory> GetValues => Enum.GetValues<GameCategory>();
    public static IEnumerable<GameMode> GetValues => Enum.GetValues<GameMode>();
}