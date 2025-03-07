using Application.Dto;

namespace Application.Interfaces;

public interface ICalendarService
{
    Task<CalendarDto> GetUserCalendarAsync(int userId, int year, int month);
}