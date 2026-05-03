using FURNITRACK.Data;
using FURNITRACK.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FURNITRACK.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDBContext _context;
        private readonly ILogger<UserService> _logger;
        private readonly PasswordHasher<User> _passwordHasher;

        public UserService(ApplicationDBContext context, ILogger<UserService> logger)
        {
            _context = context;
            _logger = logger;
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            try
            {
                return await _context.Users
                    .Where(u => u.IsActive)
                    .OrderBy(u => u.LastName)
                    .ThenBy(u => u.FirstName)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving users: {ex.Message}");
                throw;
            }
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            try
            {
                return await _context.Users
                    .FirstOrDefaultAsync(u => u.UserId == id && u.IsActive);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving user {id}: {ex.Message}");
                throw;
            }
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            try
            {
                return await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == email && u.IsActive);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving user by email: {ex.Message}");
                throw;
            }
        }

        public async Task<List<User>> GetUsersByRoleAsync(string role)
        {
            try
            {
                return await _context.Users
                    .Where(u => u.Role == role && u.IsActive)
                    .OrderBy(u => u.LastName)
                    .ThenBy(u => u.FirstName)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving users by role: {ex.Message}");
                throw;
            }
        }

        public async Task<User> CreateUserAsync(User user)
        {
            try
            {
                user.IsActive = true; // Ensure new users are active by default
                user.CreatedDate = DateTime.UtcNow;
                user.LastModifiedDate = DateTime.UtcNow;

                // Hash the password before saving
                if (!string.IsNullOrEmpty(user.Password))
                {
                    user.Password = _passwordHasher.HashPassword(user, user.Password);
                }
                
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"User created: {user.Email} (ID: {user.UserId})");
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating user: {ex.Message}");
                throw;
            }
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            try
            {
                user.LastModifiedDate = DateTime.UtcNow;

                // Only hash the password if it's being updated
                // We check if it's already a hash (simple check for Identity V3 hash format)
                if (!string.IsNullOrEmpty(user.Password) && !user.Password.StartsWith("AQAAAA")) 
                {
                    user.Password = _passwordHasher.HashPassword(user, user.Password);
                }

                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"User updated: {user.Email} (ID: {user.UserId})");
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating user {user.UserId}: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                    return false;

                user.IsActive = false;
                user.LastModifiedDate = DateTime.UtcNow;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"User deleted (soft delete): {id}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting user {id}: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UserEmailExistsAsync(string email)
        {
            try
            {
                return await _context.Users
                    .AnyAsync(u => u.Email == email && u.IsActive);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error checking if email exists: {ex.Message}");
                throw;
            }
        }

        public async Task<User?> ValidateUserAsync(string email, string password)
        {
            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == email && u.IsActive);

                if (user != null && !string.IsNullOrEmpty(user.Password))
                {
                    var result = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
                    if (result == PasswordVerificationResult.Success || result == PasswordVerificationResult.SuccessRehashNeeded)
                    {
                        return user;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error validating user: {ex.Message}");
                throw;
            }
        }
    }
}
