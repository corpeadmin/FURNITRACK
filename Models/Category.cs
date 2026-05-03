namespace FURNITRACK.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        
        public string Name { get; set; } = string.Empty;
        
        public string Description { get; set; } = string.Empty;
        
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        
        // Navigation property
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
