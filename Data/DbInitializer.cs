using FURNITRACK.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FURNITRACK.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDBContext context)
        {
            context.Database.EnsureCreated();

            // Look for any users.
            if (context.Users.Any())
            {
                return;   // DB has been seeded
            }

            var passwordHasher = new PasswordHasher<User>();

            var users = new User[]
            {
                new User { FirstName = "John", LastName = "Doe", Email = "john.doe@furnitrack.com", Role = "Admin", Department = "Management", HireDate = DateTime.Now.AddYears(-2) },
                new User { FirstName = "Jane", LastName = "Smith", Email = "jane.smith@furnitrack.com", Role = "Manager", Department = "Sales", HireDate = DateTime.Now.AddYears(-1) },
                new User { FirstName = "Mike", LastName = "Johnson", Email = "mike.j@furnitrack.com", Role = "Staff", Department = "Warehouse", HireDate = DateTime.Now.AddMonths(-6) }
            };

            foreach (var user in users)
            {
                user.Password = passwordHasher.HashPassword(user, "Password123");
            }

            context.Users.AddRange(users);
            context.SaveChanges();

            var categories = new Category[]
            {
                new Category { Name = "Office Furniture", Description = "Chairs, desks, and storage for professional environments" },
                new Category { Name = "Living Room", Description = "Sofas, coffee tables, and entertainment centers" },
                new Category { Name = "Dining Room", Description = "Dining tables, chairs, and buffets" },
                new Category { Name = "Bedroom", Description = "Bed frames, mattresses, and nightstands" }
            };

            context.Categories.AddRange(categories);
            context.SaveChanges();

            var products = new Product[]
            {
                new Product { Name = "Executive Office Chair", SKU = "OFF-CHR-001", Description = "High-back ergonomic leather chair", UnitPrice = 4250.00m, QuantityInStock = 15, MinimumStockLevel = 5, MaximumStockLevel = 30, CategoryId = categories[0].CategoryId },
                new Product { Name = "L-Shaped Desk", SKU = "OFF-DSK-002", Description = "Large wooden desk with built-in drawers", UnitPrice = 8500.00m, QuantityInStock = 8, MinimumStockLevel = 3, MaximumStockLevel = 15, CategoryId = categories[0].CategoryId },
                new Product { Name = "Leather Sectional Sofa", SKU = "LIV-SOF-001", Description = "Premium gray leather sofa", UnitPrice = 25000.00m, QuantityInStock = 4, MinimumStockLevel = 2, MaximumStockLevel = 10, CategoryId = categories[1].CategoryId },
                new Product { Name = "Oak Dining Table", SKU = "DIN-TBL-001", Description = "Solid oak table for 6 people", UnitPrice = 12000.00m, QuantityInStock = 6, MinimumStockLevel = 2, MaximumStockLevel = 12, CategoryId = categories[2].CategoryId },
                new Product { Name = "Memory Foam Mattress", SKU = "BED-MAT-001", Description = "Queen size comfort mattress", UnitPrice = 15000.00m, QuantityInStock = 12, MinimumStockLevel = 4, MaximumStockLevel = 20, CategoryId = categories[3].CategoryId },
                new Product { Name = "Folding Chair", SKU = "DIN-CHR-005", Description = "Simple metal folding chair", UnitPrice = 450.00m, QuantityInStock = 50, MinimumStockLevel = 10, MaximumStockLevel = 100, CategoryId = categories[2].CategoryId },
                new Product { Name = "Bookshelf", SKU = "OFF-STG-003", Description = "5-tier wooden bookshelf", UnitPrice = 3200.00m, QuantityInStock = 2, MinimumStockLevel = 5, MaximumStockLevel = 20, CategoryId = categories[0].CategoryId } // Low stock item
            };

            context.Products.AddRange(products);
            context.SaveChanges();

            var sales = new Sales[]
            {
                new Sales { 
                    OrderNumber = "ORD-2026-001", 
                    SalesDate = DateTime.Now.AddDays(-2), 
                    CustomerName = "Alice Walker", 
                    CustomerEmail = "alice@example.com", 
                    CustomerPhone = "0917-123-4567",
                    ShippingAddress = "123 Maple St, Quezon City",
                    SubTotal = 8500.00m,
                    TaxAmount = 1020.00m,
                    TotalAmount = 9520.00m,
                    Status = "Delivered",
                    UserId = users[1].UserId
                },
                new Sales { 
                    OrderNumber = "ORD-2026-002", 
                    SalesDate = DateTime.Now.AddHours(-3), 
                    CustomerName = "Bob Brown", 
                    CustomerEmail = "bob.b@example.com", 
                    CustomerPhone = "0918-987-6543",
                    ShippingAddress = "456 Oak Rd, Makati",
                    SubTotal = 4250.00m,
                    TaxAmount = 510.00m,
                    TotalAmount = 4760.00m,
                    Status = "Pending",
                    UserId = users[1].UserId
                }
            };

            context.Sales.AddRange(sales);
            context.SaveChanges();

            var salesItems = new SalesItem[]
            {
                new SalesItem { SalesId = sales[0].SalesId, ProductId = products[1].ProductId, Quantity = 1, UnitPrice = 8500.00m, LineTotal = 8500.00m },
                new SalesItem { SalesId = sales[1].SalesId, ProductId = products[0].ProductId, Quantity = 1, UnitPrice = 4250.00m, LineTotal = 4250.00m }
            };

            context.SalesItems.AddRange(salesItems);
            context.SaveChanges();
        }
    }
}
