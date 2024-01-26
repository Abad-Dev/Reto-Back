using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using back.Models;

namespace back.Context;

public class OrderContext : DbContext
{
    public DbSet<Product> Products { get;set; }
    public DbSet<Order> Orders { get;set; }
    private readonly IConfiguration _configuration;

    public OrderContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySQL(_configuration.GetConnectionString("Reto"));
    }

}