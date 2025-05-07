using System;

namespace GamePlatform.Shared.Models;

/// <summary>
/// Relacja przyjaźni (symetryczna) pomiędzy dwoma użytkownikami.
/// Jeden rekord = jedno zaproszenie przyjęte.
/// </summary>
public class Friend
{
    /* —— klucz złożony —— */
    public Guid UserId { get; set; }          // zapraszający
    public Guid MateId { get; set; }          // zaakceptował

    /* —— nawigacje —— */
    public User? User { get; set; }
    public User? Mate { get; set; }
}
