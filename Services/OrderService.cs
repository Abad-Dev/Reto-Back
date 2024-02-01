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
        var orderFound = _context.Orders
            .Where(o => o.Id == id)
            .Include(o => o.Details)
            .ThenInclude(d => d.Product)
            .FirstOrDefault();
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
            Product detailProduct = _context.Products.Find(detail.ProductId);
            detailProduct.QtyInStock -= detail.Qty;

            detail.OrderId = newOrder.Id;
            detail.Product = detailProduct;
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

        if (orderFound.Status != OrderStatus.Completed)
        {
            foreach(Detail detail in orderFound.Details)
            {
                Product productFound = _context.Products.Find(detail.ProductId);
                detail.OrderId = orderId;
                detail.Order = orderFound;
                detail.Product = productFound;
                productFound.QtyInStock += detail.Qty;
            }
        }

        var detailsToRemove = _context.Details.Where(d => d.OrderId == orderId || d.OrderId == null);

        _context.Orders.Remove(orderFound);
        _context.Details.RemoveRange(detailsToRemove);
        _context.SaveChanges();
        return true;
    }

    public Order UpdateOrder(string orderId, Order order)
    {
        Order orderFound = GetOrderById(orderId);
        if (orderFound == null)
        {
            return null;
        }
        foreach (Detail detail in order.Details)
        {
            Detail prevDetail = _context.Details
                .Where(d => d.ProductId == detail.ProductId &&
                d.OrderId == orderId)
                .Include(d => d.Product)
                .FirstOrDefault();
            Product detailProduct = _context.Products.Find(detail.ProductId);
            if (prevDetail != null)
            {   
                detailProduct.QtyInStock = detailProduct.QtyInStock + prevDetail.Qty - detail.Qty;
            } else // New Product has been added
            {
                detailProduct.QtyInStock -= detail.Qty;
            }
            detail.OrderId = orderFound.Id;
            detail.Order = orderFound;
            detail.ProductId = detailProduct.Id;
            detail.Product = detailProduct;
        }

        foreach (Detail detail in orderFound.Details)
        {
            Detail currDetail = order.Details.Where(d => d.ProductId == detail.ProductId).FirstOrDefault();

            if (currDetail == null) // A product has been deleted
            {
                Product detailProduct = _context.Products.Find(detail.ProductId);
                detailProduct.QtyInStock += detail.Qty;
                _context.Details.Remove(detail);
            }
        }

        orderFound.Status = order.Status;
        orderFound.Details = order.Details;
        _context.SaveChanges();

        return orderFound;
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
    public Order UpdateOrder(string orderId, Order order);
    public bool AddDetail(string orderId, string productId, int qty);
    public bool IsValidDetail(Detail detail);
}
