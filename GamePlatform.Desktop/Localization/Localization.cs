using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Markup;

namespace GamePlatform.Desktop.Localization;

public enum Lang { Polish, English }

public static class Localization
{
    public static Lang Current { get; set; } = Lang.Polish;

    private static readonly Dictionary<Lang, Dictionary<string, string>> _dict = new()
    {
        [Lang.Polish] = new(StringComparer.OrdinalIgnoreCase)
        {
            ["GAMES"] = "GRY",
            ["PROFILE"] = "PROFIL",
            ["STATS"] = "STATYSTYKI",
            ["NOTIFICATIONS"] = "POWIADOMIENIA",
            ["PENDING"] = "DO AKCEPTACJI",
            ["REPORTS"] = "ZGŁOSZENIA",
            ["USERS"] = "UŻYTKOWNICY",
            ["LOGOUT"] = "WYLOGUJ",
            ["REFRESH"] = "ODŚWIEŻ",
            ["ACCEPT"] = "AKCEPTUJ",
            ["DECLINE"] = "ODRZUĆ",
            ["FROM"] = "OD",
            ["WHEN"] = "KIEDY",
            ["TITLE"] = "TYTUŁ",
            ["VERSION"] = "WERSJA",
            ["PLAYER"] = "GRACZ",
            ["DATE"] = "DATA",
            ["ACTIONS"] = "AKCJE",
            ["PLAY"] = "GRAJ",
            ["APPROVE"] = "ZAAKCEPTUJ",
            ["REQUEST_FIX"] = "ZGŁOŚ POPRAWKI",
            ["DELETE"] = "USUŃ"
        },
        [Lang.English] = new(StringComparer.OrdinalIgnoreCase)
        {
            ["GAMES"] = "GAMES",
            ["PROFILE"] = "PROFILE",
            ["STATS"] = "STATS",
            ["NOTIFICATIONS"] = "NOTIFICATIONS",
            ["PENDING"] = "PENDING",
            ["REPORTS"] = "REPORTS",
            ["USERS"] = "USERS",
            ["LOGOUT"] = "LOGOUT",
            ["REFRESH"] = "REFRESH",
            ["ACCEPT"] = "ACCEPT",
            ["DECLINE"] = "DECLINE",
            ["FROM"] = "FROM",
            ["WHEN"] = "WHEN",
            ["TITLE"] = "TITLE",
            ["VERSION"] = "VERSION",
            ["PLAYER"] = "PLAYER",
            ["DATE"] = "DATE",
            ["ACTIONS"] = "ACTIONS",
            ["PLAY"] = "PLAY",
            ["APPROVE"] = "APPROVE",
            ["REQUEST_FIX"] = "REQUEST FIX",
            ["DELETE"] = "DELETE"
        }
    };

    public static string T(string key)
    {
        if (_dict.TryGetValue(Current, out var lang) && lang.TryGetValue(key.ToUpperInvariant(), out var value))
            return value.ToUpperInvariant();
        return key.ToUpperInvariant();
    }
}

public sealed class LocExtension : MarkupExtension
{
    public string Key { get; set; } = string.Empty;

    public override object ProvideValue(IServiceProvider serviceProvider)
        => Localization.T(Key);
}