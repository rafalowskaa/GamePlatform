using System;
using System.Collections.Generic;
using System.Linq;

namespace GamePlatform.Shared.Enums;

public static class EnumExtensions
{
    public static IEnumerable<UserRole> RoleValues =>
        Enum.GetValues<UserRole>().Where(r => r is not UserRole.Guest);

    public static IEnumerable<GameCategory> CategoryValues => Enum.GetValues<GameCategory>();
    public static IEnumerable<GameMode> ModeValues => Enum.GetValues<GameMode>();
}
