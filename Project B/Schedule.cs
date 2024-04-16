using System.Data.Entity;

public class Schedule
{
    public int ID { get; set; }
    public int Movie_ID { get; set; }
    public Film Film { get; set; }
    public int Hall_ID { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    
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
        public static int GetTotalTicketsPerMovie(int movieID)
    {
        using DataBaseConnection db = new();
        var totalTicket = db.Ticket.Where(t => t.Movie_ID == movieID).Count();
        return totalTicket;
    }
    public bool CountTickets (int schedule_id)
    {   
        DataBaseConnection db = new DataBaseConnection();
        var count = db.Ticket.Where(s=> s.Schedule_ID == schedule_id).Count();
        //var getschedule = db.Ticket.Where(s => s.Schedule_ID == schedule_id);
    

        var getHallID = db.Schedule.Where(s => s.ID == schedule_id).Select(s => s.Hall_ID).FirstOrDefault();

        if (getHallID == 1)
        {
            if (count >= 150)
            {
                return true;
            }
            else 
            {
                return false;
            }
        }
        else if (getHallID == 2)
        {
            if (count >= 300)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
        
            if (count >= 500)
            {
                return true;
            }
            else 
            {
                return false;
            }
        }
        
    }

    public List<(string, DateTime, DateTime)> DisplayAvailableMovies (int schedule_id)
    {
        DataBaseConnection db = new DataBaseConnection();
        // 
        var MatchingScheduleId = db.Schedule.FirstOrDefault(s => s.ID == schedule_id);
        var matchingMovieID = MatchingScheduleId.Movie_ID;

        if (MatchingScheduleId != null)
        {   
            var availableMoviesList = (from schedule in db.Schedule
                                    join movie in db.Movie on schedule.Movie_ID equals movie.ID
                                    where schedule.Movie_ID == matchingMovieID
                                    select new { FilmTitle = movie.Title, ScheduleStartDate = schedule.StartDate, ScheduleEndDate = schedule.EndDate })
                                    .ToList();

            // Convert the result to the desired format
            var formattedList = availableMoviesList.Select(item => (item.FilmTitle, item.ScheduleStartDate, item.ScheduleEndDate )).ToList();



            return formattedList;
        }
       
        return null;
    }
}

