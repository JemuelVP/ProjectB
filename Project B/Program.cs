using Spectre.Console;
Film film = new Film();
Admin admin = new Admin();
bool active = true;
Console.WriteLine("Welkom bij YourEyes");
while (active)
{
    var MainMenuOption = AnsiConsole.Prompt(new SelectionPrompt<MainMenuOptions>().Title("Bent u een admin of een klant").AddChoices(
    MainMenuOptions.Admin,
    MainMenuOptions.Customer));

    switch (MainMenuOption)
    {
        case MainMenuOptions.Admin:
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
            break;
        case MainMenuOptions.Customer:
            var ReservationMenuOption = AnsiConsole.Prompt(new SelectionPrompt<MoviesMenuOptions>().Title("Beschikbare films").AddChoices(
            MoviesMenuOptions.OverviewMovies));
            switch (ReservationMenuOption)
            {
                case MoviesMenuOptions.OverviewMovies:
                    var films = Film.GetAllMovies();
                    string[] movieInfoArray = films
                                    .Select(book => $"Title: {book.Title}, Year: {book.Year}")
                                    .ToArray();
                    var overviewMenu = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("Kies een film").AddChoices(movieInfoArray));
                    // Split the string by ','
                    string[] parts = overviewMenu.Split(',');

                    // Extract title and year
                    string title = parts[0].Split(':')[1].Trim();
                    int year = int.Parse(parts[1].Split(':')[1].Trim());
                    var selectedMovie = Film.GetMovieByTitleAndYear(title, year);
                    if (selectedMovie == null)
                    {
                        break;
                    }
                    selectedMovie.Display(selectedMovie);
                    Console.ReadKey();
                    break;
            }

            break;

    }
}

public enum MainMenuOptions
{
    Admin,
    Customer
}

public enum MoviesMenuOptions
{
    OverviewMovies,
}

public enum ReservationMenuOption
{
    MakeReservation,
    Back

}

