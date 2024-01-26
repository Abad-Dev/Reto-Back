using back.Context;
using back.Models;

namespace back.Services;

public class OrderService: IOrderService
{
    private readonly OrderContext _context;

    public OrderService(OrderContext dbContext)
    {
        _context = dbContext;
    }
    public void CreateDb()
    {
        _context.Database.EnsureCreated();
    }

    public void CreateEmptyOrder()
    {
        _context.Orders.Add(new Order());
        _context.SaveChanges();
    }

    public void AddProductToOrder(string orderId, string productId)
    {
        Order orderFound = _context.Orders.Where(o => o.Id == orderId).FirstOrDefault();
        Product productFound = _context.Products.Where(p => p.Id == productId).FirstOrDefault();

        if (orderFound != null && productFound != null)
        {
            orderFound.Products ??= new List<Product>(); // Se inicializa vacía si no existe
            orderFound.Products.Add(productFound);

            _context.SaveChanges();
        }
    }
}

public interface IOrderService 
{
    public void CreateDb();
    public void CreateEmptyOrder();
    public void AddProductToOrder(string orderId, string productId);
}