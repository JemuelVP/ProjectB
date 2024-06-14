class ChairController
{
    public static List<Chair> GetAvailableSeats(int scheduleId, int hallId)
    {
        using DataBaseConnection db = new();
        var chairs = db
            .Chair.Where(chair =>
                !db
                    .Ticket.Where(ticket => ticket.Schedule_ID == scheduleId)
                    .Select(ticket => ticket.Chair_ID)
                    .Contains(chair.ID)
                && chair.CinemaHallID == hallId
            )
            .ToList();

        return chairs;
    }
}
