using FURNITRACK.Models;

namespace FURNITRACK.Services
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetUserByEmailAsync(string email);
        Task<List<User>> GetUsersByRoleAsync(string role);
        Task<User> CreateUserAsync(User user);
        Task<User> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(int id);
        Task<bool> UserEmailExistsAsync(string email);
        Task<User?> ValidateUserAsync(string email, string password);
    }
}
