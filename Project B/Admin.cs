using Spectre.Console;

public class Admin
{
    private bool IsLoggedIn { get; set; }
    private AdminController adminController { get; set; }

    private Users admin { get; set; }
    private AdminChoices SelectedAdminOption { get; set; }
    private CinemaHallController hallController { get; set; }
    private DataBaseConnection db = new DataBaseConnection();

    public Admin()
    {
        db = new DataBaseConnection();
        IsLoggedIn = false;
        adminController = new AdminController();
        admin = new Users();
        hallController = new CinemaHallController(db);
    }
    private void SetSelectedAdminOption()
    {
        
        var choices = new List<AdminChoices>
        {
            AdminChoices.FilmToevoegen,
            AdminChoices.GeplandeFilms,
            AdminChoices.FilmsOverZicht,
            AdminChoices.Omzet,
            AdminChoices.UitLoggen,
            AdminChoices.Back
        };
        SelectedAdminOption = AnsiConsole.Prompt(
            new SelectionPrompt<AdminChoices>()
                .Title($"[blue]Welkom {admin.Name} wat wilt u doen?[/]")
                .AddChoices(
                    choices
                )
        );
    }
    public void Run()
    {
        var name = AnsiConsole.Prompt(new TextPrompt<string>("Voer je gebruikersnaam in: "));
        var password = AnsiConsole.Prompt(new TextPrompt<string>("Voer je wachtwoord in: ").Secret());
        admin.Login(name, password);
        while (true)
        {
            SetSelectedAdminOption();
            if (SelectedAdminOption == AdminChoices.Back)
            {
                break;
            }
            if (SelectedAdminOption == AdminChoices.UitLoggen)
            {
                            var choice = AnsiConsole.Prompt(
            new SelectionPrompt<Logout>()
                .Title("[red]Weet je zeker dat je wilt uitloggen?[/]")
                .AddChoices(Logout.Yes, Logout.No)
            );
            if (choice == Logout.Yes)
            {
                break;
            }
            }
            switch (SelectedAdminOption)
            {
                case AdminChoices.FilmToevoegen:
                    FilmToevoegen();
                    break;
                case AdminChoices.GeplandeFilms:
                    GeplandeFilms();
                    break;
                case AdminChoices.FilmsOverZicht:
                    FilmsOverZicht();
                    break;
                case AdminChoices.Omzet:
                    Omzet();
                    break;
            }
        }
    }
    private void FilmToevoegen()
    {
        string title = AnsiConsole.Ask<string>("Titel:");
        int year = AnsiConsole.Ask<int>("Jaar: ");
        string description = AnsiConsole.Ask<string>("Beschrijving: ");
        string authors = AnsiConsole.Ask<string>("Auteurs: ");
        string categories = AnsiConsole.Ask<string>("Categorieën: ");
        string directors = AnsiConsole.Ask<string>("Directeuren: ");
        int age = AnsiConsole.Ask<int>("Minimale leeftijd: ");
        int durationInMin = AnsiConsole.Ask<int>("Duurt in (minuten): ");
        AnsiConsole.Write(
            new Rule("[green]Film is toegevoegd [/]").RuleStyle("green")
        );
        ;

        // Call the AddMovie method with the input parameters
        adminController.AddMovie(
            title,
            year,
            description,
            authors,
            categories,
            directors,
            age,
            durationInMin
        );
    }
    private void GeplandeFilms()
    {
        DateTime startDate = DateTime.Now;
        DateTime endDate = DateTime.Now.AddDays(28);
        var schedules = ScheduleController.GetAvailableSchedules(
        startDate,
        endDate
        );

        // Display available films
        AnsiConsole.Write(
            new Rule(
                $"[blue]Beschikbare Films Van {startDate.Date.ToShortDateString()} Tot {endDate.Date.ToShortDateString()}:[/]"
            ).RuleStyle("blue")
        );
        var movies = schedules
            .Select(s => $"{s.Film.Title} - {s.StartDate}")
            .ToList();
        foreach (var movie in movies)
            AnsiConsole.WriteLine(movie);
    }
    private void FilmsOverZicht()
    {
        var adminOverview = AdminController.GetAllMovies();
        string[] movieTitle = adminOverview
        .Select(book => $"Titel: {book.Title}")
        .ToArray();
        var overviewMovies = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Kies een film :")
                .AddChoices(movieTitle)
        );
        // Get the total price for the selected movie
        string titlePart = overviewMovies.Replace("Titel: ", "");
        var selectedMovie = AdminController.GetMovieByTitle(titlePart);
        var ScheduleMovie = AnsiConsole.Prompt(
        new SelectionPrompt<RevenueOrScheduleMovie>()
            .Title("[green]Wat wilt u nu doen[/]")
            .AddChoices(
                RevenueOrScheduleMovie.TotaleOmzet,
                RevenueOrScheduleMovie.ScheduleMovie
            )
        );
        switch (ScheduleMovie)
        {
        case RevenueOrScheduleMovie.TotaleOmzet:
            Console.ForegroundColor = ConsoleColor.Blue;
            AnsiConsole.WriteLine(
                $"Total Tickets: {RevenueStatistics.GetTotalTicketsPerMovie(selectedMovie.ID)}"
            );
            AnsiConsole.WriteLine(
                $"Totale Omzet: {RevenueStatistics.GetTotalPricePerMovie(selectedMovie.ID)}"
            );
            break;
        case RevenueOrScheduleMovie.ScheduleMovie:
            Console.WriteLine(
                "Voer een datum in ANSI-formaat in (dd-MM-jjjj HH:mm:ss):"
            );
            string? userInput = Console.ReadLine();
            if (userInput == null)
                break;
            var date = DateTime.ParseExact(
                userInput,
                "dd-MM-yyyy HH:mm:ss",
                null,
                System.Globalization.DateTimeStyles.None
            );
            var halls = hallController.GetAllHalls();
            string[] hallNames = halls.Select(hall => hall.Name).ToArray();
            var selectedHallName = AnsiConsole.Prompt(
                new SelectionPrompt<string>().AddChoices(hallNames)
            );
            var selectedHall = hallController.GetByName(selectedHallName);
            if (selectedHall == null)
                break;
            var schedule = new Schedule();
            schedule.CreateFromFilm(selectedMovie, selectedHall.ID, date);

            Console.ReadKey();
            break;
        }
    }
    private void Omzet()
    {
        var money = new RevenueStatistics();
        money.GetTotalRevenue();
    }
    private void UitLoggen()
    {
            var choice = AnsiConsole.Prompt(
            new SelectionPrompt<Logout>()
                .Title("[red]Weet je zeker dat je wilt uitloggen?[/]")
                .AddChoices(Logout.Yes, Logout.No)
        );
        while (choice == Logout.Yes)
        {
            break;
        }
    }
    public enum AdminChoices
    {
        FilmToevoegen,
        GeplandeFilms,
        FilmsOverZicht,
        Omzet,
        UitLoggen,
        Back
    }
    public enum RevenueOrScheduleMovie
    {
        TotaleOmzet,
        ScheduleMovie
    }
}
