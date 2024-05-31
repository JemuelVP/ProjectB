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
            AdminChoices.FilmPlannen,
            AdminChoices.Omzet,
            AdminChoices.UitLoggen
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
        bool adminCheck = admin.Login(name, password);
        while (adminCheck)
        {
            SetSelectedAdminOption();
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
                case AdminChoices.FilmPlannen:
                    FilmPlannen();
                    break;
                case AdminChoices.Omzet:
                    Omzet();
                    break;
            }
        }
    }
    private void FilmToevoegen()
    {
        Console.Clear();
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
        Console.Clear();
        DateTime startDate = DateTime.Now;
        DateTime endDate = DateTime.Now.AddDays(28);
        var schedules = ScheduleController.GetAllSchedules(
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
    private void FilmPlannen()
    {
        Console.Clear();
        var adminOverview = AdminController.GetAllMovies();
        string[] movieTitle = adminOverview
        .Select(book => $"{book.Title}")
        .OrderBy(title => title)  // Sort titles alphabetically
        .ToArray();
        var overviewMovies = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Kies een film :")
                .AddChoices(movieTitle)
        );
        // Get the total price for the selected movie
        string titlePart = overviewMovies.Replace("Titel: ", "");
        var selectedMovie = AdminController.GetMovieByTitle(titlePart);
    
        
        DateTime date;
        while (true)
        {
            Console.WriteLine("Voer een datum in ANSI-formaat in (dd-MM-jjjj HH:mm): ");
            string? userInput = Console.ReadLine();
            if (userInput == null)
            {
                AnsiConsole.Markup("[red]De invoer mag niet leeg zijn. Probeer het opnieuw.[/]\n");
                continue;
            }

            try
            {
                date = DateTime.ParseExact(
                    userInput,
                    "dd-MM-yyyy HH:mm",
                    null,
                    System.Globalization.DateTimeStyles.None
                );
                
                if (date < DateTime.Now)
                {
                    AnsiConsole.Markup("[red]De ingevoerde datum ligt in het verleden. Probeer het opnieuw.[/]\n");
                    continue;
                }
                
                // Check if the end time of the movie is before 11
                var endTime = date.AddMinutes(selectedMovie.DurationInMin);
                if (endTime.TimeOfDay > new TimeSpan(23, 0, 0) || endTime.Date != date.Date ||
                date.TimeOfDay  < new TimeSpan(10, 0, 0) || date.TimeOfDay > new TimeSpan(23, 0, 0))
                {
                    AnsiConsole.Markup("[red]De eindtijd van de film moet voor 23:00 en op dezelfde dag zijn. Probeer het opnieuw.[/]\n");
                    continue;
                }
                break;
            }
            catch (FormatException)
            {
                AnsiConsole.Markup("[red]Ongeldig datumformaat. Probeer het opnieuw.[/]\n");
            }
            
            }

            // Check if the entered date is beyond 4 weeks from now
            var maxScheduleDate = DateTime.Now.AddDays(28);
            if (date > maxScheduleDate)
            {
                AnsiConsole.Markup("[yellow]De ingevoerde datum ligt buiten de 4-weken termijn, maar de film zal worden gepland.[/]");
            }
            AnsiConsole.WriteLine();
            var halls = hallController.GetAllHalls();
            string[] hallNames = halls.Select(hall => hall.Name).ToArray();
            var selectedHallName = AnsiConsole.Prompt(
                new SelectionPrompt<string>().AddChoices(hallNames)
            );
            var selectedHall = hallController.GetByName(selectedHallName);
            if (selectedHall == null)
                return;
            var schedule = new Schedule();
            schedule.CreateFromFilm(selectedMovie, selectedHall.ID, date);
            AnsiConsole.Markup("[green]Film is succesvol toegevoegd aan de schema.[/]");
            AnsiConsole.WriteLine();
            return;
        
    }

    private void Omzet()
    {
        Console.Clear();
        var OmzetVanWat = AnsiConsole.Prompt(new SelectionPrompt<RevenueChoices>()
        .Title("[green]Van wat wilt u de omzet weten?[/]")
        .AddChoices(RevenueChoices.TotaleOmzet,
                    RevenueChoices.TotaleOmzetPerFilm,
                    RevenueChoices.Back));
        
        switch (OmzetVanWat)
        {

            case RevenueChoices.TotaleOmzet:
            
            var money = new RevenueStatistics();
            money.GetTotalRevenue();
            break; 

            case RevenueChoices.TotaleOmzetPerFilm:

            var adminOverview = AdminController.GetAllMovies();
            
            string[] movieTitle = adminOverview
                                .Select(book => $"{book.Title}")
                                .OrderBy(title => title)  // Sort titles alphabetically
                                .ToArray();

            var overviewMovies = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Kies een film :")
                    .AddChoices(movieTitle)
            );
            // Get the total price for the selected movie
            string titlePart = overviewMovies.Replace("Titel: ", "");
            var selectedMovie = AdminController.GetMovieByTitle(titlePart);
            
            Console.ForegroundColor = ConsoleColor.Blue;
            AnsiConsole.WriteLine(
                $"Total Tickets: {RevenueStatistics.GetTotalTicketsPerMovie(selectedMovie.ID)}"
            );
            AnsiConsole.WriteLine(
                $"Totale Omzet: {RevenueStatistics.GetTotalPricePerMovie(selectedMovie.ID)}"
            );
            break;

            case RevenueChoices.Back:

            Console.Clear();
            break;



        }
    ;
           
        }


    }
    public enum AdminChoices
    {
        FilmToevoegen,
        GeplandeFilms,
        FilmPlannen,
        Omzet,
        UitLoggen,
        Back
    }
    public enum RevenueChoices
    {
      
        TotaleOmzet,

        TotaleOmzetPerFilm,

        Back


    }


