using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistance;

public class DashboardRepository : IDashboardRepository
{
    private readonly MyDbContext _dbContext;

    public DashboardRepository(MyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TrainingPlan?> GetTrainingPlan(int userId)
    {
        //(En un MVP, si tienes un solo plan, puedes tomar el más reciente).
        return await _dbContext.TrainingPlans
            .Where(tp => tp.UserId == userId)
            // Incluir Workouts y Sessions para tener acceso a ellos
            .Include(tp => tp.Workouts)
            .ThenInclude(w => w.TrainingSessions)
            // .Include(...) si quieres incluir más
            .OrderByDescending(tp => tp.GeneratedAt) // Ej. el más reciente
            .FirstOrDefaultAsync();
    }
}