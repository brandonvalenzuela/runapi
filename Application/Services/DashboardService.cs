using Application.Dto;
using Application.Interfaces;
using Domain.Entities;
using Domain.Repositories;

namespace Application.Services;

public class DashboardService : IDashboardService
{
    private readonly IDashboardRepository _dashboardRepository; 
        
    public DashboardService(IDashboardRepository dashboardRepository)
    {
        _dashboardRepository = dashboardRepository;
    }
    
    public async Task<DashboardDto>  GetUserDashboardAsync(int userId)
    {
        // 1) Obtener el plan "activo" del usuario
        //    (En un MVP, si tienes un solo plan, puedes tomar el más reciente).
        var plan = await _dashboardRepository.GetTrainingPlan(userId);
        
        if (plan == null)
        {
            // Manejar si no existe plan (podrías retornar un DashboardDto vacío o lanzar excepción)
            throw new Exception("No hay plan activo o registrado para este usuario.");
        }

        // 2) Calcular las estadísticas (Total de sesiones, cuántas completadas, etc.)
        //    Dado que cada Workout puede tener sus TrainingSessions:
        var allSessions = plan.Workouts
            .SelectMany(w => w.TrainingSessions ?? new List<TrainingSession>())
            .ToList();

        var totalSessions = allSessions.Count;
        var completedSessions = allSessions.Count(s => s.Completed);

        double completionRate = 0;
        if (totalSessions > 0)
            completionRate = Math.Round((completedSessions / (double)totalSessions) * 100, 1);

        // 3) Preparar las "próximas sesiones" (por ejemplo, las que no estén completadas y sean posteriores a 'hoy')
        var futureSessions = allSessions
            .Where(s => s.SessionDate >= DateTime.UtcNow)
            .OrderBy(s => s.SessionDate)
            .Take(allSessions.Count()) // Ej. mostrar las próximas 5
            .ToList();

        var nextSessions = new List<NextSessionDto>();
        foreach (var fs in futureSessions)
        {
            var workout = plan.Workouts.FirstOrDefault(w => w.WorkoutId == fs.WorkoutId);
            nextSessions.Add(new NextSessionDto
            {
                SessionDate = fs.SessionDate,
                WorkoutName = workout?.Name,
                Description = workout?.Description,
                Completed = fs.Completed
            });
        }

        // 4) Llenar la info de 'meta' desde 'IAOutput' o de donde la tengas
        //    (Posible parse: if plan.IAOutput contuviese el JSON, o si guardaste estos campos en TrainingPlan)
        //    Para un MVP, supongamos que NO guardamos la meta en la base, sino en IAOutput en JSON,
        //    o tienes un 'Description' con esa info. Ejemplo rápido:
        //    (O puedes deserializar plan.IAOutput a tus DTOs y extraer meta)
        
        // Por simplicidad, supongamos que en plan.Description guardaste "Maratón 5:30 min/km..."
        // Esto depende de cómo implementaste la persistencia.

        // 5) Construir el DashboardDto
        var dashboardDto = new DashboardDto
        {
            RaceType = "Maratón", // Demo. Realmente deberías extraerlo de plan o su IAOutput
            TargetPace = "5:30 min/km",
            GoalTime = "3h30m",   // Ejemplo
            WeeksToRace = 12,     // Ejemplo

            TotalSessions = totalSessions,
            CompletedSessions = completedSessions,
            CompletionRate = completionRate,
            NextWeekSessions = nextSessions
        };

        return dashboardDto;
    }
}