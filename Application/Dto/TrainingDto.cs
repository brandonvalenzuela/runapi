using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto;

public class TrainingDto
{
    public Meta meta { get; set; }
    public List<Week> plan { get; set; }
}

public class Meta
{
    public string raceType { get; set; }
    public string targetPace { get; set; }
    public string goalTime { get; set; }
    public string currentBestTime { get; set; }
    public string weeklyMileage { get; set; }
    public bool hasTrainingExperience { get; set; }
    public bool isInjured { get; set; }
    public string raceDate { get; set; }
    public int weeksToRace { get; set; }
    public string primaryGoal { get; set; }
}

public class Week
{
    public int weekNumber { get; set; }
    public string focus { get; set; }
    public string weeklyVolumeTarget { get; set; }
    public List<WorkoutDTO> workouts { get; set; }
}

public class WorkoutDTO
{
    public string day { get; set; }
    public string type { get; set; }
    public string description { get; set; }
    public string intensityLevel { get; set; }
    public string additionalNotes { get; set; }
}


public class GeminiOuterResponse
{
    public List<Candidate> candidates { get; set; }
    public UsageMetadata usageMetadata { get; set; }
    public string modelVersion { get; set; }
}

public class Candidate
{
    public Content content { get; set; }
    public string finishReason { get; set; }
    public List<SafetyRating> safetyRatings { get; set; }
    public double avgLogprobs { get; set; }
}

public class Content
{
    public List<Part> parts { get; set; }
    public string role { get; set; }
}

public class Part
{
    public string text { get; set; }
}

public class UsageMetadata
{
    public int promptTokenCount { get; set; }
    public int candidatesTokenCount { get; set; }
    public int totalTokenCount { get; set; }
}

public class SafetyRating
{
    public string category { get; set; }
    public string probability { get; set; }
}
