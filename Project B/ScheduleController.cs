using System.Data.Entity;
public class ScheduleController
{

   public static List<Schedule> GetAvailableSchedules(DateTime startDate, DateTime endDate)
    {
        using (DataBaseConnection db = new DataBaseConnection())
        {
            var schedules = db.Schedule
                .Include(s => s.Film)
                .Where(s => s.StartDate >= startDate && s.EndDate < endDate)
                .ToList();

            foreach (var s in schedules)
            {
                db.Entry(s).Reference(s => s.Film).Load();

                // Define hall capacities based on hall ID
                int hallCapacity = 150;
                switch (s.Hall_ID)
                {
                    case 2: // Hall 2
                        hallCapacity = 300;
                        break;
                    case 3: // Hall 3
                        hallCapacity = 500;
                        break;
                }

                // Check sold tickets count
                int soldTicketsCount = db.Ticket.Count(t => t.Schedule_ID == s.ID);

                // If sold tickets count equals or exceeds hall capacity, remove the schedule
                if (soldTicketsCount >= hallCapacity)
                {
                    schedules.Remove(s);
                }
            }

            return schedules;
        }
    }

}