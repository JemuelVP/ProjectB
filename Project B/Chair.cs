public class Chair
{
    public int Id;
    public string ChairType;
    public double Price;

    public bool Sold;

    public Chair(int id, string chairType, double price)
    {
        Id = id;
        ChairType = chairType;
        Price = price;
        Sold = false;
        if (ChairType == "StandardSeat")
        {
            Price = 25.0;
        }
        else if (ChairType == "ExtraLeg")
        {
            Price = 30.0;
        }
        else if(ChairType == "LoveSeat")
        {
            Price = 35.0;
        }
    }
}
// added an if statement to determine what the price is for each seattype
