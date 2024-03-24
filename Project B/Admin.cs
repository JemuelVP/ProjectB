using System.Data.SQLite;

public class Admin
{
    public int ID { get; set; }
    public string? Name;
    public string? Password;
    public bool LoggedIn = false;

    public void Login(string? name, string? password)
    {
        string connectionString = "Data Source=movie.sqlite;Version=3;";
        //Making an connection with the database
        using SQLiteConnection connection = new SQLiteConnection(connectionString);
        connection.Open();
        string selectQuery = "SELECT * FROM Admin";
        // Create a command object to execute the SQL query
        using SQLiteCommand selectCommand = new SQLiteCommand(selectQuery, connection);
        //Execute SQL Query
        using SQLiteDataReader reader = selectCommand.ExecuteReader();
        // Check if there is data to read
        while (reader.Read() && !LoggedIn)
        {
            // Get name and password from the database
            string? storedName = reader["Name"].ToString();
            string? storedPassword = reader["Password"].ToString();
            if (name == storedName && password == storedPassword)
            {
                LoggedIn = true;
                Console.WriteLine("Succesvol ingelogd");
            }
            while (name != storedName && password != storedPassword)
            {
                Console.WriteLine("Verkeerde gegevens ingevuld probeer het opnieuw");
                Console.WriteLine("Voer je naam in:");
                name = Console.ReadLine();
                Console.WriteLine("Voer je wachtwoord in:");
                password = Console.ReadLine();
            }
        }
        connection.Close();
    }
    public static List<Film> GetAllMovies()
    {
        using DataBaseConnection db = new();
        var films = db.Movie.ToList();

        return films;
    }
}
