using System.Text.Json;
using Application.Dto;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ApiRun.Controllers
{
    [ApiController]
    [Route("api/dashboard")]
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }
        
        [HttpGet("obtener-plan")]
        public async Task<IActionResult> GetDashboard(int userId)
        {
            try
            {
                var dashboard = await _dashboardService.GetUserDashboardAsync(userId);
                return Ok(new {dashboard});
            }
            catch (Exception ex)
            {
                // Manejar errores (not found, etc.)
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
