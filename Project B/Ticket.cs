using System.CodeDom;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;

public class Ticket
{
    public int ID { get; set; }
    public int Schedule_ID { get; set; }
    public int User_ID { get; set; }
    public int Movie_ID { get; set; }
    public int Chair_ID { get; set; }
    public double Price { get; set; }
    public DateTime DateBought { get; set; }

    public string ReservationNumber { get; set; }

    public double CreateTicket(
        Schedule schedule,
        int chair_ID,
        int movieId,
        double price,
        int? userId = null,
        string? reservationNumber = ""
    )
    {
        Schedule_ID = schedule.ID;
        Chair_ID = chair_ID;
        Movie_ID = movieId; // Set Movie_ID
        Price = price;
        User_ID = userId ?? 0; // Assign User_ID if it's provided, otherwise use 0 or any other default value
        ReservationNumber = reservationNumber;

        using DataBaseConnection db = new();
        var entry = db.Ticket.Add(
            new Ticket
            {
                Schedule_ID = schedule.ID,
                Chair_ID = chair_ID,
                Movie_ID = movieId,
                Price = price,
                User_ID = userId ?? 0,
                DateBought = DateTime.Now,
                ReservationNumber = reservationNumber
            }
        );
        db.SaveChanges();

        return price;
    }

    public double GetSeatPrice(
        int seatType,
        int seatrow,
        int seatcol,
        Schedule schedule,
        bool qualifyForDiscount
    )
    {
        double basePrice = 30.0; // Base price for all seats
        double priceMultiplier = 1.0; // Multiplier for seat types

        // This is for the red area
        if (
            schedule.Hall_ID == 1
                && seatType == 1
                && seatrow >= 5
                && seatrow <= 8
                && seatcol >= 5
                && seatcol <= 6
            || schedule.Hall_ID == 2
                && seatrow >= 5
                && seatrow <= 12
                && seatcol >= 8
                && seatcol <= 9
            || seatrow >= 6 && seatrow <= 11 && seatcol >= 7 && seatcol <= 10
            || seatrow >= 7 && seatrow <= 10 && seatcol >= 6 && seatcol <= 11
            || schedule.Hall_ID == 3
                && seatrow >= 4
                && seatrow <= 12
                && seatcol >= 13
                && seatcol <= 16
            || seatrow >= 5 && seatrow <= 11 && seatcol >= 12 && seatcol <= 17
            || seatrow >= 6 && seatrow <= 11 && seatcol >= 11 && seatcol <= 18
        )
        {
            priceMultiplier *= 2.0;
        }
        // This is for the orange area
        if (
            schedule.Hall_ID == 1
                && seatType == 2
                && seatrow >= 3
                && seatrow <= 10
                && seatcol <= 4
                && seatcol >= 3
            || seatrow >= 3 && seatrow <= 10 && seatcol <= 8 && seatcol >= 7
            || seatrow >= 9 && seatrow <= 10 && seatcol >= 5 && seatcol <= 6
            || seatrow >= 3 && seatrow <= 4 && seatcol >= 5 && seatcol <= 6
            || schedule.Hall_ID == 2
                && seatType == 2
                && seatrow >= 1
                && seatrow <= 15
                && seatcol >= 6
                && seatcol <= 11
            || seatrow >= 2 && seatrow <= 13 && seatcol >= 5 && seatcol <= 12
            || seatrow >= 4 && seatrow <= 12 && seatcol >= 4 && seatcol <= 13
            || seatrow >= 6 && seatrow <= 11 && seatcol >= 3 && seatcol <= 14
            || seatrow >= 8 && seatrow <= 10 && seatcol >= 2 && seatcol <= 15
            || schedule.Hall_ID == 3
                && seatType == 2
                && seatrow >= 1
                && seatrow <= 16
                && seatcol >= 12
                && seatcol <= 17
            || seatrow >= 1 && seatrow <= 15 && seatcol >= 9 && seatcol <= 20
            || seatrow >= 2 && seatrow <= 13 && seatcol >= 8 && seatcol <= 21
            || seatrow >= 4 && seatrow <= 11 && seatcol >= 7 && seatcol <= 22
            || seatrow >= 6 && seatrow <= 10 && seatcol >= 6 && seatcol <= 23
            || seatrow >= 8 && seatrow <= 9 && seatcol >= 5 && seatcol <= 24
        )
        {
            priceMultiplier *= 1.5;
        }

        // Calculate final price based on base price, multiplier
        double Price = basePrice * priceMultiplier;

        // If the movie is shown during a peak time, increase the price
        if (IsPeakTime(schedule.StartDate))
        {
            Price += 5.0;
        }
        if (IsEarlyTime(schedule.StartDate))
        {
            Price -= 5;
        }

        if (qualifyForDiscount)
        {
            double discountPercentage = 0.10;
            double discountAmount = Price * discountPercentage;
            Price -= discountAmount;
        }

        return Price;
    }

    private bool IsPeakTime(DateTime startTime)
    {
        int hour = startTime.Hour;
        return hour >= 18 && hour <= 22; // Assuming peak hours are from 18 PM to 200 PM
    }

    private bool IsEarlyTime(DateTime startTime)
    {
        // Check if the startTime falls within peak hours
        int hour = startTime.Hour;
        return hour >= 10 && hour <= 14; // Assuming peak hours are from 10 AM to 14 PM
    }

    public void CheckAge(Film film, int age)
    {
        if (age < film.Age)
        {
            AnsiConsole.Write(
                new Rule($"[red]Waarschuwing: dit is een {film.Age}+ film.[/]").RuleStyle("red")
            );
            AnsiConsole.Write(
                new Rule($"[blue]Druk op iets om verder te gaan[/]").RuleStyle("blue")
            );
            Console.ReadKey();
        }
    }

    public static void DisplayTicketDetails(
        int seatType,
        int row,
        int col,
        double price,
        string reservationNumber
    )
    {
        string stoelType = "";
        switch (seatType)
        {
            case 0:
                stoelType = "Classic";
                break;
            case 1:
                stoelType = "ExtraBeenRuimte";
                break;
            case 2:
                stoelType = "LoveSeat";
                break;
        }
        AnsiConsole.Write(new Rule($"[blue]Ticket Informatie [/]").RuleStyle("blue"));
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Stoel type: {stoelType}");
        Console.WriteLine($"Rij: {row}");
        Console.WriteLine($"Nummer: {col}");
        Console.WriteLine($"Prijs: {price} euro");
        Console.WriteLine($"Reserveringsnummer: {reservationNumber}");
        Console.ResetColor();
    }

    public static void SeeUserStats(int userID)
    {
        using (DataBaseConnection db = new DataBaseConnection())
        {
            List<Ticket> userTickets = db.Ticket.Where(t => t.User_ID == userID).ToList();

            var ticketsPerSchedule = userTickets
                .GroupBy(t => new
                {
                    t.Movie_ID,
                    t.Schedule_ID,
                    t.DateBought
                })
                .GroupBy(g => g.Key.Schedule_ID)
                .Select(scheduleGroup => new
                {
                    MovieName = db
                        .Movie.FirstOrDefault(movie =>
                            movie.ID == scheduleGroup.First().Key.Movie_ID
                        )
                        ?.Title,
                    DateBought = scheduleGroup.First().Key.DateBought.ToString("yyyy-MM-dd HH:mm"),
                    ScheduleDate = db
                        .Schedule.FirstOrDefault(s => s.ID == scheduleGroup.First().Key.Schedule_ID)
                        ?.StartDate.ToString("yyyy-MM-dd HH:mm"), // Include the show date
                    ReservationNumbers = scheduleGroup
                        .SelectMany(innerGroup => innerGroup.Select(t => t.ReservationNumber))
                        .Distinct()
                        .ToList(),
                    TicketCount = scheduleGroup.Sum(innerGroup => innerGroup.Count()),
                    TotalPrice = scheduleGroup.Sum(innerGroup => innerGroup.Sum(t => t.Price))
                })
                .ToList();

            if (ticketsPerSchedule.Any())
            {
                var table = new Table();

                table.Border = TableBorder.Rounded;
                table.AddColumn("[blue]Film[/]");
                table.AddColumn("[blue]Reserveringsnummers[/]"); // Changed column header
                table.AddColumn("[blue]Tickets gekocht[/]");
                table.AddColumn("[blue]Totale Prijs[/]");
                table.AddColumn("[blue]Datum gekocht[/]");
                table.AddColumn("[blue]Datum van vertoning[/]");

                foreach (var scheduleInfo in ticketsPerSchedule)
                {
                    // Combine all reservation numbers into a single string
                    string reservationNumbers = string.Join(", ", scheduleInfo.ReservationNumbers);

                    table.AddRow(
                        new Markup($"[blue]{scheduleInfo.MovieName}[/]"),
                        new Markup($"[blue]{reservationNumbers}[/]"), // Show all reservation numbers
                        new Markup($"[blue]{scheduleInfo.TicketCount}[/]"),
                        new Markup($"[blue]{scheduleInfo.TotalPrice.ToString()} euro[/]"), // Formatting as currency
                        new Markup($"[blue]{scheduleInfo.DateBought}[/]"),
                        new Markup($"[blue]{scheduleInfo.ScheduleDate}[/]")
                    );
                }

                // Create a bordered panel with a specific color
                var panel = new Panel(table)
                    .Header(
                        "[bold blue]Overzicht van de bezochte films en totale tickets per schema[/]"
                    )
                    .BorderColor(Color.Blue);

                AnsiConsole.Render(panel);
            }
            else
            {
                AnsiConsole.Write("[red]Er zijn nog geen tickets op deze account gekocht. \n[/]");
            }
        }
    }

    public static bool UserTicketDiscount(int userID)
    {
        using DataBaseConnection db = new();

        Users user = db.Users.FirstOrDefault(u => u.ID == userID);

        if (user != null)
        {
            int totalVisits = user.Visits;
            List<int> wantedTotalVisits = new List<int> { 11, 21, 31, 41, 51, 61, 71, 81, 91 };

            if (wantedTotalVisits.Contains(totalVisits))
            {
                user.DiscountReceived = true;
                db.SaveChanges();
                Console.WriteLine("Gefeliciteerd u ontvangt korting op uw reservering!");
                return true;
            }
        }

        return false;
    }

    public static string GenerateReservationNumber()
    {
        string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        Random random = new Random();
        StringBuilder reservationNumber = new StringBuilder(6);

        for (int i = 0; i < 6; i++)
        {
            int index = random.Next(characters.Length);
            reservationNumber.Append(characters[index]);
        }

        return reservationNumber.ToString();
    }

    public static void DecideSeatTypeName(
        int classicSeatsCounter,
        int loveSeatsCounter,
        int ExtraLegRoomCounter,
        double totalClassicseatPrice,
        double totalLoveseatPrice,
        double totalExtraLegroomPrice
    )
    {
        string seatTypeName;
        if (classicSeatsCounter != 0)
        {
            seatTypeName = "Classic Seat";
            AnsiConsole.Markup(
                $"[blue]StoelType:[/] [white]{seatTypeName, -15} X{classicSeatsCounter}[/] [blue]Prijs:[/] [white]{totalClassicseatPrice}[/][blue] euro[/]\n"
            );
        }
        if (ExtraLegRoomCounter != 0)
        {
            seatTypeName = "ExtraBeenRuimte";
            AnsiConsole.Markup(
                $"[blue]StoelType:[/] [white]{seatTypeName} X{ExtraLegRoomCounter}[/] [blue]Prijs:[/] [white]{totalExtraLegroomPrice}[/][blue] euro[/]\n"
            );
        }
        if (loveSeatsCounter != 0)
        {
            seatTypeName = "LoveSeat";
            AnsiConsole.Markup(
                $"[blue]StoelType:[/] [white]{seatTypeName, -15} X{loveSeatsCounter}[/] [blue]Prijs:[/] [white]{totalLoveseatPrice}[/][blue] euro[/]\n"
            );
        }
    }

    public static Dictionary<string, int> GetGenreCount(int userID) // dictionary met genre en bijbehorende count
    {
        using (DataBaseConnection db = new DataBaseConnection())
        {
            List<Ticket> userTickets = db.Ticket.Where(t => t.User_ID == userID).ToList(); // gets all tickets from user
            List<int> movieIds = userTickets.Select(t => t.Movie_ID).Distinct().ToList(); // gets the movie id's from each ticket
            List<string> genres = db
                .Movie.Where(m => movieIds.Contains(m.ID))
                .Select(m => m.Categories)
                .Distinct()
                .ToList(); // gets the genre of each movie
            Dictionary<string, int> genreCounts = new Dictionary<string, int>();
            foreach (var genre in genres)
            {
                int genreMovieCount = movieIds.Count(m =>
                    db.Movie.First(mo => mo.ID == m).Categories == genre
                );
                genreCounts.Add(genre, genreMovieCount);
            }
            return genreCounts;
        }
    }

    public static string FiveTicketsGenre(int userID) // gets the genre that has been bought atleast 5 times
    {
        var genreCounts = GetGenreCount(userID);
        var atleast5 = genreCounts.Where(g => g.Value >= 5);
        if (!atleast5.Any())
            return null;
        var mostWatchedGenre = atleast5.OrderByDescending(g => g.Value).First();
        return mostWatchedGenre.Key;
    }
}
