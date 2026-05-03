using FURNITRACK.Data;
using FURNITRACK.Models;
using Microsoft.EntityFrameworkCore;

namespace FURNITRACK.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDBContext _context;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(ApplicationDBContext context, ILogger<CategoryService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            try
            {
                return await _context.Categories
                    .Include(c => c.Products)
                    .OrderBy(c => c.Name)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving categories: {ex.Message}");
                throw;
            }
        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            try
            {
                return await _context.Categories
                    .Include(c => c.Products)
                    .FirstOrDefaultAsync(c => c.CategoryId == id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving category {id}: {ex.Message}");
                throw;
            }
        }

        public async Task<Category> CreateCategoryAsync(Category category)
        {
            try
            {
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Category created: {category.Name} (ID: {category.CategoryId})");
                return category;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating category: {ex.Message}");
                throw;
            }
        }

        public async Task<Category> UpdateCategoryAsync(Category category)
        {
            try
            {
                _context.Categories.Update(category);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Category updated: {category.Name} (ID: {category.CategoryId})");
                return category;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating category {category.CategoryId}: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            try
            {
                var category = await _context.Categories.FindAsync(id);
                if (category == null)
                    return false;

                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Category deleted: {id}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting category {id}: {ex.Message}");
                throw;
            }
        }
    }
}
