using Microsoft.EntityFrameworkCore;

namespace Webshop.Models;

internal class WebshopDbContext : DbContext
{
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<PaymentMethod> PaymentMethods { get; set; }
    public DbSet<ShippingMethod> ShippingMethods { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(Helpers.ConfigHelper.GetConnectionString());
        //optionsBuilder.UseSqlServer("Server=tcp:fornkatt.database.windows.net,1433;Initial Catalog=JohansDB;Persist Security Info=False;User ID=unplanned;Password={your_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("webshop");

        modelBuilder.Entity<Order>()
            .HasMany(o => o.OrderItems)
            .WithOne(oi => oi.Order)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Product)
            .WithMany(p => p.OrderItems)
            .HasForeignKey(oi => oi.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Product>()
            .HasOne(c => c.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.Customer)
            .WithMany(c => c.Orders)
            .HasForeignKey(o => o.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Product>()
            .HasOne(p => p.Supplier)
            .WithMany(s => s.Products)
            .HasForeignKey(p => p.SupplierId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.ShippingMethod)
            .WithMany(sm => sm.Orders)
            .HasForeignKey(o => o.ShippingMethodId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.PaymentMethod)
            .WithMany(pm => pm.Orders)
            .HasForeignKey(o => o.PaymentMethodId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ShippingMethod>().HasData(
        new ShippingMethod { Id = 1, Name = "Standard", Description = "3-5 arbetsdagar", Price = 49m, EstimatedDaysMin = 3, EstimatedDaysMax = 5, IsActive = true },
        new ShippingMethod { Id = 2, Name = "Express", Description = "1-2 arbetsdagar", Price = 99m, EstimatedDaysMin = 1, EstimatedDaysMax = 2, IsActive = true },
        new ShippingMethod { Id = 3, Name = "Hämta i butik", Description = "Hämta direkt", Price = 0m, EstimatedDaysMin = 0, EstimatedDaysMax = 0, IsActive = true }
        );

        modelBuilder.Entity<PaymentMethod>().HasData(
        new PaymentMethod { Id = 1, Name = "Kort", Provider = "Klarna", IsActive = true, TransactionFee = 0m },
        new PaymentMethod { Id = 2, Name = "Swish", Provider = "Swish", IsActive = true, TransactionFee = 0m },
        new PaymentMethod { Id = 3, Name = "Faktura", Provider = "Klarna", IsActive = true, TransactionFee = 29m }
        );
    }
}