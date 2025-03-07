namespace Application.Dto;

public class UserInputDto
{
    public int UserId { get; set; } // Asegúrate de recibir el UserId correctamente
    public string RaceType { get; set; }
    public string TargetPace { get; set; }
    public string GoalTime { get; set; }
    public string CurrentBestTime { get; set; }
    public string WeeklyMileage { get; set; }
    public bool HasTrainingExperience { get; set; }
    public bool IsInjured { get; set; }
    public DateTime RaceDate { get; set; }
    public int WeeksToRace { get; set; }
    public string PrimaryGoal { get; set; }
}
