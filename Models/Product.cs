namespace FURNITRACK.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        
        public string? SKU { get; set; } = string.Empty;
        
        public string Name { get; set; } = string.Empty;
        
        public string? Description { get; set; } = string.Empty;
        
        public decimal UnitPrice { get; set; }
        
        public int QuantityInStock { get; set; }
        
        public int MinimumStockLevel { get; set; }
        
        public int MaximumStockLevel { get; set; }
        
        public int CategoryId { get; set; }
        
        public Category? Category { get; set; }

        public string? ImageUrl { get; set; } = "/images/placeholder-furniture.png";
        
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        
        public DateTime LastModifiedDate { get; set; } = DateTime.UtcNow;
        
        public bool IsActive { get; set; } = true;
        
        // Navigation property for sales items
        public ICollection<SalesItem>? SalesItems { get; set; } = new List<SalesItem>();
    }
}
