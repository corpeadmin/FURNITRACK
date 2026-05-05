namespace FURNITRACK.ViewModels
{
    public class ReportsViewModel
    {
        public DateTime StartDate { get; set; } = DateTime.Now.AddMonths(-1);
        public DateTime EndDate { get; set; } = DateTime.Now;
        
        // Sales metrics
        public decimal TotalSales { get; set; }
        public int TotalOrders { get; set; }
        public decimal AverageOrderValue { get; set; }
        public decimal GrowthPercentage { get; set; }
        
        // Inventory metrics
        public int TotalProducts { get; set; }
        public int LowStockItems { get; set; }
        public decimal TotalInventoryValue { get; set; }
        
        // Category breakdown
        public Dictionary<string, int> SalesByCategory { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, decimal> RevenueByCategory { get; set; } = new Dictionary<string, decimal>();
        
        // Top performers
        public List<TopProductDTO> TopProducts { get; set; } = new List<TopProductDTO>();
        public List<TopCategoryDTO> TopCategories { get; set; } = new List<TopCategoryDTO>();
    }

    public class TopProductDTO
    {
        public string ProductName { get; set; } = string.Empty;
        public int QuantitySold { get; set; }
        public decimal TotalRevenue { get; set; }
    }

    public class TopCategoryDTO
    {
        public string CategoryName { get; set; } = string.Empty;
        public int ProductsSold { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
