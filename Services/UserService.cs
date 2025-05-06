using GameBackEnd.Data;
using GameBackEnd.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameBackEnd.Services
{
    public class UserService
    {
        private readonly GameDbContext _context;
        public UserService(GameDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByUserNameAsync(string userName)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
        }
        public async Task<User?> GetUserByIdAsync(int Id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == Id);
        }
        public async Task<User?> GetUserByEmailAsync(string Email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == Email);
        }

    }
}
