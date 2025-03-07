namespace Application.Dto;

public class DashboardDto
{
    /// <summary>
    /// Tipo de carrera (5K, 10K, Maratón, etc.)
    /// </summary>
    public string? RaceType { get; set; }

    /// <summary>
    /// Ritmo objetivo (ej. "5:30 min/km").
    /// </summary>
    public string? TargetPace { get; set; }

    /// <summary>
    /// Tiempo meta (ej. "3h30m", "22m30s").
    /// </summary>
    public string? GoalTime { get; set; }

    /// <summary>
    /// Semanas totales de preparación (ej. 8, 12, etc.).
    /// </summary>
    public int WeeksToRace { get; set; }

    /// <summary>
    /// Número total de sesiones de entrenamiento en el plan.
    /// </summary>
    public int TotalSessions { get; set; }

    /// <summary>
    /// Número de sesiones ya completadas por el usuario.
    /// </summary>
    public int CompletedSessions { get; set; }

    /// <summary>
    /// Porcentaje de avance (ej: 50.0 para 50%).
    /// </summary>
    public double CompletionRate { get; set; }

    /// <summary>
    /// Próximas sesiones (o las de la semana en curso) para mostrar en el dashboard.
    /// </summary>
    public List<NextSessionDto>? NextWeekSessions { get; set; }
}
