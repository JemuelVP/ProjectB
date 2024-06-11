using Spectre.Console;
using System.Net;
using System.Net.Mail;
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
            writer.WriteLine("Titel, Aantal ClassicSeats, Aantal ExtraBeenRuimte, Aantal LoveSeats, Totaleprijs");

            foreach (var movie in adminOverview)
            {   
                double totalPricePerMovie = GetTotalPricePerMovie(movie.ID);
    
                int countClassicSeats = getCountPerSeatType(movie,0);
                int countExtraLegRoom = getCountPerSeatType(movie,1);
                int countLoveSeats = getCountPerSeatType(movie,2);
                writer.WriteLine($"{movie.Title}, {countClassicSeats}, {countExtraLegRoom}, {countLoveSeats}, {totalPricePerMovie} euro");
                
            }
            writer.Flush();
        }

        //SEND EMAIL

        SendEmail(CsvFilePath);
    }
    public static int getCountPerSeatType(Film movie, int seattype)
    {
        using DataBaseConnection db = new();
        return (from T in db.Ticket
                join C in db.Chair on T.Chair_ID equals C.ID
                where T.Movie_ID == movie.ID && C.SeatType == seattype
                select T).Count();

    }


    public static void SendEmail(string csvPath)
    {
    try
    {
        using (MailMessage mail = new MailMessage())
        using (SmtpClient smtpServer = new SmtpClient("smtp-mail.outlook.com"))
        {   
            
            mail.From = new MailAddress("youreyesbioscoop@hotmail.com");
            mail.To.Add("1073976@hr.nl");
            mail.Subject = "CSV Bestand Film Statistieken YourEyes";
            
           // mail.Body = "Beste Marcel,\nHier is uw aangevraagde CSV bestand met de film statistieken";

            Attachment attachment = new Attachment(csvPath);
            mail.Attachments.Add(attachment);

            smtpServer.Port = 587;
            smtpServer.Credentials = new NetworkCredential("youreyesbioscoop@hotmail.com", "rkmxzxxugkjsizwm"); 
            smtpServer.EnableSsl = true;

            smtpServer.Send(mail);
        }
        Console.WriteLine($"Het csv bestand is succesvol verstuurd, check uw email!");
    }
    catch (SmtpException smtpEx)
    {
        Console.WriteLine($"SMTP Exception: {smtpEx.Message}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Exception: {ex.Message}");
    }
    }

}
  
