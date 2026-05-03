using FURNITRACK.Models;

namespace FURNITRACK.ViewModels
{
    public class InventoryViewModel
    {
        public List<Product> Products { get; set; } = new List<Product>();
        public List<Category> Categories { get; set; } = new List<Category>();
        public string? SelectedCategory { get; set; }
        public string? SearchTerm { get; set; }
        public int TotalProducts { get; set; }
        public int LowStockCount { get; set; }
        public decimal TotalInventoryValue { get; set; }
    }
}
