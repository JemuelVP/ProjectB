public class Chair
{
    public int Id;
    public string Name;
    public double Price;
    public bool Sold;

    public Chair(int id, string name, double price)
    {
        Id = id;
        Name = name;
        Price = price;
        Sold = false;
    }
}

