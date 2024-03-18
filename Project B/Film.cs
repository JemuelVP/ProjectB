using System.Data.SQLite;

class Film
{
    public int ID { get; set; }
    public required string Title { get; set; }
    public required int Year { get; set; }
    public required int Price { get; set; }
    public required string Description { get; set; }
    public required string Authors { get; set; }
    public required string Categories { get; set; }
    public required string Directors { get; set; }
    public required int Age { get; set; }
    public required int DurationInMin { get; set; }
    
    public void OverviewMovies(string title, int year)
    {
        string connectionString = "Data Source=movie.sqlite;Version=3;";
        using SQLiteConnection connection = new SQLiteConnection(connectionString);
        connection.Open();
        string selectQuery = $"INSERT INTO  Movie (Title, Year, Price, Description, Authors, Categories, Directors, Age, DurationInMin) VALUES (Spiderman, 2002, 200, THE BOMB, VALPOORT AND ARAB, ACTION, VALPOORT AND ARAB, 36, 120) ";
        using SQLiteCommand insertCommand = new SQLiteCommand(selectQuery, connection);

        using SQLiteDataReader reader = insertCommand.ExecuteReader();
        // if (reader.Read())
        // {
        //     string? storedTitle = reader["Title"].ToString();
        //     string? storedYear = reader["Year"].ToString();
        //     Console.WriteLine($"{storedTitle}");
        //     Console.WriteLine($"{storedYear}");

        // }
        
    }
}