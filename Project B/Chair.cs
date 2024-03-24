public class Chair
{
    public int Id;
    public string Type;
    public double Price;

    public bool Sold;

    public Chair(int id, string type, double price)
    {
        Id = id;
        Type = type;
        Price = price;
        Sold = false;
    }
}
