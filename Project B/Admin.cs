using System.Data.SQLite;


public class Admin {

    public string? Name;
    public string? Password;
    public bool LoggedIn = false;

    public void Login(string name, string password) {
        string connectionString = "Data Source=movie.sqlite;Version=3;";
        using SQLiteConnection connection = new SQLiteConnection(connectionString);
        connection.Open();
        string selectQuery = "SELECT * FROM Admin";
        using SQLiteCommand selectCommand = new SQLiteCommand(selectQuery, connection);

        using SQLiteDataReader reader = selectCommand.ExecuteReader();
        if (reader.Read())
        {
            string? storedName = reader["Name"].ToString();
            string? storedPassword = reader["Password"].ToString();
            if (name == storedName && password == storedPassword)
            {
                LoggedIn = true;
                return;
            }
            Console.WriteLine("Verkeerde gegevens ingevuld probeer het opnieuw");
        }
        connection.Close();
    }
}