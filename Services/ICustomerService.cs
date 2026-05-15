using FURNITRACK.Models;

namespace FURNITRACK.Services
{
    public interface ICustomerService
    {
        Task<List<Customer>> GetAllCustomersAsync();
        Task<Customer?> GetCustomerByIdAsync(int id);
        Task<Customer?> GetCustomerByNameAsync(string name);
        Task<Customer> CreateCustomerAsync(Customer customer);
        Task<Customer> UpdateCustomerAsync(Customer customer);
        Task<List<Sales>> GetCustomerPurchaseHistoryAsync(int customerId);
    }
}
