using Microsoft.EntityFrameworkCore;
using MyUserApi.Data;
using MyUserApi.Models;

namespace MyUserApi.Services
{
    public class UserService : IUserService
    {
        private readonly MyDbContext _context;

        public UserService(MyDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<IEnumerable<User>> GetUsersAsync(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0) pageNumber = 1;
            if (pageSize <= 0) pageSize = 10;

            return await _context.Users
                .OrderBy(u => u.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<User> CreateUserAsync(User user)
        {
            
            if (!user.Email.Contains("@"))
              throw new ArgumentException("Invalid email format");
            
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            var existing = await _context.Users.FindAsync(user.Id);
            if (existing == null)
            {
                throw new KeyNotFoundException($"User with id {user.Id} not found.");
            }

            // Zaktualizuj pola
            existing.FirstName = user.FirstName;
            existing.LastName = user.LastName;
            existing.Email = user.Email;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var existing = await _context.Users.FindAsync(id);
            if (existing == null)
            {
                return false;
            }

            _context.Users.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
