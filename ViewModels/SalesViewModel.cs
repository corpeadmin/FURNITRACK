using FURNITRACK.Models;

namespace FURNITRACK.ViewModels
{
    public class SalesViewModel
    {
        public List<Sales> Sales { get; set; } = new List<Sales>();
        public List<Product> Products { get; set; } = new List<Product>();
        public string? FilterStatus { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal AverageOrderValue { get; set; }
    }
}
