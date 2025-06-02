using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Markup;

namespace GamePlatform.Desktop.Localization
{
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
                ["DELETE"] = "USUŃ",
                ["SEND"] = "WYŚLIJ",
                ["CONVERSATIONS"] = "ROZMOWY",
                ["FRIENDS"] = "ZNAJOMI",
                ["BLOCKED"] = "ZABLOKOWANI",
                ["WRITE_MSG"] = "NAPISZ WIADOMOŚĆ",
                ["SHOW_PROFILE"] = "POKAŻ PROFIL",
                ["BLOCK"] = "ZABLOKUJ",
                ["REPORT"] = "ZGŁOŚ",
                ["REMOVE_FRIEND"] = "USUŃ ZE ZNAJOMYCH",
                ["UNBLOCK"] = "ODBLOKUJ",
                ["INVITE"] = "ZAPROŚ",
                ["SEARCH"] = "SZUKAJ",
                ["LOGIN_OR_EMAIL"] = "LOGIN LUB E-MAIL",
                ["GAME_ROOM"] = "POKÓJ GRY",
                ["CANCEL"] = "ANULUJ",
                ["SAVE"] = "ZAPISZ",
                ["ADD_GAME"] = "DODAJ GRĘ"
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
                ["DELETE"] = "DELETE",
                ["SEND"] = "SEND",
                ["CONVERSATIONS"] = "CONVERSATIONS",
                ["FRIENDS"] = "FRIENDS",
                ["BLOCKED"] = "BLOCKED",
                ["WRITE_MSG"] = "WRITE MESSAGE",
                ["SHOW_PROFILE"] = "SHOW PROFILE",
                ["BLOCK"] = "BLOCK",
                ["REPORT"] = "REPORT",
                ["REMOVE_FRIEND"] = "REMOVE FRIEND",
                ["UNBLOCK"] = "UNBLOCK",
                ["INVITE"] = "INVITE",
                ["SEARCH"] = "SEARCH",
                ["LOGIN_OR_EMAIL"] = "LOGIN OR EMAIL",
                ["GAME_ROOM"] = "GAME ROOM",
                ["CANCEL"] = "CANCEL",
                ["SAVE"] = "SAVE",
                ["ADD_GAME"] = "ADD GAME"
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

        public LocExtension() { }

        public LocExtension(string key) => Key = key;
        public string Key { get; set; } = string.Empty;

        public override object ProvideValue(IServiceProvider serviceProvider)
            => Localization.T(Key);
    }
}