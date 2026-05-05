using FURNITRACK.Data;
using FURNITRACK.Models;
using Microsoft.EntityFrameworkCore;

namespace FURNITRACK.Services
{
    public class SalesService : ISalesService
    {
        private readonly ApplicationDBContext _context;
        private readonly ILogger<SalesService> _logger;

        public SalesService(ApplicationDBContext context, ILogger<SalesService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Sales>> GetAllSalesAsync()
        {
            try
            {
                return await _context.Sales
                    .Include(s => s.CreatedBy)
                    .Include(s => s.SalesItems)
                        .ThenInclude(si => si.Product)
                    .OrderByDescending(s => s.SalesDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving sales: {ex.Message}");
                throw;
            }
        }

        public async Task<Sales?> GetSalesByIdAsync(int id)
        {
            try
            {
                return await _context.Sales
                    .Include(s => s.CreatedBy)
                    .Include(s => s.SalesItems)
                        .ThenInclude(si => si.Product)
                    .FirstOrDefaultAsync(s => s.SalesId == id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving sale {id}: {ex.Message}");
                throw;
            }
        }

        public async Task<List<Sales>> GetSalesByUserAsync(int userId)
        {
            try
            {
                return await _context.Sales
                    .Include(s => s.CreatedBy)
                    .Include(s => s.SalesItems)
                    .Where(s => s.UserId == userId)
                    .OrderByDescending(s => s.SalesDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving sales for user {userId}: {ex.Message}");
                throw;
            }
        }

        public async Task<List<Sales>> GetSalesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                return await _context.Sales
                    .Include(s => s.CreatedBy)
                    .Include(s => s.SalesItems)
                    .Where(s => s.SalesDate >= startDate && s.SalesDate <= endDate)
                    .OrderByDescending(s => s.SalesDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving sales for date range: {ex.Message}");
                throw;
            }
        }

        public async Task<Sales> CreateSaleAsync(Sales sale)
        {
            try
            {
                _context.Sales.Add(sale);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Sale created: {sale.OrderNumber} (ID: {sale.SalesId})");
                return sale;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating sale: {ex.Message}");
                throw;
            }
        }

        public async Task<Sales> UpdateSaleAsync(Sales sale)
        {
            try
            {
                sale.LastModifiedDate = DateTime.UtcNow;
                _context.Sales.Update(sale);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Sale updated: {sale.OrderNumber} (ID: {sale.SalesId})");
                return sale;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating sale {sale.SalesId}: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteSaleAsync(int id)
        {
            try
            {
                var sale = await _context.Sales.FindAsync(id);
                if (sale == null)
                    return false;

                sale.Status = "Cancelled";
                sale.LastModifiedDate = DateTime.UtcNow;
                _context.Sales.Update(sale);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Sale cancelled: {id}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting sale {id}: {ex.Message}");
                throw;
            }
        }

        public async Task<decimal> GetTotalSalesAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                var query = _context.Sales.AsQueryable();

                if (startDate.HasValue)
                    query = query.Where(s => s.SalesDate >= startDate);

                if (endDate.HasValue)
                    query = query.Where(s => s.SalesDate <= endDate);

                return await query.SumAsync(s => s.TotalAmount);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error calculating total sales: {ex.Message}");
                throw;
            }
        }
    }
}
