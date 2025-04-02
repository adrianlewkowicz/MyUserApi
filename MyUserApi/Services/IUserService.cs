using MyUserApi.Models;

namespace MyUserApi.Services
{
    public interface IUserService
    {
        Task<User?> GetUserAsync(int id);
        Task<IEnumerable<User>> GetUsersAsync(int pageNumber, int pageSize);
        Task<User> CreateUserAsync(User user);
        Task<User> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(int id);
    }
}
