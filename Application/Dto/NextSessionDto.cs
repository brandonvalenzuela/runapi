namespace Application.Dto;

public class NextSessionDto
{
    /// <summary>
    /// Fecha y hora en la que se programó la sesión.
    /// </summary>
    public DateTime SessionDate { get; set; }

    /// <summary>
    /// Nombre o tipo general del workout (p. ej. "Intervalos", "Fuerza").
    /// </summary>
    public string? WorkoutName { get; set; }

    /// <summary>
    /// Breve descripción de la sesión (km, ritmos, descansos).
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Indica si ya se completó la sesión.
    /// </summary>
    public bool Completed { get; set; }
}