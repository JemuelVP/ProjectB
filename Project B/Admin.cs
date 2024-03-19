using System.Data.SQLite;

public class Admin
{
    public string? Name;
    public string? Password;
    public bool LoggedIn = false;

    public void Login(string name, string password)
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
        if (reader.Read())
        {
            // Get name and password from the database
            string? storedName = reader["Name"].ToString();
            string? storedPassword = reader["Password"].ToString();
            if (name == storedName && password == storedPassword)
            {
                LoggedIn = true;
                Console.WriteLine("Succesvol ingelogd");
                return;
            }
            else if (name != storedName)
            {
                Console.WriteLine("Naam is verkeerd probeer het opnieuw");
            }
            else if (password != storedPassword)
            {
                System.Console.WriteLine("Wachtwoord is verkeerd probeer het opniewu");
            }
            else
            {
                Console.WriteLine("Verkeerde gegevens ingevuld probeer het opnieuw");
            }
        }
        connection.Close();
    }
}
