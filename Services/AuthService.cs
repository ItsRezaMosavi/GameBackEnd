using BCrypt.Net;
using GameBackEnd.Data;
using GameBackEnd.Models;
using Microsoft.EntityFrameworkCore;

namespace GameBackEnd.Services
{
    public class AuthService
    {
        private readonly GameDbContext _context;
        public AuthService(GameDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult> RegisterAsync(RegisterRequest model)
        {
            if (await _context.Users.AnyAsync(u => u.Email == model.Email))
                throw new Exception("This Email has already been registered!");
            if (await _context.Users.AnyAsync(u => u.UserName == model.UserName))
               throw new Exception("Username has been taken!");

            try
            {
                User user = new User
                {
                    Email = model.Email,
                    UserName = model.UserName,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password)
                };
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                return ServiceResult.Success("Registration Sucessful", model.UserName);
            }
            catch
            {
                return ServiceResult.Failure("Server Error");
            }
        }
        public async Task<ServiceResult> LoginAsync(LoginRequest model)
        {

        }
        public async Task<ServiceResult> LogOutAsync()
        {

        }
    }
}
