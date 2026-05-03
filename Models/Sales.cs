namespace FURNITRACK.Models
{
    public class Sales
    {
        public int SalesId { get; set; }
        
        public string OrderNumber { get; set; } = string.Empty;
        
        public DateTime SalesDate { get; set; } = DateTime.UtcNow;
        
        public string CustomerName { get; set; } = string.Empty;
        
        public string CustomerEmail { get; set; } = string.Empty;
        
        public string CustomerPhone { get; set; } = string.Empty;
        
        public string ShippingAddress { get; set; } = string.Empty;
        
        public decimal SubTotal { get; set; }
        
        public decimal TaxAmount { get; set; }
        
        public decimal TotalAmount { get; set; }
        
        public string Status { get; set; } = "Pending"; // Pending, Shipped, Delivered, Cancelled
        
        public int UserId { get; set; }
        
        public User? CreatedBy { get; set; }
        
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        
        public DateTime LastModifiedDate { get; set; } = DateTime.UtcNow;
        
        // Navigation property
        public ICollection<SalesItem> SalesItems { get; set; } = new List<SalesItem>();
    }
}
