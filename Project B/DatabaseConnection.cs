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
            using (SQLiteCommand createTableCommand = new SQLiteCommand(createTableQuery, connection))
            {
                createTableCommand.ExecuteNonQuery();
            }
            using (SQLiteCommand createTableCommand = new SQLiteCommand(createMovieQuery, connection))
            {
                createTableCommand.ExecuteNonQuery();
            }
        }
    }
}
