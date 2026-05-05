using FURNITRACK.Models;

namespace FURNITRACK.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalProducts { get; set; }
        public int LowStockProducts { get; set; }
        public int TotalUsers { get; set; }
        public decimal TotalSalesThisMonth { get; set; }
        public int SalesThisMonth { get; set; }
        public decimal AverageSaleValue { get; set; }
        
        public List<Product> TopProducts { get; set; } = new List<Product>();
        public List<Sales> RecentSales { get; set; } = new List<Sales>();
        public List<Product> LowStockItems { get; set; } = new List<Product>();
    }
}
