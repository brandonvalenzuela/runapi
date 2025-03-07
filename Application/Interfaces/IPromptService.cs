using Application.Dto;

namespace Application.Interfaces;

public interface IPromptService
{
    Task<string> GenerateTrainingPlanPrompt(UserInputDto userInput);
}