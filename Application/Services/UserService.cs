using Application.Dto;
using Application.Interfaces;
using Domain.Entities;
using Domain.Repositories;
using Domain.Services;
using Microsoft.Extensions.Configuration;
using System.Security.Authentication;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserDomainService _userDomainService;
        private readonly string? _jwtSecret;

        public UserService(IUserRepository userRepository, UserDomainService userDomainService, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _userDomainService = userDomainService;
            _jwtSecret = configuration["Jwt:Secret"];
        }

        public async Task RegisterUser(UserRegistrationDto userDto)
        {
            if (await _userRepository.ExistsByEmailAsync(userDto.Email))
            {
                throw new InvalidOperationException("Email is already in use.");
            }

            if (!_userDomainService.ValidatePassword(userDto.Password))
            {
                throw new InvalidOperationException("Password does not meet complexity requirements.");
            }

            var user = new User(userDto.FirstName, userDto.LastName, userDto.Email, HashPassword(userDto.Password));

            await _userRepository.AddAsync(user);
        }

        public async Task<string> AuthenticateUser(UserAuthenticateDto userDto)
        {
            if(!await _userRepository.ExistsByEmailAsync(userDto.Email))
            {
                throw new InvalidOperationException("User not found.");
            }

            if (!await _userRepository.VerifyPassword(HashPassword(userDto.Password)))
            {
                throw new AuthenticationException("Invalid credentials.");
            }

            //Validaciones adicionales como: verificacion de subscripcion activa

            var user = await _userRepository.GetByEmailAsync(userDto.Email);

            return _userDomainService.GenerateJwtToken(user, _jwtSecret);
        }

        private string HashPassword(string password) =>
            Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
    }
}
