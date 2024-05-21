using System.Data.Entity;
public class ScheduleController
{
   public static List<Schedule> GetTitlesForScheduledMovies(DateTime startDate, DateTime endDate)
    {
        using (DataBaseConnection db = new DataBaseConnection())
        {   
            //Gets the titles of the scheduled movies and groups them so that the title is only one time in the list
            var Titles = db.Schedule
                .Include(s => s.Film)
                .Where(s => s.StartDate >= startDate && s.EndDate < endDate)
                .GroupBy(s => s.Film)
                .Select(s => s.First()) 
                .ToList();

            foreach (var s in Titles)
            {
                db.Entry(s).Reference(s => s.Film).Load();
            }

            return Titles;
        }
    }

    public static List<Schedule> GetAllSchedules(DateTime startDate, DateTime endDate)
    { 
        using (DataBaseConnection db = new DataBaseConnection())
        {   //Gets all the schedules with the given date range
            var schedules = db.Schedule
                .Include(s => s.Film)
                .Where(s => s.StartDate >= startDate && s.EndDate < endDate)
                .ToList();

            foreach (var s in schedules)
            {
                db.Entry(s).Reference(s => s.Film).Load();
            }

            return schedules;
        }
    }
    public static List<Schedule> GetAvailableSchedules(DateTime startDate, DateTime endDate)
    {
        using (DataBaseConnection db = new DataBaseConnection())
        {
            var schedules = db.Schedule.Include(s => s.Film)
                .Where(s => s.StartDate >= startDate && s.EndDate < endDate).ToList();

            foreach (var s in schedules)
            {
                db.Entry(s).Reference(s => s.Film).Load();

                // Define hall capacities based on hall ID
                int hallCapacity = 0;
                switch (s.Hall_ID)
                {
                    case 1: // Hall 1 zet deze op 4 of iets als je wilt testen
                        hallCapacity = 150;
                        break;
                    case 2: // Hall 2
                        hallCapacity = 300;
                        break;
                    case 3: // Hall 3
                        hallCapacity = 500;
                        break;
                }

                
                int soldTicketsCount = db.Ticket.Count(t => t.Schedule_ID == s.ID);
                if (soldTicketsCount >= hallCapacity)
                {
                    s.SoldOut = true;
                }
            }
            return schedules;
        }
    }
}


