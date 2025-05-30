using System.Collections.ObjectModel;

namespace GamePlatform.Desktop.Services;

/// <summary>Bardzo proste powiadomienia UI (snackbar / toast / dialog potwierdzenia).</summary>
public interface INotificationService
{
    /// <param name="message">tekst komunikatu</param>
    /// <param name="milliseconds">czas wyświetlenia; domyślnie 3 s</param>
    void Show(string message, int milliseconds = 3000);

    /// <summary>Modalne potwierdzenie (Yes/No). Zwraca <see langword="true"/> gdy użytkownik wybierze „Tak”.</summary>
    Task<bool> ConfirmAsync(string message, int milliseconds = 0);

    /// <summary>Historia wyświetlonych komunikatów.</summary>
    ObservableCollection<string> History { get; }


    /* aliasy zgodne z istniejącym kodem */
    void Raise(string message, int ms = 3000) => Show(message, ms);
    void Info(string message, int ms = 3000) => Show(message, ms);
}