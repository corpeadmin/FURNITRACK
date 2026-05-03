namespace FURNITRACK.Models
{
    public class User
    {
        public int UserId { get; set; }
        
        public string FirstName { get; set; } = string.Empty;
        
        public string LastName { get; set; } = string.Empty;
        
        public string Email { get; set; } = string.Empty;
        
        public string Password { get; set; } = string.Empty; // Added for authentication
        
        public string? PhoneNumber { get; set; } = string.Empty;
        
        public string Role { get; set; } = "Staff"; // Admin, Manager, Staff
        
        public string? Department { get; set; } = string.Empty;
        
        public DateTime HireDate { get; set; } = DateTime.UtcNow;
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        
        public DateTime LastModifiedDate { get; set; } = DateTime.UtcNow;
        
        // Navigation property
        public ICollection<Sales>? CreatedSales { get; set; } = new List<Sales>();
    }
}
