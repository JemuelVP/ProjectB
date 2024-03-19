public class CinemaHall
{
    public int ID;
    public int numberOfSeats;
    public List<Chair> Chairs;
    public CinemaHall(int id, int numberofseats)
    {
        ID = id;
        numberOfSeats = numberofseats;
        Chairs = new List<Chair>();
    }
    public virtual void FillChairs()
    {
        for (int i = 0; i < numberOfSeats; i++)
        {
            Chairs.Add(new Chair());
        }
    }
}
// this is the baseclass CinemaHall, it contains a list of chairs, numberOfSeats is the amount of seats
// it has three deriven classes.