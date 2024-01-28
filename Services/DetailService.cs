using back.Context;
using back.Models;

namespace back.Services;

public class DetailService: IDetailService
{
    private readonly OrderContext _context;

    public DetailService(OrderContext dbContext)
    {
        _context = dbContext;
    }


    public IQueryable<Detail> GetByOrderId(string orderId)
    {
        return _context.Details.Where(d => d.OrderId == orderId).AsQueryable();
    }
    public IQueryable<Detail> GetAll()
    {
        return _context.Details.AsQueryable();
    }

    public void Create(Detail detail)
    {
        _context.Details.Add(detail);
        _context.SaveChanges();
    }
}

public interface IDetailService 
{
    public IQueryable<Detail> GetByOrderId(string orderId);
    public IQueryable<Detail> GetAll();
    public void Create(Detail Detail);
}