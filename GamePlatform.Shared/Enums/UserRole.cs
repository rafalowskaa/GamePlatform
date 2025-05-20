namespace GamePlatform.Shared.Enums
{
    /// <summary>
    /// Poziomy uprawnień w aplikacji.
    /// </summary>
    public enum UserRole
    {
        Guest,        // konto niezalogowane
        User,         // zwykły użytkownik
        Moderator,    // zarządza treścią / użytkownikami (bez uprawnień admina)
        Admin         // pełne uprawnienia
    }
}
