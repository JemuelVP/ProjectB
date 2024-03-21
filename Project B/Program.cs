DatabaseConnection.CreateDatabase();

Admin admin = new Admin();

Console.WriteLine("Voer je naam in");


string? name = Console.ReadLine();

Console.WriteLine("Voer je wachtwoord in");

string? password = Console.ReadLine();

admin.Login(name, password);


if (admin.LoggedIn == true)
{
    Console.WriteLine("hallo");
}
else
{
    Console.WriteLine("niet ingelogd");
}

Film film = new Film();

film.DisplayMovieTitle();

Console.Write("Enter the title of the movie: ");
string movieTitle = Console.ReadLine();

film.DisplayMovieInfo(movieTitle);

// Call the OverviewMovies method to insert movie data into the database
// film.OverviewMovies("Batman", 2002, 200, "THE BOMB", "VALPOORT AND ARAB", "ACTION", "VALPOORT AND ARAB", 36, 120);

// // Print a message indicating successful data insertion
// Console.WriteLine("Data inserted successfully.");
// test Revenue class

// CinemaHall1 Cinemahall1 = new CinemaHall1(1);
// List<int> idsToSell = new List<int> {1,2,3,4,5};
// foreach (Chair chair in Cinemahall1.Chairs)
// {
//     if (idsToSell.Contains(chair.Id))
//     {
//         chair.Sold = true;
//     }
// }

// Revenue revenue = new Revenue();
// double totalrev = revenue.TotalRevenue(Cinemahall1.Chairs);
// Console.WriteLine(totalrev);


