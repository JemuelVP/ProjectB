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
    public static List<List<Chair>> GetAvailableAdjecentSeats(int amountOfChairs, int scheduleId, int hallId, int seatType)
    {
        // check if amount chairs and amount positions conjoined is available
        List<List<Chair>> availableGroupSeatsToReturn = new(); 
        var availableSeats = GetAvailableSeats(scheduleId, hallId, seatType);
        availableSeats.OrderBy(item => item.Position);
        for(int pos = 0; pos < availableSeats.Count; pos++)
        {
            var chair = availableSeats[pos];

                var nextChair = availableSeats[pos+amountOfChairs];
                if (nextChair.Position == chair.Position + amountOfChairs)
                {
                    List<Chair> row = new();
                    //get the row of available chairs
                    for(int i = 0; i < amountOfChairs; i++ )
                    {
                        row.Add(availableSeats[pos+i]);
                    }
                    //add to the list of rows with availeable chairs
                    availableGroupSeatsToReturn.Add(row);
                } 
        }
        return availableGroupSeatsToReturn;
    }
}