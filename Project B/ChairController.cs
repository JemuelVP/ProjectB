class ChairController
{
    // seattype = 0 = normal
    // seattype = 1 = extra beenruimte
    // seattype  = 2 = loveseat

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
    public void AddChairs()
    {
        using DataBaseConnection db = new();
        // Assuming you want to add chairs
        // change height to number of rows in hall
        // change width to number of columns in hall
        for (var row = 0; row < 14; row++)
        {
            for (var col = 0; col < 12; col++)
            {
                if (row >= 5 && row <= 8 && col >= 5 && col <= 6)
                {
                    db.Chair.Add(new Chair
                    {
                        SeatType = 1,
                        Row = row,
                        Column = col,
                        CinemaHallID = 1
                    });
                }
                else if (row >= 3 && row <= 10 && col <= 4 && col >= 3)
                {
                    db.Chair.Add(new Chair
                    {
                        SeatType = 2,
                        Row = row,
                        Column = col,
                        CinemaHallID = 1
                    });
                }
                else if (row >= 3 && row <= 10 && col <= 8 && col >= 7)
                {
                    db.Chair.Add(new Chair
                    {
                        SeatType = 2,
                        Row = row,
                        Column = col,
                        CinemaHallID = 1
                    });
                }
                else if (row >= 9 && row <= 10 && col >= 5 && col <= 6)
                {
                    db.Chair.Add(new Chair
                    {
                        SeatType = 2,
                        Row = row,
                        Column = col,
                        CinemaHallID = 1
                    });

                }
                else if (row >= 3 && row <= 4 && col >= 5 && col <= 6)
                {
                    db.Chair.Add(new Chair
                    {
                        SeatType = 2,
                        Row = row,
                        Column = col,
                        CinemaHallID = 1
                    });

                }
                else if (row == 0 && col >= 0 && col <= 1)
                {
                    // skip
                }
                else if (row >= 0 && row <= 2 && col == 0)
                {
                    // skip
                }
                else if (row == 13 && col >= 0 && col <= 1)
                {
                    // skip
                }
                else if (row >= 11 && row <= 13 && col == 0)
                {
                    // skip
                }
                else if (row == 0 && col >= 10 && col <= 11)
                {
                    // skip
                }
                else if (row >= 0 && row <= 2 && col == 11)
                {
                    // skip
                }
                else if (row == 13 && col >= 10 && col <= 11)
                {
                    // skip
                }
                else if (row >= 11 && row <= 13 && col >= 11)
                {
                    // skip
                }
                else
                {
                    db.Chair.Add(new Chair
                    {
                        SeatType = 0,
                        Row = row,
                        Column = col,
                        CinemaHallID = 1
                    });
                }
            }
        }

        db.SaveChanges();
    }
    


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