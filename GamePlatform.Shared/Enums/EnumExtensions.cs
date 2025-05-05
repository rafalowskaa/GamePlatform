using System;
using System.Collections.Generic;

namespace GamePlatform.Shared.Enums;

public static class EnumExtensions
{
    public static IEnumerable<GameCategory> CategoryValues => Enum.GetValues<GameCategory>();
    public static IEnumerable<GameMode> ModeValues => Enum.GetValues<GameMode>();
}
