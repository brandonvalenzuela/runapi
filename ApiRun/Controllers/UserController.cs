using Application.Dto;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiRun.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationDto userDto)
        {
            try
            {
                await _userService.RegisterUser(userDto);
                return Ok("User registered successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateUser([FromBody] UserAuthenticateDto userDto)
        {
            try
            {
                var token = await _userService.AuthenticateUser(userDto);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
