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

    public double CreateTicket(
        Schedule schedule,
        int chair_ID,
        int movieId,
        double price,
        int? userId = null
    )
    {
        Schedule_ID = schedule.ID;
        Chair_ID = chair_ID;
        Movie_ID = movieId; // Set Movie_ID
        Price = price;
        User_ID = userId ?? 0; // Assign User_ID if it's provided, otherwise use 0 or any other default value

        using DataBaseConnection db = new();
        var entry = db.Ticket.Add(
            new Ticket
            {
                Schedule_ID = schedule.ID,
                Chair_ID = chair_ID,
                Movie_ID = movieId,
                Price = price,
                User_ID = userId ?? 0,
                DateBought = DateTime.Now
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
        // Here you can implement your pricing logic based on seat type, seat number, and schedule
        // For demonstration purposes, let's say you have a simple pricing logic
        double basePrice = 30.0; // Base price for all seats
        double priceMultiplier = 1.0; // Multiplier for seat types

        // Adjust price based on seat type
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
            priceMultiplier *= 2.0; // Multiply the price by 1.5
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

        // Calculate final price based on base price, multiplier, or any other factors
        double Price = basePrice * priceMultiplier;

        // Example: If the movie is shown during a peak time, increase the price
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
        // Example implementation: Check if the startTime falls within peak hours
        // You can adjust this based on your business logic
        int hour = startTime.Hour;
        return hour >= 18 && hour <= 22; // Assuming peak hours are from 6 PM to 10 PM
    }

    private bool IsEarlyTime(DateTime startTime)
    {
        // Example implementation: Check if the startTime falls within peak hours
        // You can adjust this based on your business logic
        int hour = startTime.Hour;
        return hour >= 10 && hour <= 14; // Assuming peak hours are from 6 PM to 10 PM
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

    public static void DisplayTicketDetails(int seatType, int row, int col, double price)
    {
        string stoelType;
        switch (seatType)
        {
            case 1:
                stoelType = "LoveSeat";
                break;
            case 2:
                stoelType = "ExtraBeenRuimte";
                break;
            default:
                stoelType = "Classic";
                break;
        }
        AnsiConsole.Write(new Rule($"[blue]Ticket Informatie [/]").RuleStyle("blue"));
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Stoel type: {stoelType}");
        Console.WriteLine($"Rij: {row}");
        Console.WriteLine($"Nummer: {col}");
        Console.WriteLine($"Prijs: {price} euro");
        Console.ResetColor();
    }

    public static void SeeUserStats(int userID)
    {
        using (DataBaseConnection db = new DataBaseConnection())
        {
            List<Ticket> userTickets = db.Ticket.Where(t => t.User_ID == userID).ToList();

            var ticketsPerSchedule = userTickets
                .GroupBy(t => new { t.Movie_ID, t.Schedule_ID, t.DateBought })
                .GroupBy(g => g.Key.Schedule_ID) // Group by Schedule_ID
                .Select(scheduleGroup => new
                {
                    MovieID = scheduleGroup.First().Key.Movie_ID, // Accessing Movie_ID from the key of the first item in the group
                    MovieName = db.Movie.FirstOrDefault(movie => movie.ID == scheduleGroup.First().Key.Movie_ID)?.Title,
                    DateBought = scheduleGroup.First().Key.DateBought.ToString("yyyy-MM-dd HH:mm"),
                    TicketIDs = scheduleGroup.SelectMany(innerGroup => innerGroup.Select(t => t.ID)), // Collecting all ticket IDs within the group
                    TicketCount = scheduleGroup.Sum(innerGroup => innerGroup.Count()), // Sum of counts of inner groups
                    TotalPrice = scheduleGroup.Sum(innerGroup => innerGroup.Sum(t => t.Price)) // Sum of total prices of inner groups
                })
                .ToList();

            if (ticketsPerSchedule.Any())
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Overview of visited movies and total tickets per schedule");
                Console.ResetColor();

                foreach (var scheduleInfo in ticketsPerSchedule)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(
                        $"Movie: {scheduleInfo.MovieName}, Ticket IDS: {string.Join(", ", scheduleInfo.TicketIDs)} , Tickets bought: {scheduleInfo.TicketCount}, Total Price: {scheduleInfo.TotalPrice}, Date Bought: {scheduleInfo.DateBought}"
                    );
                    Console.ResetColor();
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No tickets have been purchased on this account yet");
                Console.ResetColor();
            }
        }
    }



    public static bool UserTicketDiscount(int userID)
    {
        using DataBaseConnection db = new();

        Users user = db.Users.FirstOrDefault(u => u.ID == userID);

        if (!user.DiscountReceived)
        {
            List<Ticket> userTickets = db.Ticket.Where(t => t.User_ID == userID).ToList();
            int totalVisits = user.Visits;
            List<int> wantedTotalVisits = new List<int> { 11, 21, 31, 41, 51, 61, 71, 81, 91 };

            foreach (int visit in wantedTotalVisits)
            {
                if (totalVisits == visit)
                {
                    Console.WriteLine(
                        "Gefeliciteerd je ontvangt korting op je volgende bestelling!"
                    );
                    user.DiscountReceived = true;
                    db.SaveChanges();
                    return true;
                }
            }
        }

        return false;
    }
}
