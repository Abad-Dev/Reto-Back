using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using back.Models;

namespace back.Context;

public class OrderContext : DbContext
{
    public DbSet<Product> Products { get;set; }
    public DbSet<Order> Orders { get;set; }
    public DbSet<Detail> Details { get;set; }
    private readonly IConfiguration _configuration;

    public OrderContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySQL(_configuration.GetConnectionString("Reto"));
    }

     protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Detail>()
            .HasOne(d => d.Order)
            .WithMany(o => o.Details)
            .HasForeignKey(d => d.OrderId);

        modelBuilder.Entity<Detail>()
            .HasOne(d => d.Product)
            .WithMany()
            .HasForeignKey(d => d.ProductId);

        base.OnModelCreating(modelBuilder);
    }
}