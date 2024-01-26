using back.Context;

namespace back.Services;

public class OrderService: IOrderService
{
    private readonly OrderContext _context;

    public OrderService(OrderContext dbContext)
    {
        _context = dbContext;
    }
    public void Get()
    {
        _context.Database.EnsureCreated();
    }
}

public interface IOrderService 
{
    public void Get();
}