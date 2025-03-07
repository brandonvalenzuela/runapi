using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dto;
using Application.Interfaces;
using Domain.Entities;
using Domain.Repositories;

namespace Application.Services;

public class CalendarService : ICalendarService
{ 
    private readonly ICalendarRepository _calendarRepository; 
        
    public CalendarService(ICalendarRepository calendarRepository)
    {
        _calendarRepository = calendarRepository;
    }
    
    public async Task<CalendarDto> GetUserCalendarAsync(int userId, int year, int month)
    {
        // 1) Determinar el rango de fechas que vamos a cubrir en el calendario
        //    Ejemplo: primer día del mes -> dateStart
        //             último día del mes -> dateEnd
        //    Y luego extenderlo a lunes-domingo si lo deseas.
        var firstOfMonth = new DateTime(year, month, 1);
        var lastOfMonth = firstOfMonth.AddMonths(1).AddDays(-1); // Último día del mes

        // Ajustar dateStart a lunes (o domingo, según tu preferencia):
        var dateStart = firstOfMonth;
        while (dateStart.DayOfWeek != DayOfWeek.Monday)
            dateStart = dateStart.AddDays(-1);

        // Ajustar dateEnd a domingo:
        var dateEnd = lastOfMonth;
        while (dateEnd.DayOfWeek != DayOfWeek.Sunday)
            dateEnd = dateEnd.AddDays(1);

        // 2) Obtener de la base de datos las sesiones del usuario en ese rango
        //    Asumiendo que TrainingSession tiene SessionDate
        //    y corresponde al plan "activo" o al plan más reciente. Ajusta según tu modelo.
        var trainingPlan = await _calendarRepository.GetTrainingSessionAsync(userId);

        if (trainingPlan == null)
        {
            // Si no hay plan, retornar un calendario vacío (o lanzar excepción)
            return new CalendarDto
            {
                Year = year,
                Month = month,
                Days = new List<CalendarDayDto>() // sin entrenos
            };
        }

        // Filtrar sesiones del rango
        // (Workouts -> Sessions)
        var sessionsInRange = trainingPlan.Workouts
            .SelectMany(w => w.TrainingSessions ?? new List<TrainingSession>())
            .Where(s => s.SessionDate >= dateStart && s.SessionDate <= dateEnd)
            .ToList();

        // 3) Construir la estructura "CalendarDto"
        var calendarDto = new CalendarDto
        {
            Year = year,
            Month = month,
        };

        // 4) Rellenar "Days" desde dateStart hasta dateEnd
        var currentDate = dateStart;
        while (currentDate <= dateEnd)
        {
            // Filtrar las sesiones de ese día
            var daySessions = sessionsInRange
                .Where(s => s.SessionDate.Date == currentDate.Date)
                .ToList();

            // Mapear a CalendarSessionDto
            var sessionDtos = daySessions
                .Select(s =>
                {
                    var workout = trainingPlan.Workouts.FirstOrDefault(w => w.WorkoutId == s.WorkoutId);
                    return new CalendarSessionDto
                    {
                        SessionId = s.TrainingSessionId,
                        WorkoutName = workout?.Name,
                        Description = workout?.Description,
                        Completed = s.Completed
                    };
                })
                .ToList();

            var dayDto = new CalendarDayDto
            {
                Date = currentDate,
                // "IsCurrentMonth" es true si el mes de currentDate == month
                // útil para "grisar" los días que no son del mes real
                IsCurrentMonth = (currentDate.Month == month),
                Sessions = sessionDtos
            };

            calendarDto.Days.Add(dayDto);
            currentDate = currentDate.AddDays(1);
        }

        return calendarDto;
    }
}