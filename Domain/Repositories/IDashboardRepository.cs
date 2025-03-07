using Domain.Entities;

namespace Domain.Repositories;

public interface IDashboardRepository
{
    Task<TrainingPlan?> GetTrainingPlan(int userId);
}