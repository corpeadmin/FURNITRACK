namespace FURNITRACK.Models
{
    public class SalesItem
    {
        public int SalesItemId { get; set; }
        
        public int SalesId { get; set; }
        
        public Sales? Sales { get; set; }
        
        public int ProductId { get; set; }
        
        public Product? Product { get; set; }
        
        public int Quantity { get; set; }
        
        public decimal UnitPrice { get; set; }
        
        public decimal Discount { get; set; } = 0;
        
        public decimal LineTotal { get; set; }
        
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
