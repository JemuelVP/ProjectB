using Spectre.Console;
class RevenueStatistics
{
    public static List<dynamic> GetTotalTicketsPerSeatType()
    {
        using DataBaseConnection db = new();
        var seatTypeCounts = db.Ticket
            .Join(db.Chair,
                t => t.Chair_ID, // Adjust this to match the actual foreign key property in your Ticket class
                c => c.ID,
                (t, c) => new { t, c })
            .GroupBy(tc => tc.c.SeatType)
            .Select(g => new 
            {
                SeatType = g.Key,
                Count = g.Count()
            }).ToList<dynamic>();
        return seatTypeCounts;
    }
    public void GetTotalRevenue()
    {
        double totalRevenue = 0;
        using DataBaseConnection db = new();
        var tickets = db.Ticket.ToList();

        foreach (var ticket in tickets)
        {
            totalRevenue += ticket.Price;
        }
        Console.ForegroundColor = ConsoleColor.Blue;
        AnsiConsole.WriteLine($"Totale omzet: {totalRevenue}");
    }
    public static double GetTotalPricePerMovie(int movieID)
    {
        using DataBaseConnection db = new();
        var totalPrice = db.Ticket.Where(t => t.Movie_ID == movieID).Sum(t => t.Price);
        return totalPrice;
    }
    public static int GetTotalTicketsPerMovie(int movieID)
    {
        using DataBaseConnection db = new();
        var totalTicket = db.Ticket.Where(t => t.Movie_ID == movieID).Count();
        return totalTicket;
    }
    public static List<dynamic> GetTotalTicketsPerSeatType(int movieID)
    {
        using DataBaseConnection db = new();
        var seatTypeCounts = db.Ticket
            .Where(t => t.Movie_ID == movieID)
            .Join(db.Chair, 
                t => t.Chair_ID, // Adjust this to match the actual foreign key property in your Ticket class
                c => c.ID,
                (t, c) => new { t, c })
            .GroupBy(tc => tc.c.SeatType)
            .Select(g => new 
            {
                SeatType = g.Key,
                Count = g.Count()
            }).ToList<dynamic>();
        return seatTypeCounts;
    }
}