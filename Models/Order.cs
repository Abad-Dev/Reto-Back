namespace back.Models;

public class Order : Base
{
    public string OrderNum { get;set; }
    public DateTime Date { get;set; }
    public List<Detail> Details { get;set; }

    public Order()
    {
        Date = DateTime.Now;
        OrderNum = Guid.NewGuid().ToString();
        Details = new List<Detail> () {};
    }
}