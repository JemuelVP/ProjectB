using System.Data.Entity;

public class Schedule
{
    public int ID { get; set; }
    public int Movie_ID { get; set; }
    public Film Film { get; set; }
    public int Hall_ID { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool SoldOut { get; set; } = false;
    public DateTime CalculateEndDate(Film film)
    {
        return StartDate.AddMinutes(film.DurationInMin);
    }
    
    public void CreateFromFilm(Film movie, int hallID, DateTime startDate)
    {
        DataBaseConnection db = new DataBaseConnection();
        // Check if a schedule already exists for the given hallID, and startDate
        var existingSchedule = db.Schedule.FirstOrDefault(s => s.Hall_ID == hallID && s.StartDate == startDate);
        
        if (existingSchedule != null)
        {
            Console.WriteLine("Er bestaat al een schema voor deze zaal en startdatum.");
            return;
        }
        StartDate = startDate;
        EndDate = CalculateEndDate(movie);
        Hall_ID = hallID;
        Movie_ID = movie.ID;
        var entry = db.Schedule.Add(this);
        db.SaveChanges();
    }
}