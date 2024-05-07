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

    

}