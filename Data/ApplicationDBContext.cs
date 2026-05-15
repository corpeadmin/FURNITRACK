using FURNITRACK.Models;
using Microsoft.EntityFrameworkCore;

namespace FURNITRACK.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
            : base(options)
        {
        }

        // DbSets for entities
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Sales> Sales { get; set; }
        public DbSet<SalesItem> SalesItems { get; set; }
        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Category configuration
            modelBuilder.Entity<Category>()
                .HasKey(c => c.CategoryId);
            modelBuilder.Entity<Category>()
                .Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            // Product configuration
            modelBuilder.Entity<Product>()
                .HasKey(p => p.ProductId);
            modelBuilder.Entity<Product>()
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);
            modelBuilder.Entity<Product>()
                .Property(p => p.SKU)
                .IsRequired()
                .HasMaxLength(50);
            modelBuilder.Entity<Product>()
                .Property(p => p.UnitPrice)
                .HasPrecision(18, 2);
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // User configuration
            modelBuilder.Entity<User>()
                .HasKey(u => u.UserId);
            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100);
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Customer configuration
            modelBuilder.Entity<Customer>()
                .HasKey(c => c.CustomerId);
            modelBuilder.Entity<Customer>()
                .Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            // Sales configuration
            modelBuilder.Entity<Sales>()
                .HasKey(s => s.SalesId);
            modelBuilder.Entity<Sales>()
                .Property(s => s.OrderNumber)
                .IsRequired()
                .HasMaxLength(50);
            modelBuilder.Entity<Sales>()
                .Property(s => s.SubTotal)
                .HasPrecision(18, 2);
            modelBuilder.Entity<Sales>()
                .Property(s => s.TaxAmount)
                .HasPrecision(18, 2);
            modelBuilder.Entity<Sales>()
                .Property(s => s.TotalAmount)
                .HasPrecision(18, 2);
            modelBuilder.Entity<Sales>()
                .HasOne(s => s.CreatedBy)
                .WithMany(u => u.CreatedSales)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Sales-Customer relationship
            modelBuilder.Entity<Sales>()
                .HasOne(s => s.Customer)
                .WithMany(c => c.Sales)
                .HasForeignKey(s => s.CustomerId)
                .OnDelete(DeleteBehavior.SetNull);

            // SalesItem configuration
            modelBuilder.Entity<SalesItem>()
                .HasKey(si => si.SalesItemId);
            modelBuilder.Entity<SalesItem>()
                .Property(si => si.UnitPrice)
                .HasPrecision(18, 2);
            modelBuilder.Entity<SalesItem>()
                .Property(si => si.LineTotal)
                .HasPrecision(18, 2);
            modelBuilder.Entity<SalesItem>()
                .Property(si => si.Discount)
                .HasPrecision(18, 2);
            modelBuilder.Entity<SalesItem>()
                .HasOne(si => si.Sales)
                .WithMany(s => s.SalesItems)
                .HasForeignKey(si => si.SalesId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<SalesItem>()
                .HasOne(si => si.Product)
                .WithMany(p => p.SalesItems)
                .HasForeignKey(si => si.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
