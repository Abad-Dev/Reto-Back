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

    public IQueryable<Product> GetAll()
    {
        return _context.Products.AsQueryable();
    }

    public Product Create(Product product)
    {
        _context.Products.Add(product);
        _context.SaveChanges();

        return product;
    }
}

public interface IProductService 
{
    public IQueryable<Product> GetAll();
    public Product Create(Product product);
}