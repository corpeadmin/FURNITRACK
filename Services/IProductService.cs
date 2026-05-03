using FURNITRACK.Models;

namespace FURNITRACK.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(int id);
        Task<List<Product>> GetProductsByCategoryAsync(int categoryId);
        Task<Product> CreateProductAsync(Product product);
        Task<Product> UpdateProductAsync(Product product);
        Task<bool> DeleteProductAsync(int id);
        Task<List<Product>> GetLowStockProductsAsync();
    }
}
