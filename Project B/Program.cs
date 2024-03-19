DatabaseConnection.CreateDatabase();

Admin admin = new Admin();

admin.Login("admin", "123");

if (admin.LoggedIn == true) {
    Console.WriteLine("hallo");
}

else {
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