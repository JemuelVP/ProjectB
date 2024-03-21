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


    //Checks which type of Chair is chosen and adjusts the Chair Price based on the Chair Name.
    public void PriceForChairType(Chair chair)
    {
        if (chair.Name.ToLower() == "loveseat")
        {
            chair.Price += 5;
        }
        else if (chair.Name.ToLower() == "extralegroom")
        {   
            // There is no price specified for the extralegroom chair so i just gave it a 2.5 for now, we can adjust it later.
            chair.Price += 2.5;
        }
    }
}
