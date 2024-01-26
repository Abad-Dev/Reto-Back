using back.Context;
using back.Models;

namespace back.Services;

public class ProductService: IProductService
{
    private readonly OrderContext _context;

    public ProductService(OrderContext dbContext)
    {
        _context = dbContext;
    }
    public void Create(Product product)
    {
        _context.Products.Add(product);
        _context.SaveChanges();
    }
}

public interface IProductService 
{
    public void Create(Product product);
}