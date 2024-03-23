DatabaseConnection.CreateDatabase();


CinemaHall3 hall = new CinemaHall3(3);
hall.AddHall("Zaal 3", 500);
Console.WriteLine("Data inserted successfully.");

//checkt of de schedule overview correct update naar de databse
hall.OverviewSchedules(1, 1, 1, 0, new DateTime(2024,12,12), new DateTime(2024,12,13));
hall.OverviewSchedules(2, 1, 3, 0, new DateTime(2019,12,3), new DateTime(2020,12,30));

hall.OverviewTickets(1,"sude", 1, 2, "loveseat", 30, 1);
hall.OverviewTickets(2, "twee", 1, 3, "extralegroom", 27.5,1 );
hall.OverviewTickets(3, "drie", 2, 4, "loveseat", 30, 2);

hall.CountTicketsPerFilm();


Admin admin = new Admin();
Film film = new Film();


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


// Film Overview
film.DisplayMovieTitle();

Console.Write("Enter the title of the movie: ");
string? movieTitle = Console.ReadLine();

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
