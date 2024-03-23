using System.Data.SQLite;

public class DatabaseConnection
{
    public static void CreateDatabase()
    {
        string connectionString = "Data Source=movie.sqlite;Version=3;";
        using (SQLiteConnection connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            string createTableQuery = "CREATE TABLE IF NOT EXISTS Admin (Name TEXT, Password TEXT)";
            string createMovieQuery = "CREATE TABLE IF NOT EXISTS Movie (ID INTEGER  PRIMARY KEY AUTOINCREMENT , Title TEXT, Year INT, Price INT, Description TEXT, Authors TEXT, Categories TEXT, Directors TEXT, Age INT, DurationInMin INT)";
            string createScheduleQuery = "CREATE TABLE IF NOT EXISTS Schedule (ID INTEGER  PRIMARY KEY AUTOINCREMENT, Movie_ID INT, Hall_ID INT, SoldSeats INT, StartDate DATETIME, EndDate DATETIME)";
            string createHallQuery = "CREATE TABLE IF NOT EXISTS Hall (ID INTEGER  PRIMARY KEY AUTOINCREMENT, Name TEXT, ChairsAmount INT)";
            string createTicketQuery = "CREATE TABLE IF NOT EXISTS Ticket (ID INTEGER  PRIMARY KEY AUTOINCREMENT, UserName TEXT, Schedule_ID INT, SeatNumber INT, SeatType TEXT, Price INT, Movie_ID)";
            using (SQLiteCommand createTableCommand = new SQLiteCommand(createTableQuery, connection))
            {
                createTableCommand.ExecuteNonQuery();
            }
            using (SQLiteCommand createTableCommand = new SQLiteCommand(createMovieQuery, connection))
            {
                createTableCommand.ExecuteNonQuery();
            }
            using (SQLiteCommand createTableCommand = new SQLiteCommand(createScheduleQuery, connection))
            {
                createTableCommand.ExecuteNonQuery();
            }
            using (SQLiteCommand createTableCommand = new SQLiteCommand(createHallQuery, connection))
            {
                createTableCommand.ExecuteNonQuery();
            }
            using (SQLiteCommand createTableCommand = new SQLiteCommand(createTicketQuery, connection))
            {
                createTableCommand.ExecuteNonQuery();
            }
        }
    }
}
