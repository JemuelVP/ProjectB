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
            AdminChoices.ReserveringZoeken,
            AdminChoices.Uitloggen
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
        var name = AnsiConsole.Prompt(new TextPrompt<string>("Voer uw gebruikersnaam in: "));
        var password = AnsiConsole.Prompt(new TextPrompt<string>("Voer uw wachtwoord in: ").Secret());
        bool adminCheck = admin.Login(name, password);
        while (adminCheck)
        {
            SetSelectedAdminOption();
            if (SelectedAdminOption == AdminChoices.Uitloggen)
            {
                var choice = AnsiConsole.Prompt(
                new SelectionPrompt<Logout>()
                    .Title("[red]Weet u zeker dat u wilt uitloggen?[/]")
                    .AddChoices(Logout.Ja, Logout.Nee)
                );
                if (choice == Logout.Ja)
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
                case AdminChoices.ReserveringZoeken:
                    ReserveringZoeken();
                    break;
            }
        }
    }
    private void FilmToevoegen()
    {
        while (true)
        {
            Console.Clear();

            string title = AskForInput<string>("Titel:");
            if (title == null) return;

            int year;
            while (true)
            {
                year = AnsiConsole.Ask<int>("Jaar uitgekomen (Voer'0' om te stoppen): ");
                if (year <= DateTime.Now.Year)
                {
                    if (year == 0) return; // Check for 0 input to stop the terminal
                    break;
                }
                else
                {
                    AnsiConsole.Markup("[red]Het uitkomst jaar kan niet in de toekomst liggen. Probeer het opnieuw.[/]\n");
                }
            }
            
            string description = AskForInput<string>("Beschrijving: ");
            if (description == null) return;

            string authors = AskForInput<string>("Acteurs: ");
            if (authors == null) return;

            string categories = AskForInput<string>("Categorieën: ");
            if (categories == null) return;

            string directors = AskForInput<string>("Regisseurs: ");
            if (directors == null) return;

            int? age = AskForInput<int>("Minimale leeftijd (Voer'0' om te stoppen): ");
            if (age == null || age == 0) return; // Check for 0 input to stop the terminal

            int? durationInMin = AskForInput<int>("Duurt in (minuten) (Voer'0' om te stoppen): ");
            if (durationInMin == null || durationInMin == 0) return; // Check for 0 input to stop the terminal

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
                age.Value,
                durationInMin.Value
            );
            break;
        }
    }

    private static T? AskForInput<T>(string prompt)
    {
        var input = AnsiConsole.Ask<string>($"{prompt} (Voer 'q' om te stoppen):");
        if (input.ToLower() == "q" || input == "0")
        {
            AnsiConsole.Markup("[yellow]Teruggegaan.[/]\n");
            return default;
        }

        try
        {
            return (T)Convert.ChangeType(input, typeof(T));
        }
        catch
        {
            AnsiConsole.Markup("[red]Ongeldige invoer. Probeer het opnieuw.[/]\n");
            return AskForInput<T>(prompt);
        }
        
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

        var MoviePlanChoices = AnsiConsole.Prompt(new SelectionPrompt<FilmPlannenChoices>().Title("[green]Wat wilt u nu doen[/]")
        .AddChoices(
        FilmPlannenChoices.TerugNaarFilmKeuzes,
        FilmPlannenChoices.DoorgaanMetPlannen,
        FilmPlannenChoices.Terug));

        if (MoviePlanChoices == FilmPlannenChoices.TerugNaarFilmKeuzes) 
        {
            FilmPlannen();
        }
        if (MoviePlanChoices == FilmPlannenChoices.DoorgaanMetPlannen)
        {
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

                var ScheduleConfirmation = AnsiConsole.Confirm("Weet u zeker dat u dit wilt plannen?");
                {
                    if (ScheduleConfirmation)
                    {
                        var schedule = new Schedule();
                            schedule.CreateFromFilm(selectedMovie, selectedHall.ID, date);
                            AnsiConsole.Markup("[green]Film is succesvol toegevoegd aan de schema.[/]");
                            AnsiConsole.WriteLine();
                            return; 
                    }
                    else
                    {   
                        AnsiConsole.Write(new Rule("[red]Film is niet toegevoegd aan de planning[/]").RuleStyle("red"));
                        return;
                    }
                }

            
            }
        if (MoviePlanChoices == FilmPlannenChoices.Terug)
        {
            Console.Clear();
            return;
        }


    }

    private void Omzet()
    {
        Console.Clear();
        var OmzetVanWat = AnsiConsole.Prompt(new SelectionPrompt<RevenueChoices>()
        .Title("[green]Van wat wilt u de omzet weten?[/]")
        .AddChoices(RevenueChoices.TotaleOmzet,
                    RevenueChoices.TotaleOmzetPerFilm,
                    RevenueChoices.Terug));
        
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

            case RevenueChoices.Terug:

            Console.Clear();
            break;



        }
    ;
        }
    
    private void ReserveringZoeken() 
    {
        using (DataBaseConnection db = new DataBaseConnection())
        {
        AnsiConsole.WriteLine("Voer een reserveringsnummer in:");
        string? userInput = Console.ReadLine();
            List<Ticket> userTickets = db.Ticket.Where(t => t.ReservationNumber == userInput).ToList();

            var ticketsPerSchedule = userTickets
                .GroupBy(t => new
                {
                    t.Movie_ID,
                    t.Schedule_ID,
                    t.DateBought
                })
                .GroupBy(g => g.Key.Schedule_ID) 
                .Select(scheduleGroup => new
                {
                    MovieID = scheduleGroup.First().Key.Movie_ID, 
                    MovieName = db
                        .Movie.FirstOrDefault(movie =>
                            movie.ID == scheduleGroup.First().Key.Movie_ID
                        )
                        ?.Title,
                    DateBought = scheduleGroup.First().Key.DateBought.ToString("yyyy-MM-dd HH:mm"),
                    ScheduleDate = db.Schedule.FirstOrDefault(s => s.ID == scheduleGroup.First().Key.Schedule_ID)?.StartDate.ToString("yyyy-MM-dd HH:mm"), 
                    TicketIDs = string.Join(", ", scheduleGroup.SelectMany(innerGroup => innerGroup.Select(t => t.ID))), 
                    TicketCount = scheduleGroup.Sum(innerGroup => innerGroup.Count()), 
                    TotalPrice = scheduleGroup.Sum(innerGroup => innerGroup.Sum(t => t.Price)) 
                })
                .ToList();

            if (ticketsPerSchedule.Any())
            {
                var table = new Table();

                table.Border = TableBorder.Rounded;
                table.AddColumn("[blue]Film[/]");
                table.AddColumn("[blue]Reserveringsnummer[/]");
                table.AddColumn("[blue]Tickets gekocht[/]");
                table.AddColumn("[blue]Totale Prijs[/]");
                table.AddColumn("[blue]Datum gekocht[/]");
                table.AddColumn("[blue]Datum van vertoning[/]");

                foreach (var scheduleInfo in ticketsPerSchedule)
                {
                    table.AddRow(
                    new Markup($"[blue]{scheduleInfo.MovieName}[/]"),
                    new Markup($"[blue]{userInput}[/]"),
                    new Markup($"[blue]{scheduleInfo.TicketCount}[/]"),
                    new Markup($"[blue]{scheduleInfo.TotalPrice.ToString()} euro[/]"), // Formatting as currency
                    new Markup($"[blue]{scheduleInfo.DateBought}[/]"),
                    new Markup($"[blue]{scheduleInfo.ScheduleDate}[/]")
                    );
                }

                // Create a bordered panel with a specific color
                var panel = new Panel(table)
                    .Header($"[bold blue]Reservering van reservingsnummer: {userInput}[/]")
                    .BorderColor(Color.Blue);

                AnsiConsole.Render(panel);
        }

        else 
        {
            AnsiConsole.Write(new Rule("[red]Geen reservering gevonden[/]").RuleStyle("red"));
        }
    }
}


    }
    public enum AdminChoices
    {
        FilmToevoegen,
        GeplandeFilms,
        FilmPlannen,
        ReserveringZoeken,
        Omzet,
        Uitloggen,
        Terug
    }


    public enum FilmPlannenChoices
    {   
        Ja,
        Nee,
        TerugNaarFilmKeuzes,
        DoorgaanMetPlannen,
        Terug
    }
    public enum RevenueChoices
    {

        TotaleOmzet,
        TotaleOmzetPerFilm,
        Terug


    }


