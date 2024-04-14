using Spectre.Console;

public class Ticket
{
    public int ID { get; set; }
    public int Schedule_ID { get; set; }
    public int User_ID { get; set; }
    public int Movie_ID { get; set; }
    public int Chair_ID { get; set; }
    public double Price { get; set; }

    public void CreateTicket(
        Schedule schedule,
        int chair_ID,
        int movieId,
        double price,
        int? userId
    )
    {
        Schedule_ID = schedule.ID;
        Chair_ID = chair_ID;
        Movie_ID = movieId; // Set Movie_ID
        Price = price;
        User_ID = userId ?? 0; // Assign User_ID if it's provided, otherwise use 0 or any other default value

        using DataBaseConnection db = new();
        var entry = db.Ticket.Add(this);
        db.SaveChanges();
    }

    public double GetSeatPrice(int seatType, int seatNumber, Schedule schedule)
    {
        // Here you can implement your pricing logic based on seat type, seat number, and schedule
        // For demonstration purposes, let's say you have a simple pricing logic
        double basePrice = 20.0; // Base price for all seats
        double priceMultiplier = 1.0; // Multiplier for seat types

        // Adjust price based on seat type
        switch (seatType)
        {
            case 1: // Loveseat
                priceMultiplier = 1.5;
                break;
            case 2: // Extrabeenruimte
                priceMultiplier = 2.0;
                break;
            // Classic seats have the base price, so no need for a case for seatType == 0
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

    public static void DisplayTicketDetails(Ticket ticket, Chair chair, double price)
    {
        Console.WriteLine("Ticket Details:");
        Console.WriteLine("----------------");
        Console.WriteLine($"Ticket ID: {ticket.ID}");
        Console.WriteLine($"Schedule ID: {ticket.Schedule_ID}");
        Console.WriteLine($"Movie ID: {ticket.Movie_ID}");
        Console.WriteLine($"Chair ID: {chair.SeatType}");
        Console.WriteLine($"Chair ID: {chair.Position}");
        Console.WriteLine($"Price: {price:C}"); // Format price as currency
    }

    public static void CheckBoughtTickets(int userID)
    {
        using DataBaseConnection db = new();

        int ticketCount = db.Ticket.Count(t => t.User_ID == userID);

        if (ticketCount >= 3)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("U heeft meer dan 10 tickets bij ons gekocht!");
            Console.WriteLine("Hier een kortingscode voor de volgende bestelling: BIG10");
            Console.ResetColor();
        }
    }
}
