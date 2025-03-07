using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Data;

namespace Infrastructure.Persistance;

public class TrainingRepository : ITrainingPlanRepository
{
    private readonly MyDbContext _context;

    public TrainingRepository(MyDbContext context)
    {
        _context = context;
    }

    public async Task AddTrainigPlanAsync(TrainingPlan? plan)
    {
        await _context.TrainingPlans.AddAsync(plan);
        await _context.SaveChangesAsync();
    }

    public async Task AddWorkoutsPlanAsync(List<Workout> workout)
    {
        await _context.Workouts.AddRangeAsync(workout);
        await _context.SaveChangesAsync();
    }
    
    public async Task AddTrainingSessionAsync(List<TrainingSession> sessions)
    {
        await _context.TrainingSessions.AddRangeAsync(sessions);
        await _context.SaveChangesAsync();
    }
    
}