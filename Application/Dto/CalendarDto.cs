namespace Application.Dto;

public class CalendarDto
{
    public int Year { get; set; }
    public int Month { get; set; }
    public List<CalendarDayDto> Days { get; set; } = new();
}

public class CalendarDayDto
{
    public DateTime Date { get; set; }
    public bool IsCurrentMonth { get; set; }     // Para marcar si es del mes o está en la rejilla como "día gris"
    public List<CalendarSessionDto> Sessions { get; set; } = new();
}

public class CalendarSessionDto
{
    public int SessionId { get; set; }           // (opcional) para updates
    public string WorkoutName { get; set; }
    public string Description { get; set; }
    public bool Completed { get; set; }
}
