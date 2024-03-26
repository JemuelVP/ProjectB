using System.Data.Entity;

public class ScheduleController
{

    public static List<Schedule> GetAvailableSchedules(DateTime startDate, DateTime endDate)
    {
        using DataBaseConnection db = new();
        var schedules = db.Schedule.
        Include(s => s.Film).
        Where(s => s.StartDate >= startDate && s.EndDate < endDate).
        ToList();
        foreach (var s in schedules)
        {
            db.Entry(s).Reference(s => s.Film).Load();
        }
        return schedules;
    }
}