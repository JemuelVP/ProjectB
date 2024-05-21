class ChairController
{
    // seattype = 0 = normal
    // seattype = 1 = extra beenruimte
    // seattype  = 2 = loveseat
    // public void AddChairs()
    // {
    //     using DataBaseConnection db = new();
    //     // Assuming you want to add chairs
    //     // change height to number of rows in hall
    //     // change width to number of columns in hall
    //     // seattype = 0 = normal
    //     // seattype = 1 = extra beenruimte
    //     // seattype  = 2 = loveseat
    //     for (var row = 0; row < 20; row++)
    //     {
    //         for (var col = 0; col < 30; col++)
    //         {
    //             if                 
    //             (
    //                 (row >= 4 && row <= 12 && col >= 13 && col <= 16) ||
    //                 (row >= 5 && row <= 11 && col >= 12 && col <= 17) ||
    //                 (row >= 6 && row <= 11 && col >= 11 && col <= 18)
    //             )
    //             {
    //                 db.Chair.Add(new Chair
    //                 {
    //                     SeatType = 2,
    //                     Row = row,
    //                     Column = col,
    //                     CinemaHallID = 3
    //                 });
    //             }
    //             else if 
    //             (
    //                 (row >= 1 && row <= 16 && col >= 12 && col <= 17) ||
    //                 (row >= 1 && row <= 15 && col >= 9 && col <= 20) ||
    //                 (row >= 2 && row <= 13 && col >= 8 && col <= 21) ||
    //                 (row >= 4 && row <= 11 && col >= 7 && col <= 22) ||
    //                 (row >= 6 && row <= 10 && col >= 6 && col <= 23) ||
    //                 (row >= 8 && row <= 9 && col >= 5 && col <= 24)
    //             )
    //             {
    //                 db.Chair.Add(new Chair
    //                 {
    //                     SeatType = 1,
    //                     Row = row,
    //                     Column = col,
    //                     CinemaHallID = 3
    //                 });
    //             }
    //             else if 
    //             (
    //                 (row >= 0 && row <= 6 && col == 0) ||
    //                 (row >= 0 && row <= 5 && col == 1) ||
    //                 (row >= 0 && row <= 4 && col == 2) ||
    //                 (row == 0 && col == 3) ||
    //                 (row >= 0 && row <= 6 && col == 29) ||
    //                 (row >= 0 && row <= 5 && col == 28) ||
    //                 (row >= 0 && row <= 4 && col == 27) ||
    //                 (row == 0 && col == 26) ||
    //                 (row >= 12 && row <= 19 && col == 0) ||
    //                 (row >= 13 && row <= 19 && col == 1) ||
    //                 (row >= 15 && row <= 19 && col == 2) ||
    //                 (row >= 17 && row <= 19 && col >= 3 && col <= 4) ||
    //                 (row >= 18 && row <= 19 && col >= 5 && col <= 6) ||
    //                 (row == 19 && col == 7) ||
    //                 (row >= 12 && row <= 19 && col == 29) ||
    //                 (row >= 13 && row <= 19 && col == 28) ||
    //                 (row >= 15 && row <= 19 && col == 27) ||
    //                 (row >= 17 && row <= 19 && col >= 25 && col <= 26) ||
    //                 (row >= 18 && row <= 19 && col >= 23 && col <= 24) ||
    //                 (row == 19 && col == 22)
    //             )
    //             {
    //                 // skip
    //             }
    //             else
    //             {
    //                 db.Chair.Add(new Chair
    //                 {
    //                     SeatType = 0,
    //                     Row = row,
    //                     Column = col,
    //                     CinemaHallID = 3
    //                 });
    //             }
    //         }
    //     }

    //     db.SaveChanges();
    // }


    public static List<Chair> GetAvailableSeats(int scheduleId, int hallId)
    {
        using DataBaseConnection db = new();
        var chairs = db.Chair
        .Where(chair => !db.Ticket
        .Where(ticket => ticket.Schedule_ID == scheduleId)
        .Select(ticket => ticket.Chair_ID)
        .Contains(chair.ID)  && chair.CinemaHallID == hallId).ToList();

        return chairs;
    }
}