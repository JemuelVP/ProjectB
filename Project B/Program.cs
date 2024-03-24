using Spectre.Console;

CinemaHall1 Cinemahall1 = new CinemaHall1(1);
CinemaHall2 Cinemahall2 = new CinemaHall2(1);
CinemaHall3 Cinemahall3 = new CinemaHall3(1);
Revenue revenue = new Revenue();
Film film = new Film();
Admin admin = new Admin();
bool active = true;

Console.WriteLine("Welkom bij YourEyes");
while (active)
{
    var MainMenuOption = AnsiConsole.Prompt(
        new SelectionPrompt<MainMenuOptions>()
            .Title("Bent u een admin of een klant")
            .AddChoices(MainMenuOptions.Admin, MainMenuOptions.Klant)
    );

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
                Console.Clear();
                var adminMenuOption = AnsiConsole.Prompt(
                    new SelectionPrompt<AdminMenuOptions>()
                        .Title($"Welkom [blue]{name}[/] wat wilt u doen")
                        .AddChoices(
                            AdminMenuOptions.TotalRevenue,
                            AdminMenuOptions.TicketsPerFilm,
                            AdminMenuOptions.AddMovie
                        )
                );
                switch (adminMenuOption)
                {
                    case AdminMenuOptions.TotalRevenue:
                        double totalrev1 = revenue.TotalRevenue(Cinemahall1.Chairs);
                        double totalrev2 = revenue.TotalRevenue(Cinemahall2.Chairs);
                        double totalrev3 = revenue.TotalRevenue(Cinemahall3.Chairs);
                        Console.Clear();
                        Console.WriteLine($"Totale omzet zaal 1: ${totalrev1}");
                        Console.WriteLine($"Totale omzet zaal 2: ${totalrev2}");
                        Console.WriteLine($"Totale omzet zaal 3: ${totalrev3}");
                        break;

                    case AdminMenuOptions.TicketsPerFilm:
                        break;

                    case AdminMenuOptions.AddMovie:
                        break;
                }
            }
            break;
        case MainMenuOptions.Klant:
            var ReservationMenuOption = AnsiConsole.Prompt(
                new SelectionPrompt<MoviesMenuOptions>()
                    .Title("Beschikbare films")
                    .AddChoices(MoviesMenuOptions.OverviewMovies)
            );
            switch (ReservationMenuOption)
            {
                case MoviesMenuOptions.OverviewMovies:
                    var films = Film.GetAllMovies();
                    string[] movieInfoArray = films
                        .Select(book => $"Title: {book.Title}, Year: {book.Year}")
                        .ToArray();
                    var overviewMenu = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("Kies een film")
                            .AddChoices(movieInfoArray)
                    );
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
    Klant
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

public enum AdminMenuOptions
{
    TotalRevenue,
    TicketsPerFilm,

    AddMovie,
}
