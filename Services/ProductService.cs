using FURNITRACK.Data;
using FURNITRACK.Models;
using Microsoft.EntityFrameworkCore;

namespace FURNITRACK.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDBContext _context;
        private readonly ILogger<ProductService> _logger;

        public ProductService(ApplicationDBContext context, ILogger<ProductService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            try
            {
                return await _context.Products
                    .Include(p => p.Category)
                    .Where(p => p.IsActive)
                    .OrderBy(p => p.Name)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving products: {ex.Message}");
                throw;
            }
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            try
            {
                return await _context.Products
                    .Include(p => p.Category)
                    .FirstOrDefaultAsync(p => p.ProductId == id && p.IsActive);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving product {id}: {ex.Message}");
                throw;
            }
        }

        public async Task<List<Product>> GetProductsByCategoryAsync(int categoryId)
        {
            try
            {
                return await _context.Products
                    .Include(p => p.Category)
                    .Where(p => p.CategoryId == categoryId && p.IsActive)
                    .OrderBy(p => p.Name)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving products for category {categoryId}: {ex.Message}");
                throw;
            }
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            try
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Product created: {product.Name} (ID: {product.ProductId})");
                return product;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating product: {ex.Message}");
                throw;
            }
        }

        public async Task<Product> UpdateProductAsync(Product product)
        {
            try
            {
                product.LastModifiedDate = DateTime.UtcNow;
                _context.Products.Update(product);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Product updated: {product.Name} (ID: {product.ProductId})");
                return product;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating product {product.ProductId}: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                    return false;

                product.IsActive = false;
                product.LastModifiedDate = DateTime.UtcNow;
                _context.Products.Update(product);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Product deleted (soft delete): {id}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting product {id}: {ex.Message}");
                throw;
            }
        }

        public async Task<List<Product>> GetLowStockProductsAsync()
        {
            try
            {
                return await _context.Products
                    .Include(p => p.Category)
                    .Where(p => p.QuantityInStock <= p.MinimumStockLevel && p.IsActive)
                    .OrderBy(p => p.QuantityInStock)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving low stock products: {ex.Message}");
                throw;
            }
        }
    }
}
