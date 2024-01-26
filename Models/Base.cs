namespace back.Models;

public class Base
{
    public string Id { get; private set; }

    public Base()
    {
        Id = Guid.NewGuid().ToString();
    }
}