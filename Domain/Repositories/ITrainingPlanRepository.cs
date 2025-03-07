using Domain.Entities;

namespace Domain.Repositories;

public interface ITrainingPlanRepository
{
    Task AddTrainigPlanAsync(TrainingPlan? plan);
    Task AddWorkoutsPlanAsync(List<Workout> workout);
    Task AddTrainingSessionAsync(List<TrainingSession> session);
}