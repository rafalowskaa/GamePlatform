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
                ["FAVORITE_GAMES"] = "ULUBIONE GRY",
                ["ERROR"] = "BŁĄD",
                ["CRITICAL_ERROR"] = "BŁĄD KRYTYCZNY",
                ["ALL"] = "WSZYSCY",
                ["ADD_FRIEND"] = "DODAJ ZNAJOMEGO",
                ["CONFIRM"] = "POTWIERDŹ",
                ["INVALID_PASSWORD"] = "NIEPRAWIDŁOWE HASŁO.",
                ["BLOCK_USER_ASK"] = "CZY NA PEWNO ZABLOKOWAĆ UŻYTKOWNIKA {0}?",
                ["DELETE_USER_ASK"] = "CZY NA PEWNO CHCESZ USUNĄĆ UŻYTKOWNIKA „{0}”?",
                ["NO_USER_CONTEXT"] = "BRAK KONTEKSTU ZALOGOWANEGO UŻYTKOWNIKA.",
                ["FILE_INVALID"] = "PLIK MUSI BYĆ JPG/PNG I MNIEJSZY NIŻ 1MB",
                ["EMPTY_PASSWORD"] = "HASŁO NIE MOŻE BYĆ PUSTE.",
                ["CONFIRM_PASSWORD"] = "POTWIERDŹ HASŁO",
                ["ENTER_PASSWORD_PROMPT"] = "ABY POTWIERDZIĆ OPERACJĘ, WPISZ SWOJE HASŁO:",
                ["PASSWORD"] = "HASŁO",
                ["REMEMBER_ME"] = "ZAPAMIĘTAJ MNIE",
                ["REGISTER"] = "REJESTRACJA",
                ["NEW_ACCOUNT"] = "NOWE KONTO",
                ["BACK_TO_LOGIN"] = "LOGOWANIE",
                ["CREATE_ACCOUNT"] = "UTWÓRZ KONTO",
                ["SEARCH_IN_CHAT"] = "SZUKAJ W CZACIE",
                ["SEARCH_PLAYER"] = "WYSZUKAJ GRACZA",
                ["EDIT_USER"] = "EDYTUJ UŻYTKOWNIKA",
                ["REASON"] = "POWÓD",
                ["BLOCK_REASON"] = "POWÓD BLOKADY",
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
                ["FAVORITE_GAMES"] = "FAVORITE GAMES",
                ["ERROR"] = "ERROR",
                ["CRITICAL_ERROR"] = "CRITICAL ERROR",
                ["ALL"] = "ALL",
                ["ADD_FRIEND"] = "ADD FRIEND",
                ["CONFIRM"] = "CONFIRM",
                ["INVALID_PASSWORD"] = "INVALID PASSWORD.",
                ["BLOCK_USER_ASK"] = "ARE YOU SURE YOU WANT TO BLOCK USER {0}?",
                ["DELETE_USER_ASK"] = "ARE YOU SURE YOU WANT TO DELETE USER \"{0}\"?",
                ["NO_USER_CONTEXT"] = "NO LOGGED USER CONTEXT.",
                ["FILE_INVALID"] = "FILE MUST BE JPG/PNG AND SMALLER THAN 1MB",
                ["EMPTY_PASSWORD"] = "PASSWORD CANNOT BE EMPTY.",
                ["CONFIRM_PASSWORD"] = "CONFIRM PASSWORD",
                ["ENTER_PASSWORD_PROMPT"] = "TO CONFIRM THE ACTION, ENTER YOUR PASSWORD:",
                ["PASSWORD"] = "PASSWORD",
                ["REMEMBER_ME"] = "REMEMBER ME",
                ["REGISTER"] = "REGISTER",
                ["NEW_ACCOUNT"] = "NEW ACCOUNT",
                ["BACK_TO_LOGIN"] = "BACK TO LOGIN",
                ["CREATE_ACCOUNT"] = "CREATE ACCOUNT",
                ["SEARCH_IN_CHAT"] = "SEARCH CHAT",
                ["SEARCH_PLAYER"] = "SEARCH PLAYER",
                ["EDIT_USER"] = "EDIT USER",
                ["REASON"] = "REASON",
                ["BLOCK_REASON"] = "BLOCK REASON",
            }
        };

        public static string T(string key)
        {
            if (_dict.TryGetValue(Current, out var lang) && lang.TryGetValue(key.ToUpperInvariant(), out var value))
                return value.ToUpperInvariant();
            return key.ToUpperInvariant();
        }

        public static string TRaw(string key)
        {
            if (_dict.TryGetValue(Current, out var lang) && lang.TryGetValue(key.ToUpperInvariant(), out var value))
                return value;
            return key;
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