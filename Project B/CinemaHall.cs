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
}