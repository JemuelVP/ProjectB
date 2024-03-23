using System.Data.SQLite;
class Film
{
    public int ID { get; set; }
    public string? Title { get; set; }
    public int Year { get; set; }
    public int Price { get; set; }
    public string? Description { get; set; }
    public string? Authors { get; set; }
    public string? Categories { get; set; }
    public string? Directors { get; set; }
    public int Age { get; set; }
    public int DurationInMin { get; set; }

    //Method to insert movie data into the database
    public void OverviewMovies(string title, int year, int price, string description, string authors, string categories, string directors, int age, int durationInMin)
    {
        // Connection string to connect to the SQLite database
        string connectionString = "Data Source=movie.sqlite;Version=3;";

        // Using statement ensures the connection is closed properly after use
        using (SQLiteConnection connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            // SQL query to insert data into the Movie table
            string insertQuery = "INSERT INTO Movie (Title, Year, Price, Description, Authors, Categories, Directors, Age, DurationInMin) " +
                                 "VALUES (@Title, @Year, @Price, @Description, @Authors, @Categories, @Directors, @Age, @DurationInMin)";
            // Using the '@' symbol before the "" so you can include the '' without escaping them
            // Using statement ensures the command is disposed properly after use
            using (SQLiteCommand insertCommand = new SQLiteCommand(insertQuery, connection))
            {
                insertCommand.Parameters.AddWithValue("@Title", title);
                insertCommand.Parameters.AddWithValue("@Year", year);
                insertCommand.Parameters.AddWithValue("@Price", price);
                insertCommand.Parameters.AddWithValue("@Description", description);
                insertCommand.Parameters.AddWithValue("@Authors", authors);
                insertCommand.Parameters.AddWithValue("@Categories", categories);
                insertCommand.Parameters.AddWithValue("@Directors", directors);
                insertCommand.Parameters.AddWithValue("@Age", age);
                insertCommand.Parameters.AddWithValue("@DurationInMin", durationInMin);

                // Execute the command to insert data into the database
                insertCommand.ExecuteNonQuery();
            }
        }
    }
    
    public void DisplayMovieTitle()
    {
        string connectionString = "Data Source=movie.sqlite;Version=3;";

        using (SQLiteConnection connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            string selectQuery = "SELECT Title, Year From Movie";
            using (SQLiteCommand selectCommand = new SQLiteCommand(selectQuery, connection))
            {
                using (SQLiteDataReader reader = selectCommand.ExecuteReader())
                {
                    if(reader.HasRows)
                    {
                        Console.WriteLine("Movies: ");
                        while (reader.Read())
                        {
                            string? title = reader["Title"].ToString();
                            int year = Convert.ToInt32(reader["Year"]);
                            System.Console.WriteLine($"{title}, {year}");
                        }
                    }
                }
            }
        }
    }

    public void DisplayMovieInfo(string? movieTitle)
    {
        string connectionString = "Data Source=movie.sqlite;Version=3;";

        using (SQLiteConnection connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            string selectQuery = "SELECT * From Movie WHERE Title = @Title ";
            using (SQLiteCommand selectCommand = new SQLiteCommand(selectQuery, connection))
            {
                // Add parameter for the movie title
                selectCommand.Parameters.AddWithValue("@Title", movieTitle);
                
                using (SQLiteDataReader reader = selectCommand.ExecuteReader())
                {
                    if(reader.Read())
                    {
                        Console.WriteLine("Movie Information:");
                        Console.WriteLine($"Title: {reader["Title"]}");
                        Console.WriteLine($"Year: {reader["Year"]}");
                        Console.WriteLine($"Price: {reader["Price"]}");
                        Console.WriteLine($"Description: {reader["Description"]}");
                        Console.WriteLine($"Authors: {reader["Authors"]}");
                        Console.WriteLine($"Categories: {reader["Categories"]}");
                        Console.WriteLine($"Directors: {reader["Directors"]}");
                        Console.WriteLine($"Age: {reader["Age"]}");
                        Console.WriteLine($"Duration (minutes): {reader["DurationInMin"]}");
                    }
                    else
                    {
                        Console.WriteLine("Movie not found.");
                    }
                }
            }
        }
    }
}