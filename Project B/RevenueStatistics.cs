class RevenueStatistics
{
    public void GetTotalRevenue()
    {
        double totalRevenue = 0;
        using DataBaseConnection db = new();
        var tickets = db.Ticket.ToList();

        foreach (var ticket in tickets)
        {
            totalRevenue += ticket.Price;
        }
        Console.WriteLine($"Totale omzet: {totalRevenue}");
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
}