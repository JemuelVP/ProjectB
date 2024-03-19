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
            using (SQLiteCommand createTableCommand = new SQLiteCommand(createTableQuery, connection))
            {
                createTableCommand.ExecuteNonQuery();
            }
        }
    }
}
