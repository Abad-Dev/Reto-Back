//using System.Data.Entity;
using back.Context;
using back.Models;
using Microsoft.EntityFrameworkCore;

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

    public Order GetOrderById(string id)
    {
        var orderFound = _context.Orders.Where(o => o.Id == id).Include(o => o.Details).FirstOrDefault();
        return orderFound;
    }

    public IQueryable<Order> GetAll()
    {
        return _context.Orders.Include(o => o.Details).ThenInclude(d => d.Product).AsQueryable();
    }

    public Order CreateEmptyOrder()
    {
        Order newOrder = new();
        _context.Orders.Add(newOrder);
        _context.SaveChanges();

        return newOrder;
    }

    public Order CreateOrder(Order order) {
        Order newOrder = new() {
            OrderNum = order.OrderNum,
            Date = order.Date,
            Details = order.Details
        };

        foreach (Detail detail in newOrder.Details) 
        {
            detail.OrderId = newOrder.Id;
        }

        _context.Orders.Add(newOrder);
        _context.SaveChanges();

        return new Order() {
            OrderNum = order.OrderNum,
            Date = order.Date,
            Details = null
        };
    }

    public bool DeleteOrder(string orderId) 
    {
        Order orderFound = GetOrderById(orderId);

        if (orderFound == null) {
            return false;
        }

        _context.Orders.Remove(orderFound);
        _context.SaveChanges();
        return true;
    }

    public bool AddDetail(string orderId, string productId, int qty)
    {
        Order orderFound = GetOrderById(orderId);
        Product productFound = _context.Products.Find(productId);

        Detail newDetail = new() {
            OrderId = orderId,
            Order = orderFound,
            ProductId = productId,
            Product = productFound,
            Qty = qty
        };
        if (IsValidDetail(newDetail))
        {
            if (orderFound.Details.Any(d => d.ProductId == productId)) // si ya se ha pedido el producto
            {
                Detail oldDetail = orderFound.Details.Where(d => d.ProductId == productId).FirstOrDefault();

                oldDetail.Qty += qty;
            }
            else
            {
                orderFound.Details.Add(newDetail);
                newDetail.Order = orderFound;
            }
          
            productFound.QtyInStock -= newDetail.Qty;
            
            _context.SaveChanges();

            return true;
        }

        return false;
    }

    public bool IsValidDetail(Detail detail)
    {
        Product productFound = _context.Products.Where(p => p.Id == detail.ProductId).FirstOrDefault();
        return detail.Qty <= productFound.QtyInStock && detail.Product != null && detail.Order != null;
    }
}

public interface IOrderService 
{
    public void CreateDb();
    public Order GetOrderById(string id);
    public IQueryable<Order> GetAll();
    public Order CreateEmptyOrder();
    public Order CreateOrder(Order order);
    public bool DeleteOrder(string orderId);
    public bool AddDetail(string orderId, string productId, int qty);
    public bool IsValidDetail(Detail detail);
}