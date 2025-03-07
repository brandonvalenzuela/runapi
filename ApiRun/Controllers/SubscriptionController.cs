using Application.Dto;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiRun.Controllers
{
    [ApiController]
    [Route("api/subscription")]
    public class SubscriptionController : Controller
    {
        private readonly ISubscriptionService _subscriptionService;
        private readonly IUserContextService _userContextService; // asumiendo que tu auth da un userId
    
        public SubscriptionController(ISubscriptionService subscriptionService, IUserContextService userContextService)
        {
            _subscriptionService = subscriptionService;
            _userContextService = userContextService;
        }
    
        // POST /api/subscription - crea nueva suscripción
        [AllowAnonymous] 
        [HttpPost]
        public async Task<IActionResult> CreateSubscription([FromBody] CreateSubscriptionRequest request)
        {
            // asumiendo un DTO con { SubscriptionType Type, decimal Price, int DurationDays }
            var userId = _userContextService.GetUserId(); // obtener userId
            
            var subscription = await _subscriptionService.CreateSubscriptionAsync(
                userId,
                request.Type,
                request.Period
            );
            
            return Ok(subscription);
        }
    
        // POST /api/subscription/renew
        [HttpPost("renew")]
        public async Task<IActionResult> RenewSubscription(int userId, [FromBody] RenewSubscriptionRequest request)
        {
            // asumiendo { SubscriptionType NewType, decimal NewPrice, int DurationDays }
            var subscription = await _subscriptionService.RenewSubscriptionAsync(
                userId,
                request.NewType,
                request.Period
            );
            
            return Ok(subscription);
        }
    
        // DELETE /api/subscription/cancel
        [HttpDelete("cancel")]
        public async Task<IActionResult> CancelSubscription(int subscriptionId)
        {
            await _subscriptionService.CancelSubscriptionAsync(subscriptionId);
            return NoContent();
        }
    
        // GET /api/subscription/active
        [AllowAnonymous] 
        [HttpGet("active")]
        public async Task<IActionResult> GetActive()
        {
            var userId = _userContextService.GetUserId();
            var sub = await _subscriptionService.GetActiveSubscriptionAsync(userId);
            if (sub == null) return NotFound("No hay suscripción activa.");
            return Ok(sub);
        }
    }
}
