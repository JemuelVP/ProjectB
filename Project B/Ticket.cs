using Spectre.Console;



public class Ticket
{
    public int ID { get; set; }
    public int Schedule_ID { get; set; }
    public int User_ID { get; set; }
    public int Movie_ID { get; set; }
    public int Chair_ID{get; set;}
    public double Price { get; set; }

    public double CreateTicket(Schedule schedule, int chair_ID, int movieId, double price, int? userId = null)
    {
        Schedule_ID = schedule.ID;
        Chair_ID = chair_ID;
        Movie_ID = movieId; // Set Movie_ID
        Price = price;
        User_ID = userId ?? 0; // Assign User_ID if it's provided, otherwise use 0 or any other default value

        using DataBaseConnection db = new();
        var entry = db.Ticket.Add(this);
        db.SaveChanges();

        return price;
    }
    public double GetSeatPrice(int seatType, int seatNumber, Schedule schedule)
    {
        // Here you can implement your pricing logic based on seat type, seat number, and schedule
        // For demonstration purposes, let's say you have a simple pricing logic
        double basePrice = 30.0; // Base price for all seats
        double priceMultiplier = 1.0; // Multiplier for seat types

        // Adjust price based on seat type
        if (seatType == 0 && seatNumber >= 50 && seatNumber <= 59 || seatType == 1 && seatNumber >= 1 && seatNumber <= 5 || seatType == 2 && seatNumber >= 1 && seatNumber <= 3 )
        {
            priceMultiplier *= 2.0; // Multiply the price by 1.5
        }
        if (seatType == 0 && seatNumber >= 40 && seatNumber <= 49 || seatType == 1 && seatNumber >= 6 && seatNumber <= 10 || seatType == 2 && seatNumber >= 8 && seatNumber <= 10 )
        {
            priceMultiplier *= 1.5; // Multiply the price by 1.5
        }


        // Calculate final price based on base price, multiplier, or any other factors
        double Price = basePrice * priceMultiplier;

        // Example: If the movie is shown during a peak time, increase the price
        if (IsPeakTime(schedule.StartDate))
        {
            Price += 5.0;
        }
        if(IsEarlyTime(schedule.StartDate))
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

    public void CheckAge(Film film, int age)
    {
        if (age < film.Age)
        {
            AnsiConsole.Write(new Rule($"[red]Waarschuwing: dit is een {film.Age}+ film.[/]").RuleStyle("red"));
            AnsiConsole.Write(new Rule($"[blue]Druk op iets om verder te gaan[/]").RuleStyle("blue"));
            Console.ReadKey();
        }
    }

public static void DisplayTicketDetails(Ticket ticket,Chair chair, double price)
{
    AnsiConsole.Write(new Rule($"[blue]Ticket Informatie [/]").RuleStyle("blue"));
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine($"Ticket ID: {ticket.ID}");
    Console.WriteLine($"Stoel type: {chair.SeatType}");
    Console.WriteLine($"Stoel nummer: {chair.Position}");
    Console.WriteLine($"Prijs: {price} euro");
    Console.ResetColor();
}


}
