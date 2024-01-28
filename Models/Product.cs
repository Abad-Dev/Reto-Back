namespace back.Models;

public class Product : Base
{
    public string Image { get;set; }
    public string Name { get;set; }
    public double Price { get;set; }
    public int QtyInStock { get;set; }
}