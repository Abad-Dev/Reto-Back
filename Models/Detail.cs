namespace back.Models;

public class Detail: Base
{
    public virtual Order Order { get;set; }
    public string OrderId { get;set; }
    public virtual Product Product { get;set; }
    public string ProductId { get;set; }
    public int Qty { get;set; }
}