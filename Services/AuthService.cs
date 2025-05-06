using BCrypt.Net;
using GameBackEnd.Data;
using GameBackEnd.Models;
using GameBackEnd.Models.Entities;
using GameBackEnd.Models.API;
using Microsoft.EntityFrameworkCore;
using Azure.Core;
using GameBackEnd.Handlers;

namespace GameBackEnd.Services
{
    public class AuthService
    {
        private readonly GameDbContext _context;
        private readonly JwtService _jwtService;
        public AuthService(GameDbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }
        public async Task<AuthResponseModel?> RegisterAsync(RegisterRequestModel request)
        {
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
                throw new Exception("This Email has already been registered!");
            if (await _context.Users.AnyAsync(u => u.UserName == request.UserName))
                throw new Exception("Username has been taken!");
            try
            {
                User user = new User
                {
                    Email = request.Email,
                    UserName = request.UserName,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
                };
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                var result = await LoginAsync(new LoginRequestModel
                {
                    Email = request.Email,
                    Password = request.Password
                });
                return result;
            }
            catch
            {
                return null;
            }
        }
        public async Task<AuthResponseModel> LoginAsync(LoginRequestModel request)
        {
            User? user = await _context.Users.FirstOrDefaultAsync(x => x.Email == request.Email);
            if (user is null || !PasswordHashHandler.VerifyPassword(request.Password, user.PasswordHash))
                return null;
            var result = _jwtService.CreateToken(user.UserName, user.Email);
            return new AuthResponseModel
            {
                UserName = user.UserName,
                AccessToken = result.Item1,
                ExpiresIn = (int)result.Item2.Subtract(DateTime.UtcNow).TotalSeconds
            };
        }
    }
}
