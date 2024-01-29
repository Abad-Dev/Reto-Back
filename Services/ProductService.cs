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

    public Product GetById(string productId)
    {
        return _context.Products.Find(productId);
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

    public bool Delete(string productId)
    {
        Product productFound = GetById(productId);
        if (productFound == null)
        {
            return false;
        }

        var detailsToDelete = _context.Details.Where(d => d.ProductId == productId);
        _context.Details.RemoveRange(detailsToDelete);

        _context.Products.Remove(productFound);
        _context.SaveChanges();
        return true;
    }
}

public interface IProductService 
{
    public IQueryable<Product> GetAll();
    public Product Create(Product product);
    public bool Delete(string productId);
}