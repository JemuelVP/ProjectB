public class Schedule
{
    public int ID;
    public int SoldSeats;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    
    public Schedule(DateTime stratDate, DateTime endDate)
    {
        StartDate = stratDate;
        EndDate = endDate;  
    }


    public void CalculateEndDate(Film film)
    {
        EndDate = StartDate.AddMinutes(film.DurationInMin);
    }

}