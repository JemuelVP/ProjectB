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
            AdminChoices.WachtwoordWijzigen,
            AdminChoices.GebruikersnaamWijzigen,
            AdminChoices.Uitloggen
        };
        SelectedAdminOption = AnsiConsole.Prompt(
            new SelectionPrompt<AdminChoices>()
                .Title($"[blue]Welkom {admin.Name} wat wilt u doen?[/]")
                .AddChoices(choices)
        );
    }

    public void Run()
    {
        var name = AnsiConsole.Prompt(
            new TextPrompt<string>("Voer uw [blue]gebruikersnaam[/] in: ")
        );
        var password = AnsiConsole.Prompt(
            new TextPrompt<string>("Voer uw [blue]wachtwoord[/] in: ").Secret()
        );
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
                    AdminController.ReserveringZoeken();
                    break;
                case AdminChoices.WachtwoordWijzigen:
                    WachtwoordVeranderen();
                    break;
                case AdminChoices.GebruikersnaamWijzigen:
                    GebruikersNaamVeranderen();
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
            if (title == null)
                return;

            int year;
            while (true)
            {
                year = AnsiConsole.Ask<int>("Jaar uitgekomen (Voer '0' om te stoppen): ");
                if (year <= DateTime.Now.Year)
                {
                    if (year == 0)
                        return; // Check for 0 input to stop the terminal
                    break;
                }
                else
                {
                    AnsiConsole.Markup(
                        "[red]Het uitkomst jaar kan niet in de toekomst liggen. Probeer het opnieuw.[/]\n"
                    );
                }
            }

            string description = AskForInput<string>("Beschrijving: ");
            if (description == null)
                return;

            string authors = AskForInput<string>("Acteurs: ");
            if (authors == null)
                return;

            string categories = AskForInput<string>("Categorieën: ");
            if (categories == null)
                return;

            string directors = AskForInput<string>("Regisseurs: ");
            if (directors == null)
                return;

            int? age = AskForInput<int>("Minimale leeftijd (Voer '0' om te stoppen): ");
            if (age == null || age == 0)
                return; // Check for 0 input to stop the terminal

            int? durationInMin = AskForInput<int>("Duurt in (minuten) (Voer '0' om te stoppen): ");
            if (durationInMin == null || durationInMin == 0)
                return; // Check for 0 input to stop the terminal

            AnsiConsole.Write(new Rule("[green]Film is toegevoegd [/]").RuleStyle("green"));

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
        var schedules = ScheduleController.GetAllSchedules(startDate, endDate);

        // Display available films
        AnsiConsole.Write(
            new Rule(
                $"[blue]Beschikbare Films Van {startDate.Date.ToShortDateString()} Tot {endDate.Date.ToShortDateString()}:[/]"
            ).RuleStyle("blue")
        );
        var movies = schedules
            .Select(s => $"{s.Film.Title} - {s.StartDate}")
            .OrderBy(title => title)
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
            .OrderBy(title => title) // Sort titles alphabetically
            .ToArray();
        var overviewMovies = AnsiConsole.Prompt(
            new SelectionPrompt<string>().Title("Kies een film :").AddChoices(movieTitle)
        );
        // Get the total price for the selected movie
        string titlePart = overviewMovies.Replace("Titel: ", "");
        var selectedMovie = AdminController.GetMovieByTitle(titlePart);

        var MoviePlanChoices = AnsiConsole.Prompt(
            new SelectionPrompt<FilmPlannenChoices>()
                .Title("[green]Wat wilt u nu doen[/]")
                .AddChoices(
                    FilmPlannenChoices.TerugNaarFilmKeuzes,
                    FilmPlannenChoices.DoorgaanMetPlannen,
                    FilmPlannenChoices.Terug
                )
        );

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
                    AnsiConsole.Markup(
                        "[red]De invoer mag niet leeg zijn. Probeer het opnieuw.[/]\n"
                    );
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
                        AnsiConsole.Markup(
                            "[red]De ingevoerde datum ligt in het verleden. Probeer het opnieuw.[/]\n"
                        );
                        continue;
                    }

                    // Check if the end time of the movie is before 11
                    var endTime = date.AddMinutes(selectedMovie.DurationInMin);
                    if (
                        endTime.TimeOfDay > new TimeSpan(23, 0, 0)
                        || endTime.Date != date.Date
                        || date.TimeOfDay < new TimeSpan(10, 0, 0)
                        || date.TimeOfDay > new TimeSpan(23, 0, 0)
                    )
                    {
                        AnsiConsole.Markup(
                            "[red]De film kan na 10:00 worden ingeplanned, en moet voor 23:00 eindigen. Probeer het opnieuw.[/]\n"
                        );
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
                AnsiConsole.Markup(
                    "[yellow]De ingevoerde datum ligt buiten de 4-weken termijn, maar de film zal worden gepland.[/]"
                );
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
                    AnsiConsole.Write(
                        new Rule("[red]Film is niet toegevoegd aan de planning[/]").RuleStyle("red")
                    );
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
        var OmzetVanWat = AnsiConsole.Prompt(
            new SelectionPrompt<RevenueChoices>()
                .Title("[green]Van wat wilt u de omzet weten?[/]")
                .AddChoices(
                    RevenueChoices.TotaleOmzet,
                    RevenueChoices.TotaleOmzetPerFilm,
                    RevenueChoices.CSVFileAanvragenVanOmzet,
                    RevenueChoices.Terug
                )
        );

        switch (OmzetVanWat)
        {
            case RevenueChoices.TotaleOmzet:

                var money = new RevenueStatistics();
                var totalTickets = RevenueStatistics.GetTotalTicketsPerSeatType();
                Console.ForegroundColor = ConsoleColor.Blue;
                foreach (var totalTicket in totalTickets)
                {
                    string stoelType = "";
                    switch (totalTicket.SeatType)
                    {
                        case 0:
                            stoelType = "Classic";
                            break;
                        case 1:
                            stoelType = "ExtraBeenRuimte";
                            break;
                        case 2:
                            stoelType = "LoveSeat";
                            break;
                    }

                    AnsiConsole.WriteLine($"SeatType: {stoelType}, Count: {totalTicket.Count}");
                }
                money.GetTotalRevenue();
                break;

            case RevenueChoices.TotaleOmzetPerFilm:

                var adminOverview = AdminController.GetAllMovies();

                string[] movieTitle = adminOverview
                    .Select(book => $"{book.Title}")
                    .OrderBy(title => title) // Sort titles alphabetically
                    .ToArray();

                var overviewMovies = AnsiConsole.Prompt(
                    new SelectionPrompt<string>().Title("Kies een film :").AddChoices(movieTitle)
                );
                // Get the total price for the selected movie
                string titlePart = overviewMovies.Replace("Titel: ", "");
                var selectedMovie = AdminController.GetMovieByTitle(titlePart);

                AnsiConsole.Write(new Rule($"{titlePart}").RuleStyle("blue"));
                Console.ForegroundColor = ConsoleColor.Blue;
                var results = RevenueStatistics.GetTotalTicketsPerSeatType(selectedMovie.ID);
                foreach (var result in results)
                {
                    string stoelType = "";
                    switch (result.SeatType)
                    {
                        case 0:
                            stoelType = "Classic";
                            break;
                        case 1:
                            stoelType = "ExtraBeenRuimte";
                            break;
                        case 2:
                            stoelType = "LoveSeat";
                            break;
                    }
                    AnsiConsole.WriteLine($"Stoel Type: {stoelType}, Totale: {result.Count}");
                }
                AnsiConsole.WriteLine(
                    $"Totale Tickets: {RevenueStatistics.GetTotalTicketsPerMovie(selectedMovie.ID)}"
                );
                AnsiConsole.WriteLine(
                    $"Totale Omzet: {RevenueStatistics.GetTotalPricePerMovie(selectedMovie.ID)}"
                );
                break;
            case RevenueChoices.CSVFileAanvragenVanOmzet:
                CSVFileAanvragen();
                break;

            case RevenueChoices.Terug:
                Console.Clear();
                break;
        }
        ;
    }

    private void CSVFileAanvragen()
    {
        using (db)
        {
            AnsiConsole.Write("Voer uw emailadres in:");
            string email = Console.ReadLine();
            AnsiConsole.Clear();
            while (!RevenueStatistics.EmailValidation(email))
            {
                AnsiConsole.WriteLine("Email adres is ongeldig probeer het opnieuw");
                AnsiConsole.Write("Voer uw emailadres in:");
                email = Console.ReadLine();
                AnsiConsole.Clear();
            }

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<AdminChoices>()
                    .Title($"[red]Weet u zeker dat dit het juiste email adres is: {email}[/]")
                    .AddChoices(AdminChoices.Ja, AdminChoices.Nee, AdminChoices.Terug)
            );

            switch (choice)
            {
                case AdminChoices.Ja:
                    RevenueStatistics.GenerateCSVFile(email);
                    AnsiConsole.WriteLine(
                        "CSV bestand is succesvol aangemaakt en naar uw email gestuurd"
                    );
                    break;
                case AdminChoices.Nee:
                    CSVFileAanvragen();
                    break;
                case AdminChoices.Terug:
                    break;
            }
        }
    }

    // checks for possibility changing password and errorhandling for the admin.
    public void WachtwoordVeranderen()
    {
        AnsiConsole.Write(new Rule("[blue] Wachtwoord wijzigen [/]"));
        string currentPassword;
        string newPassword;
        string confirmNewPassword;
        while (true)
        {
            currentPassword = AnsiConsole.Prompt(
                new TextPrompt<string>("Voer uw [blue]huidige wachtwoord[/] in: ").Secret()
            );
            if (currentPassword == admin.Password)
            {
                break;
            }
            AnsiConsole.Markup(
                "[red]Het huidige wachtwoord dat u heeft ingevoerd is onjuist. Probeer het opnieuw.[/]\n"
            );
        }
        Console.Clear();
        while (true)
        {
            newPassword = AnsiConsole.Prompt(
                new TextPrompt<string>("Voer uw [blue]nieuwe wachtwoord[/] in: ").Secret()
            );
            confirmNewPassword = AnsiConsole.Prompt(
                new TextPrompt<string>("Herhaal uw [blue]nieuwe wachtwoord[/]: ").Secret()
            );
            if (confirmNewPassword == newPassword)
            {
                break;
            }
            Console.Clear();
            AnsiConsole.Write(
                new Rule(
                    "[red] Uw nieuwe wachtwoord komt niet overheen met de herhaling, probeer het opnieuw[/]"
                )
            );
        }
        var answer = AnsiConsole.Confirm("Weet u zeker dat u uw wachtwoord wilt wijzigen?");
        if (answer)
        {
            if (admin.ChangePassword(currentPassword, newPassword))
            {
                Console.Clear();
                AnsiConsole.Markup("[green]Uw wachtwoord is succesvol gewijzigd.[/]\n");
            }
        }
        else
        {
            Console.Clear();
            AnsiConsole.Markup("[yellow]Wachtwoord wijziging geannuleerd.[/]\n");
        }
    }

    // checks for possibility and error handling of changing admins username
    public void GebruikersNaamVeranderen()
    {
        AnsiConsole.Write(new Rule("[blue] Gebruikersnaam wijzigen [/]"));
        string currentUsername;
        string newUsername;
        string confirmNewUsername;
        while (true)
        {
            currentUsername = AnsiConsole.Prompt(
                new TextPrompt<string>("Voer uw [blue]huidige gebruikersnaam[/] in: ")
            );
            if (currentUsername == admin.Name)
            {
                break;
            }
            AnsiConsole.Write(
                new Rule(
                    "[red]De huidige gebruikersnaam die u heeft ingevoerd is onjuist. Probeer het opnieuw.[/]\n"
                )
            );
        }
        Console.Clear();
        while (true)
        {
            newUsername = AnsiConsole.Prompt(
                new TextPrompt<string>("Voer uw [blue]nieuwe gebruikersnaam[/] in: ")
            );
            confirmNewUsername = AnsiConsole.Prompt(
                new TextPrompt<string>("Herhaal uw [blue]nieuwe gebruikersnaam[/]: ")
            );
            if (confirmNewUsername == newUsername)
            {
                break;
            }
            Console.Clear();
            AnsiConsole.Write(
                new Rule(
                    "[red] Uw nieuwe gebruikersnaam komt niet overheen met de herhaling, probeer het opnieuw[/]\n"
                )
            );
        }
        var answer = AnsiConsole.Confirm("Weet u zeker dat u uw gebruikersnaam wilt wijzigen?");
        if (answer)
        {
            if (admin.ChangeUsername(currentUsername, newUsername))
            {
                Console.Clear();
                AnsiConsole.Markup("[green]Uw gebruikersnaam is succesvol gewijzigd.[/]\n");
            }
        }
        else
        {
            Console.Clear();
            AnsiConsole.Markup("[yellow]Gebruikersnaam wijziging geannuleerd.[/]\n");
        }
    }
}

public enum AdminChoices
{
    Ja,
    Nee,
    FilmToevoegen,
    GeplandeFilms,
    FilmPlannen,
    ReserveringZoeken,
    Omzet,
    WachtwoordWijzigen,
    GebruikersnaamWijzigen,
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
    CSVFileAanvragenVanOmzet,
    Terug
}
