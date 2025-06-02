using GamePlatform.Desktop.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using static GamePlatform.Desktop.Localization.Localization;

namespace GamePlatform.Desktop.Localization;

/// <summary>Prosty słownik tłumaczeń (bez *.resx*).</summary>
public enum Lang { Polish, English }

public static class Localization
{

    public sealed class LocBindingProxy : INotifyPropertyChanged
    {
        public Lang Current => Localization.Current;
        public event PropertyChangedEventHandler? PropertyChanged;
        internal LocBindingProxy()
        {
            Localization.LanguageChanged += (s, e) =>
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Current)));
        }
    }

    public static readonly LocBindingProxy Proxy = new();

    /* -------- powiadamianie UI -------- */
    private static Lang _current = Lang.Polish;
    public static event EventHandler? LanguageChanged;
    public static event PropertyChangedEventHandler? LanguagePropertyChanged;

    public static Lang Current
    {
        get => _current;
        set
        {
            if (_current == value) return;
            _current = value;
            LanguageChanged?.Invoke(null, EventArgs.Empty);
            LanguagePropertyChanged?.Invoke(null, new(nameof(Current)));
        }
    }

    /* -------- słownik -------- */
    private static readonly Dictionary<Lang, Dictionary<string, string>> _dict = new()
    {
        /* =============  POLSKI  ============= */
        [Lang.Polish] = new(StringComparer.OrdinalIgnoreCase)
        {
            /* --- główne zakładki / przyciski --- */
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
            ["PLAY"] = "GRAJ",
            ["APPROVE"] = "ZAAKCEPTUJ",
            ["REQUEST_FIX"] = "ZGŁOŚ POPRAWĘ",
            ["DELETE"] = "USUŃ",
            ["SEND"] = "WYŚLIJ",
            ["CANCEL"] = "ANULUJ",
            ["SAVE"] = "ZAPISZ",
            ["ADD_GAME"] = "DODAJ GRĘ",
            ["CHOOSE_GAME"] = "WYBIERZ GRĘ",
            ["CLEAR"] = "WYCZYŚĆ",
            ["SEARCH"] = "SZUKAJ",
            ["LOGIN_OR_EMAIL"] = "LOGIN LUB E-MAIL",
            ["REGISTER"] = "REJESTRACJA",
            ["LOGIN"] = "LOGIN",
            ["NEW_ACCOUNT"] = "NOWE KONTO",
            ["BACK_TO_LOGIN"] = "LOGOWANIE",

            /* --- kolumny / pola --- */
            ["FROM"] = "OD",
            ["WHEN"] = "KIEDY",
            ["TITLE"] = "TYTUŁ",
            ["VERSION"] = "WERSJA",
            ["PLAYER"] = "GRACZ",
            ["ROLE"] = "ROLA",
            ["DATE"] = "DATA",
            ["ACTIONS"] = "AKCJE",
            ["CATEGORY"] = "KATEGORIA",
            ["STATUS"] = "STATUS",
            ["REPORTER"] = "ZGŁASZAJĄCY",
            ["ACCUSED"] = "OSKARŻONY",
            ["BLOCK_UNTIL"] = "BLOKADA",
            ["LAST_LOGIN"] = "OST. LOG.",
            ["SCORE"] = "WYNIK",
            ["GAME_MODE"] = "TRYB GRY",
            ["MAX_PLAYERS"] = "MAX GRACZY",
            ["LIVE"] = "NA ŻYWO",
            ["DESCRIPTION"] = "OPIS",
            ["GAME_IMAGE_OPT"] = "OBRAZ GRY (OPT.)",
            ["ZIP_FILE"] = "PLIK ZIP",
            ["EMAIL"] = "E-MAIL",
            ["RECOVERY_EMAIL"] = "E-MAIL DO ODZYSKANIA",
            ["NEW_PASSWORD"] = "NOWE HASŁO",
            ["PASSWORD"] = "HASŁO",
            ["CONFIRM_PASSWORD"] = "POTWÓRZ HASŁO",
            ["REMEMBER_ME"] = "ZAPAMIĘTAJ MNIE",

            /* --- komunikaty --- */
            ["ERROR"] = "BŁĄD",
            ["CRITICAL_ERROR"] = "BŁĄD KRYTYCZNY",
            ["INVALID_PASSWORD"] = "NIEPRAWIDŁOWE HASŁO.",
            ["EMPTY_PASSWORD"] = "HASŁO NIE MOŻE BYĆ PUSTE.",
            ["NO_USER_CONTEXT"] = "BRAK KONTEKSTU UŻYTKOWNIKA.",
            ["FILE_INVALID"] = "PLIK MUSI BYĆ JPG/PNG <1 MB",

            /* --- operacje na znajomych / czacie --- */
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
            ["SEARCH_PLAYER"] = "SZUKAJ GRACZA",
            ["ADD_FRIEND"] = "DODAJ ZNAJOMEGO",
            ["CONFIRM"] = "POTWIERDŹ",
            ["FAVORITE_GAMES"] = "ULUBIONE GRY",
            ["WELCOME_PLAYER"] = "Witaj {0}, w co dziś zagramy?",

            ["STATUS_PENDING"] = "Oczekuje",
            ["STATUS_IN_REVIEW"] = "W trakcie",
            ["STATUS_CLOSED"] = "Zamknięte",
            ["STATUS_WARNED"] = "Ostrzeżono",
            ["STATUS_BLOCKED"] = "Zablokowano",
            ["STATUS_CANCELLED"] = "Anulowano",
            ["STATUS_NO_ACTION"] = "Niepodjęto akcji",
            ["STATUS_WARNING_SENT"] = "Wysłano ostrzeżenie",

            /* --- potwierdzenia / pytania --- */
            ["BLOCK_USER_ASK"] = "ZABLOKOWAĆ UŻYTKOWNIKA {0}?",
            ["DELETE_USER_ASK"] = "USUNĄĆ UŻYTKOWNIKA „{0}”?",
            ["ENTER_PASSWORD_PROMPT"] = "ABY POTWIERDZIĆ, WPISZ HASŁO:",

            /* --- inne --- */
            ["GAME_ROOM"] = "POKÓJ GRY",
            ["CHANGE_PICTURE"] = "ZMIEŃ ZDJĘCIE",
            ["ALL"] = "WSZYSCY",
            ["WARN"] = "OSTRZEŻ",
            ["EDIT"] = "EDYTUJ",
            ["DELETE_USER"] = "USUŃ UŻYTKOWNIKA",
            ["EDIT_USER"] = "EDYTUJ UŻYTKOWNIKA",
            ["REASON"] = "POWÓD",
            ["BLOCK_REASON"] = "POWÓD BLOKADY",
            ["SEARCH_IN_CHAT"] = "SZUKAJ W CZACIE",
            ["CREATE_ACCOUNT"] = "UTWÓRZ KONTO",
            ["ONLY_BLOCKED"] = "TYLKO Z BLOKADĄ",
            ["ZIP_INFO"] = "Archiwum ZIP powinno zawierać plik index.html lub plik .exe w głównym folderze lub podfolderach."
        },

        /* =============  ENGLISH  ============= */
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
            ["WELCOME_PLAYER"] = "Welcome {0}, what shall we play today?",

            ["STATUS_PENDING"] = "Pending",
            ["STATUS_IN_REVIEW"] = "In Review",
            ["STATUS_CLOSED"] = "Closed",
            ["STATUS_WARNED"] = "Warned",
            ["STATUS_BLOCKED"] = "Blocked",
            ["STATUS_CANCELLED"] = "Cancelled",
            ["STATUS_NO_ACTION"] = "No action taken",
            ["STATUS_WARNING_SENT"] = "Warning sent",
            ["ERROR"] = "ERROR",
            ["CRITICAL_ERROR"] = "CRITICAL ERROR",
            ["ALL"] = "ALL",
            ["ADD_FRIEND"] = "ADD FRIEND",
            ["CONFIRM"] = "CONFIRM",
            ["INVALID_PASSWORD"] = "INVALID PASSWORD.",
            ["BLOCK_USER_ASK"] = "BLOCK USER {0}?",
            ["DELETE_USER_ASK"] = "DELETE USER \"{0}\"?",
            ["NO_USER_CONTEXT"] = "NO LOGGED USER CONTEXT.",
            ["FILE_INVALID"] = "FILE MUST BE JPG/PNG <1 MB",
            ["EMPTY_PASSWORD"] = "PASSWORD CANNOT BE EMPTY.",
            ["CONFIRM_PASSWORD"] = "CONFIRM PASSWORD",
            ["ENTER_PASSWORD_PROMPT"] = "TO CONFIRM, ENTER YOUR PASSWORD:",
            ["PASSWORD"] = "PASSWORD",
            ["REMEMBER_ME"] = "REMEMBER ME",
            ["REGISTER"] = "REGISTER",
            ["NEW_ACCOUNT"] = "NEW ACCOUNT",
            ["BACK_TO_LOGIN"] = "BACK TO LOGIN",
            ["SEARCH_IN_CHAT"] = "SEARCH CHAT",
            ["SEARCH_PLAYER"] = "SEARCH PLAYER",
            ["EDIT_USER"] = "EDIT USER",
            ["REASON"] = "REASON",
            ["BLOCK_REASON"] = "BLOCK REASON",
            ["CREATE_ACCOUNT"] = "CREATE ACCOUNT",
            ["ONLY_BLOCKED"] = "ONLY BLOCKED",
            ["ZIP_INFO"] = "ZIP archive should contain an index.html or .exe file in the root folder or its subfolders."
        }
    };

    /* -------- API -------- */
    public static string T(string key) => _dict.TryGetValue(Current, out var l) && l.TryGetValue(key, out var v) ? v.ToUpperInvariant() : key.ToUpperInvariant();
    public static string TRaw(string key) => _dict.TryGetValue(Current, out var l) && l.TryGetValue(key, out var v) ? v : key;
}

/*==========  MarkupExtension  ==========*/

public sealed class LocExtension : MarkupExtension
{
    public LocExtension() { }
    public LocExtension(string key) => Key = key;

    [ConstructorArgument("Key")]
    public string Key { get; set; } = string.Empty;

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        var binding = new Binding(nameof(LocBindingProxy.Current))
        {
            Source = Localization.Proxy,
            Converter = new LocConverter(),
            ConverterParameter = Key,
            Mode = BindingMode.OneWay
        };
        return binding.ProvideValue(serviceProvider);
    }
}

