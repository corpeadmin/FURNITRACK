using FURNITRACK.Models;

namespace FURNITRACK.ViewModels
{
    public class CustomersViewModel
    {
        public List<Customer> Customers { get; set; } = new List<Customer>();
        public int TotalCustomers { get; set; }
        public int NewCustomersThisMonth { get; set; }
        
        // For the detail view
        public Customer? SelectedCustomer { get; set; }
        public List<Sales> PurchaseHistory { get; set; } = new List<Sales>();
    }
}
