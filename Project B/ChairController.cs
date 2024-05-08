class ChairController
{
    // public void AddChairs()
    // {
    //     using DataBaseConnection db = new();
    //     // Assuming you want to add chairs
    //     for (int i = 1; i <= 30; i++)
    //     {
    //         db.Chair.Add(new Chair
    //         {
    //             SeatType = 2,
    //             Position = i ,// Position is always plus 1
    //             CinemaHallID = 3 // Assuming you have a CinemaHallID property in your Chair entity
    //         });

    //     }

    //     db.SaveChanges();
    // }
    public static List<Chair> GetAvailableSeats(int scheduleId, int hallId, int seatType)
    {
        using DataBaseConnection db = new();
        var chairs = db.Chair
        .Where(chair => !db.Ticket
        .Where(ticket => ticket.Schedule_ID == scheduleId)
        .Select(ticket => ticket.Chair_ID)
        .Contains(chair.ID) && chair.SeatType == seatType && chair.CinemaHallID == hallId).ToList();

        return chairs;
    }
}