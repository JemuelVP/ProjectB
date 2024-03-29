using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;

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
    public void GetSeatPrice(string seatType, int seatNumber, CinemaHall hall)
    {
        Price = 20;
        if(seatType.ToLower() == "classic")
        {
            Price = Price;
        }
        else if (seatType.ToLower() == "loveseat")
        {
            Price += 5;
        }
        else if (seatType.ToLower() == "extrabeenruimte")
        {
            Price += 5;
        }
        

        if (hall.NumberOfSeats == 150)
        {
            int maxRow = 15;
            int Row = 10;
            int distanceFromCenter = Math.Abs((maxRow / 2) - Row) + Math.Abs((maxRow / 2) - seatNumber);
            if (distanceFromCenter >= 0 && distanceFromCenter <= 7)
            {
                Price += 10;
            }
            else if (distanceFromCenter > 5 && distanceFromCenter <= 10)
            {
                Price += 5;
            }
            else
            {
                Price = Price;
            }
        }
        else if (hall.NumberOfSeats == 300)
        {
            int maxRow = 20;
            int Row = 15;
            int distanceFromCenter = Math.Abs((maxRow / 2) - Row) + Math.Abs((maxRow / 2) - seatNumber);
            if (distanceFromCenter >= 0 && distanceFromCenter <= 10)
            {
                Price += 10;
            }
            else if (distanceFromCenter > 10 && distanceFromCenter <= 14)
            {
                Price += 5;
            }
            else
            {
                Price = Price;
            }
        }
        else if (hall.NumberOfSeats == 500)
        {
            int maxRow = 25;
            int Row = 20;
            int distanceFromCenter = Math.Abs((maxRow / 2) - Row) + Math.Abs((maxRow / 2) - seatNumber);
            if (distanceFromCenter >= 0 && distanceFromCenter <= 13)
            {
                Price += 10;
            }
            else if (distanceFromCenter > 13 && distanceFromCenter <= 18)
            {
                Price += 5;
            }
            else
            {
                Price = Price;
            }
        }
    }
}
