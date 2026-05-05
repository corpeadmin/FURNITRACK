using FURNITRACK.Models;

namespace FURNITRACK.Services
{
    public interface ISalesService
    {
        Task<List<Sales>> GetAllSalesAsync();
        Task<Sales?> GetSalesByIdAsync(int id);
        Task<List<Sales>> GetSalesByUserAsync(int userId);
        Task<List<Sales>> GetSalesByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<Sales> CreateSaleAsync(Sales sale);
        Task<Sales> UpdateSaleAsync(Sales sale);
        Task<bool> DeleteSaleAsync(int id);
        Task<decimal> GetTotalSalesAsync(DateTime? startDate = null, DateTime? endDate = null);
    }
}
