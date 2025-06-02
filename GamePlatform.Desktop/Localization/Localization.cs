using GamePlatform.Desktop.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace GamePlatform.Desktop.Localization
{
    public enum Lang { Polish, English }
    public static class Localization
    {
        private static Lang _current = Lang.Polish;
        public static event EventHandler? LanguageChanged;
        public static event PropertyChangedEventHandler? PropertyChanged;

        public static Lang Current
        {
            get => _current;
            set
            {
                if (_current == value) return;
                _current = value;
                LanguageChanged?.Invoke(null, EventArgs.Empty);
                PropertyChanged?.Invoke(null, new PropertyChangedEventArgs(nameof(Current)));
            }
        }

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
                ["LOGIN"] = "LOGIN",
                ["ROLE"] = "ROLA",
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
                ["ADD_GAME"] = "DODAJ GRĘ",
                ["CHOOSE_GAME"] = "WYBIERZ GRĘ",
                ["SCORE"] = "WYNIK",
                ["EDIT"] = "EDYTUJ",
                ["DELETE_USER"] = "USUŃ UŻYTKOWNIKA",
                ["WARN"] = "OSTRZEŻ",
                ["CATEGORY"] = "KATEGORIA",
                ["STATUS"] = "STATUS",
                ["REPORTER"] = "ZGŁASZAJĄCY",
                ["ACCUSED"] = "OSKARŻONY",
                ["BLOCK_UNTIL"] = "BLOKADA",
                ["LAST_LOGIN"] = "OSTATNIE LOGOWANIE",
                ["CLEAR"] = "WYCZYŚĆ",
                ["GAME_MODE"] = "TRYB GRY",
                ["MAX_PLAYERS"] = "MAX GRACZY",
                ["LIVE"] = "NA ŻYWO",
                ["DESCRIPTION"] = "OPIS",
                ["GAME_IMAGE_OPT"] = "OBRAZ GRY (OPCJONALNIE)",
                ["ZIP_FILE"] = "PLIK ZIP",
                ["CHANGE_PICTURE"] = "ZMIEŃ ZDJĘCIE",
                ["EMAIL"] = "E-MAIL",
                ["RECOVERY_EMAIL"] = "E-MAIL ODZYSKU",
                ["NEW_PASSWORD"] = "NOWE HASŁO",
                ["FAVORITE_GAMES"] = "ULUBIONE GRY"
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
                ["LOGIN"] = "LOGIN",
                ["ROLE"] = "ROLE",
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
                ["ADD_GAME"] = "ADD GAME",
                ["CHOOSE_GAME"] = "CHOOSE GAME",
                ["SCORE"] = "SCORE",
                ["EDIT"] = "EDIT",
                ["DELETE_USER"] = "DELETE USER",
                ["WARN"] = "WARN",
                ["CATEGORY"] = "CATEGORY",
                ["STATUS"] = "STATUS",
                ["REPORTER"] = "REPORTER",
                ["ACCUSED"] = "ACCUSED",
                ["BLOCK_UNTIL"] = "BLOCK UNTIL",
                ["LAST_LOGIN"] = "LAST LOGIN",
                ["CLEAR"] = "CLEAR",
                ["GAME_MODE"] = "GAME MODE",
                ["MAX_PLAYERS"] = "MAX PLAYERS",
                ["LIVE"] = "LIVE",
                ["DESCRIPTION"] = "DESCRIPTION",
                ["GAME_IMAGE_OPT"] = "GAME IMAGE (OPTIONAL)",
                ["ZIP_FILE"] = "ZIP FILE",
                ["CHANGE_PICTURE"] = "CHANGE PICTURE",
                ["EMAIL"] = "E-MAIL",
                ["RECOVERY_EMAIL"] = "RECOVERY EMAIL",
                ["NEW_PASSWORD"] = "NEW PASSWORD",
                ["FAVORITE_GAMES"] = "FAVORITE GAMES"
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
           => new Binding(nameof(Localization.Current))
           {
               Source = typeof(Localization),
               Converter = new LocConverter(),
               ConverterParameter = Key
           };
    }
}