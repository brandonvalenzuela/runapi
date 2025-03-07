using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistance;

public class CalendarRepository : ICalendarRepository
{
    private readonly MyDbContext _dbContext;
    // O tus repositorios. También tu lógica para saber cuál plan es “activo”.

    public CalendarRepository(MyDbContext dbContext)
    {
        _dbContext = dbContext;
    }
   
    public async Task<TrainingPlan> GetTrainingSessionAsync(int userId)
    {
         return  await _dbContext.TrainingPlans
                .Where(tp => tp.UserId == userId)
                .Include(tp => tp.Workouts)
                .ThenInclude(w => w.TrainingSessions)
                .OrderByDescending(tp => tp.GeneratedAt)
                .FirstOrDefaultAsync();
    }
}

