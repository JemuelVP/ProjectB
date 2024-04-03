using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using Spectre.Console;

public class Ticket
{
    public int ID { get; set; }
    public int Schedule_ID { get; set; }
    public int Movie_ID { get; set; }
    public string? SeatType { get; set; }
    public int SeatNumber { get; set; }
    public double Price { get; set; }
    public string? UserName { get; set; }

    public void CreateTicket(Schedule schedule, int movieId , string userName, string seatType, int seatNumber)
    {
        Schedule_ID = schedule.ID;
        UserName = userName;
        SeatType = seatType;
        SeatNumber = seatNumber;
        Price = Price;
        Movie_ID = movieId; // Set Movie_ID

        using DataBaseConnection db = new();
        var entry = db.Ticket.Add(this);
        db.SaveChanges();
    }
    public void GetSeatPrice(string seatType, int seatNumber)
    {
        Price = 20;
        if(seatType.ToLower() == "classic" &&  seatNumber == 5 &&  seatNumber == 6)
        {
            Price += 5;
        }
        else if (seatType.ToLower() == "classic" &&  seatNumber == 3 || seatNumber == 4 || seatNumber == 7 || seatNumber == 8 )
        {
            Price = Price;
        }
        else if (seatType.ToLower() == "classic" &&  seatNumber == 1 || seatNumber == 2 || seatNumber == 9 || seatNumber == 10 )
        {
            Price -= 5;
        }
        else if(seatType.ToLower() == "loveseat" &&  seatNumber == 5 || seatNumber == 6)
        {
            Price += 10;
        }
        else if (seatType.ToLower() == "loveseat" &&  seatNumber == 3 || seatNumber == 4 || seatNumber == 7 || seatNumber == 8 )
        {
            Price += 5;
        }
        else if (seatType.ToLower() == "loveseat" &&  seatNumber == 1 || seatNumber == 2 || seatNumber == 9 || seatNumber == 10 )
        {
            Price = Price;
        }
        else if(seatType.ToLower() == "extrabeenruimte" &&  seatNumber == 5 ||  seatNumber == 6)
        {
            Price += 10;
        }
        else if (seatType.ToLower() == "extrabeenruimte" &&  seatNumber == 3 || seatNumber == 4 || seatNumber == 7 || seatNumber == 8 )
        {
            Price += 5;
        }
        else if (seatType.ToLower() == "extrabeenruimte" &&  seatNumber == 1 || seatNumber == 2 || seatNumber == 9 || seatNumber == 10 )
        {
            Price = Price;
        }
    }
    public void CheckAge(Film film, int age)
    {
        if (age < film.Age)
        {
            AnsiConsole.WriteLine($"Warning: this is a {film.Age}+ movie.");
        }
    }
}
