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
// this is the baseclass CinemaHall, it contains a list of chairs, numberOfSeats is the amount of seats
// it has three deriven classes.
