using Spectre.Console;
class RevenueStatistics
{
    public static List<dynamic> GetTotalTicketsPerSeatType()
    {
        using DataBaseConnection db = new();
        var seatTypeCounts = db.Ticket
            .Join(db.Chair,
                t => t.Chair_ID, 
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
                t => t.Chair_ID,
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
    public static void GenerateCSVFile()
    {   
        //getallmovies pakt alle films ookal zijn ze niet gepland
        using DataBaseConnection db = new();
        var adminOverview = AdminController.GetAllMovies();
        var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        var projectDirectory = new DirectoryInfo(baseDirectory).Parent?.Parent?.Parent;
        if(projectDirectory is null)
        {
            return;
        }
        var CsvFilePath = Path.Combine(projectDirectory.FullName,"StatsPerMovie.csv");

        //false overwrites if there is something in the csv file 
        using (var writer = new StreamWriter(CsvFilePath,false))
        {
            writer.WriteLine("Titel, AantalClassic, AantalExtraBeenRuimte, AantalLoveSeat, Totaleprijs");

            foreach (var movie in adminOverview)
            {   
                double totalPricePerMovie = GetTotalPricePerMovie(movie.ID);
    
                int countClassicSeats = getCountPerSeatType(movie,0);
                int countExtraLegRoom = getCountPerSeatType(movie,1);
                int countLoveSeats = getCountPerSeatType(movie,2);
                writer.WriteLine($"Titel: {movie.Title}, Classic: {countClassicSeats}, ExtraBeenRuimte: {countExtraLegRoom}, LoveSeats: {countLoveSeats}, TotalePrijs: {totalPricePerMovie} euro");
                
            }
            writer.Flush();
        }
        Console.WriteLine("CSV file created successfully.");
    }
    public static int getCountPerSeatType(Film movie, int seattype)
    {
        using DataBaseConnection db = new();
        return (from T in db.Ticket
                join C in db.Chair on T.Chair_ID equals C.ID
                where T.Movie_ID == movie.ID && C.SeatType == seattype
                select T).Count();

    }
}
