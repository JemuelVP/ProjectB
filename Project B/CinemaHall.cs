public class CinemaHall
{
    public int ID { get; set; }

    public string Name { get; set; }
    public int NumberOfSeats { get; set; }
    public List<Chair> Chairs { get; set; }

    public virtual void FillChairs(int id, string type, double price)
    {
        for (int i = 0; i < this.NumberOfSeats; i++)
        {
            Chairs.Add(new Chair
            {
                Type = type,
                Price = price
            });
        }
    }
}
// this is the baseclass CinemaHall, AddChairs() it adds chairs of every type (standard, extraleg, loveseat)
// FillChairs() adds the chairs to a dictionary, int amountChairs is the maximum chairs of the specific type to be added
