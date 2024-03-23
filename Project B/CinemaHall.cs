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
    public void FillChairs(Dictionary<string, int> chairArrangement)
    {
        foreach (KeyValuePair<string, int> chair in chairArrangement)
        {
            AddChairs(chair.Key, chair.Value);
        }
    }
    public void AddChairs(string chairType, int amountChairs)
    {
        for (int i = 0; i < amountChairs; i++)
        {
            if (Chairs.Count < numberOfSeats)
            {
                Chairs.Add(new Chair(Chairs.Count, chairType, 0.0)); // chairs.count keeps track of current chair id & adds respective price
            }
        }
    }
}
// this is the baseclass CinemaHall, AddChairs() it adds chairs of every type (standard, extraleg, loveseat)
// FillChairs() adds the chairs to a dictionary, int amountChairs is the maximum chairs of the specific type to be added
