using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiRun.Controllers
{
    [ApiController]
    [Route("api/calendar")]
    public class CalendarController : Controller
    {
        private readonly ICalendarService _calendarService;

        public CalendarController(ICalendarService calendarService)
        {
            _calendarService = calendarService;
        }
        
        [HttpGet("get-trainings-of-the-month")]
        public async Task<IActionResult> GetCalendar(int userId, int year, int month)
        {
            try
            {
                var calendar = await _calendarService.GetUserCalendarAsync(userId, year, month);
                return Ok(new {calendar});
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
