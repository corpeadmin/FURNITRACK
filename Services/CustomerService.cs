using FURNITRACK.Data;
using FURNITRACK.Models;
using Microsoft.EntityFrameworkCore;

namespace FURNITRACK.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ApplicationDBContext _context;
        private readonly ILogger<CustomerService> _logger;

        public CustomerService(ApplicationDBContext context, ILogger<CustomerService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Customer>> GetAllCustomersAsync()
        {
            return await _context.Customers
                .Include(c => c.Sales)
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<Customer?> GetCustomerByIdAsync(int id)
        {
            return await _context.Customers
                .Include(c => c.Sales)
                .ThenInclude(s => s.SalesItems)
                .ThenInclude(si => si.Product)
                .FirstOrDefaultAsync(c => c.CustomerId == id);
        }

        public async Task<Customer?> GetCustomerByNameAsync(string name)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower());
        }

        public async Task<Customer> CreateCustomerAsync(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task<Customer> UpdateCustomerAsync(Customer customer)
        {
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task<List<Sales>> GetCustomerPurchaseHistoryAsync(int customerId)
        {
            return await _context.Sales
                .Include(s => s.SalesItems)
                .ThenInclude(si => si.Product)
                .Where(s => s.CustomerId == customerId)
                .OrderByDescending(s => s.SalesDate)
                .ToListAsync();
        }
    }
}
