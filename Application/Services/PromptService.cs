using Application.Dto;
using Application.Interfaces;
using Application.Wrappers;
using HandlebarsDotNet;
using Microsoft.Extensions.Configuration;

namespace Application.Services;

public class PromptService : IPromptService
{
    private readonly HandlebarsTemplate<object, object> _trainingPlanTemplate;

    public PromptService(IConfiguration configuration)
    {
        // Cargar la plantilla desde configuración
        string templateContent = configuration.GetSection("Prompts:TrainingPlan").Value;
        _trainingPlanTemplate = Handlebars.Compile(templateContent);
    }
    
    public async Task<string> GenerateTrainingPlanPrompt(UserInputDto userInput)
    {
        var data = new
        {
            raceType = userInput.RaceType,
            targetPace = userInput.TargetPace,
            goalTime = userInput.GoalTime,
            currentBestTime = userInput.CurrentBestTime,
            weeklyMileage = userInput.WeeklyMileage,
            hasTrainingExperience = userInput.HasTrainingExperience ? "Sí" : "No, solo corre ocasionalmente",
            isInjured = userInput.IsInjured ? "Sí" : "No",
            raceDate = userInput.RaceDate.ToString("dd/MM/yyyy"),
            weeksToRace = userInput.WeeksToRace,
            primaryGoal = userInput.PrimaryGoal
        };

        return _trainingPlanTemplate(data);
    }
}