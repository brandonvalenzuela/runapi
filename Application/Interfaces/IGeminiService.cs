namespace Application.Services;

public interface IGeminiService
{
    Task<string> GenerateTrainingPlan(string prompt);
}