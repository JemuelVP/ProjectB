using System.Data.SQLite;
using System.Runtime.CompilerServices;


public class CinemaHall
{
    public int ID;
    public int numberOfSeats;
    public List<Chair> Chairs;

    public CinemaHall(int id, int numberofseats)
    {
        ID = id;
        numberOfSeats = numberofseats;
        Chairs = new List<Chair>();
    }

    public virtual void FillChairs(int id, string name, double price)
    {
        for (int i = 0; i < numberOfSeats; i++)
        {
            Chairs.Add(new Chair(i, name, price));
        }
    }

//this is the baseclass CinemaHall, it contains a list of chairs, numberOfSeats is the amount of seats
//it has three deriven classes.


    public void AddHall(string name, int chairsAmount)
    {
        // Connection string to connect to the SQLite database
        string connectionString = "Data Source=movie.sqlite;Version=3;";

        // Using statement ensures the connection is closed properly after use
        using (SQLiteConnection connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            // SQL query to insert data into the Movie table
            string insertQuery = "INSERT INTO Hall (Name, ChairsAmount) " +
                                 "VALUES (@Name, @ChairsAmount)";
            // Using the '@' symbol before the "" so you can include the '' without escaping them
            // Using statement ensures the command is disposed properly after use
            using (SQLiteCommand insertCommand = new SQLiteCommand(insertQuery, connection))
            {
                insertCommand.Parameters.AddWithValue("@Name", name);
                insertCommand.Parameters.AddWithValue("@ChairsAmount", chairsAmount);

                // Execute the command to insert data into the database
                insertCommand.ExecuteNonQuery();
            }
        }
    }


    public void CountTicketsPerFilm()
    {
        string connectionString = "Data Source=movie.sqlite;Version=3;";

        using (SQLiteConnection connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            //counts the combinations of the same schedule id and movie id in the Ticket table and updates the SoldSeats in the Schedule table  
            string updateSoldSeats = "UPDATE SCHEDULE " +
                                     "SET SoldSeats = (" +
                                     "SELECT COUNT(*) FROM Ticket " +
                                     "WHERE Ticket.Schedule_ID = Schedule.ID AND Ticket.Movie_ID = Schedule.Movie_ID)";

            using (SQLiteCommand updateCommand = new SQLiteCommand(updateSoldSeats, connection))
            {
                updateCommand.ExecuteNonQuery();
            }
        }
        

    }
    
    //Method to insert Schedule data into the database
    public void OverviewSchedules(int id, int movie_id, int hall_id, int soldseats, DateTime startdate, DateTime enddate)
    {

        string connectionString = "Data Source=movie.sqlite;Version=3;";

        // Using statement ensures the connection is closed properly after use
        using (SQLiteConnection connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            // SQL query to insert data into the Schedule table
            string insertQuery = "INSERT INTO Schedule (ID, Movie_ID, Hall_ID, SoldSeats, StartDate, EndDate) " +
                                 "VALUES (@ID, @Movie_ID, @Hall_ID, @SoldSeats, @StartDate, @EndDate)";
            // Using the '@' symbol before the "" so you can include the '' without escaping them
            // Using statement ensures the command is disposed properly after use
            using (SQLiteCommand insertCommand = new SQLiteCommand(insertQuery, connection))
            {
                insertCommand.Parameters.AddWithValue("@ID", id);
                insertCommand.Parameters.AddWithValue("@Movie_ID", movie_id);
                insertCommand.Parameters.AddWithValue("@Hall_ID", hall_id);
                insertCommand.Parameters.AddWithValue("@SoldSeats", soldseats);
                insertCommand.Parameters.AddWithValue("@StartDate", startdate.ToString("dd-MM-yyyy"));
                insertCommand.Parameters.AddWithValue("@EndDate", enddate.ToString("dd-MM-yyyy"));

            
                // Execute the command to insert data into the database
                insertCommand.ExecuteNonQuery();
            }
        }
    }
    
    
    //Method to insert ticket data into the database
    public void OverviewTickets(int id, string username, int schedule_id, int seatnumber, string seattype,double price, int movie_id)
    {
        string connectionString = "Data Source=movie.sqlite;Version=3;";

        // Using statement ensures the connection is closed properly after use
        using (SQLiteConnection connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            // SQL query to insert data into the Ticket table
            string insertQuery = "INSERT INTO Ticket (ID, UserName, Schedule_ID, SeatNumber, SeatType, Price, Movie_ID) " +
                                 "VALUES(@ID, @UserName, @Schedule_ID, @SeatNumber, @SeatType, @Price, @Movie_ID)";
            // Using the '@' symbol before the "" so you can include the '' without escaping them
            // Using statement ensures the command is disposed properly after use
            using (SQLiteCommand insertCommand = new SQLiteCommand(insertQuery, connection))
            {
                insertCommand.Parameters.AddWithValue("@ID", id);
                insertCommand.Parameters.AddWithValue("@UserName", username);
                insertCommand.Parameters.AddWithValue("@Schedule_ID", schedule_id);
                insertCommand.Parameters.AddWithValue("@SeatNumber", seatnumber);
                insertCommand.Parameters.AddWithValue("@SeatType", seattype);
                insertCommand.Parameters.AddWithValue("@Price", price);
                insertCommand.Parameters.AddWithValue("@Movie_ID", movie_id);
            
                // Execute the command to insert data into the database
                insertCommand.ExecuteNonQuery();
            }
        }
    }




}
