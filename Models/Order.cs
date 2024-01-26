namespace back.Models;

public class Order : Base
{
    public string OrderNum { get;set; }
    public DateTime Date { get;set; }
    public List<Product> Products { get;set; }
}