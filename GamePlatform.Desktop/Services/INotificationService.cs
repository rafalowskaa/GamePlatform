namespace GamePlatform.Desktop.Services;

/// <summary>Bardzo proste powiadomienia UI (snackbar / toast).</summary>
public interface INotificationService
{
    /// <param name="message">tekst komunikatu</param>
    /// <param name="milliseconds">czas wyświetlenia; domyślnie 3 s</param>
    void Show(string message, int milliseconds = 3000);

    /* — aliasy z istniejącego kodu — */
    void Raise(string message, int milliseconds = 3000)
        => Show(message, milliseconds);

    void Info(string message, int milliseconds = 3000)
        => Show(message, milliseconds);
}
