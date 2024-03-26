using Spectre.Console;
Film film = new Film();
FilmController filmController = new FilmController();
Admin admin = new Admin();
AdminController adminController = new AdminController();
bool active = true;
DateTime startDate = DateTime.Now;
DateTime endDate = DateTime.Now.AddDays(28);
var nowDateTime = DateTime.Now.Date.ToShortDateString();
var endDateTime = DateTime.Now.AddDays(28).Date.ToShortDateString();
using DataBaseConnection db = new();
var hallsController = new CinemaHallController(db);
AnsiConsole.Write(new Rule($"Welkom bij YourEyes").RuleStyle("red"));
while (active)
{
    var MainMenuOption = AnsiConsole.Prompt(new SelectionPrompt<MainMenuOptions>().Title("[green]Bent u een admin of een klant[/]").AddChoices(
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
                var AdminOptions = AnsiConsole.Prompt(new SelectionPrompt<AdminChoices>().Title("[green]Wat wilt u nu doen[/]").AddChoices(
                AdminChoices.AddMovie,
                AdminChoices.MoviesOverView,
                AdminChoices.Revenue));
                switch (AdminOptions)
                {
                    case AdminChoices.AddMovie:
                        string title = AnsiConsole.Ask<string>("Titel:");
                        int year = AnsiConsole.Ask<int>("Jaar: ");
                        int price = AnsiConsole.Ask<int>("Prijs: ");
                        string description = AnsiConsole.Ask<string>("Beschrijving: ");
                        string authors = AnsiConsole.Ask<string>("Auteurs: ");
                        string categories = AnsiConsole.Ask<string>("Categorieën: ");
                        string directors = AnsiConsole.Ask<string>("Directeuren: ");
                        int age = AnsiConsole.Ask<int>("Minimale leeftijd: ");
                        int durationInMin = AnsiConsole.Ask<int>("Duurt in (minuten): ");
                        AnsiConsole.Write(new Rule("[green]Film is toegevoegd [/]").RuleStyle("green")); ;

                        // Call the AddMovie method with the input parameters
                        adminController.AddMovie(title, year, price, description, authors, categories, directors, age, durationInMin);
                        break;
                    case AdminChoices.MoviesOverView:
                        var adminOverview = AdminController.GetAllMovies();

                        string[] movieTitle = adminOverview
                            .Select(book => $"Titel: {book.Title}")
                            .ToArray();
                        var overviewMovies = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("Kies een film :").AddChoices(movieTitle));
                        // Get the total price for the selected movie
                        string titlePart = overviewMovies.Replace("Titel: ", "");
                        var selectedMovie = AdminController.GetMovieByTitle(titlePart);
                        var ScheduleMovie = AnsiConsole.Prompt(new SelectionPrompt<RevenueOrScheduleMovie>().Title("[green]Wat wilt u nu doen[/]").AddChoices(
                        RevenueOrScheduleMovie.SeeRevenue,
                        RevenueOrScheduleMovie.ScheduleMovie));
                        switch (ScheduleMovie)
                        {
                            case RevenueOrScheduleMovie.SeeRevenue:
                                Console.WriteLine($"Total Revenue: {RevenueStatistics.GetTotalPricePerMovie(selectedMovie.ID)}");
                                break;
                            case RevenueOrScheduleMovie.ScheduleMovie:
                                Console.WriteLine("Voer een datum in ANSI-formaat in (dd-MM-jjjj HH:mm:ss):");
                                string? userInput = Console.ReadLine();
                                if (userInput == null) break;
                                var date = DateTime.ParseExact(userInput, "dd-MM-yyyy HH:mm:ss", null, System.Globalization.DateTimeStyles.None);
                                var halls = hallsController.GetAllHalls();
                                string[] hallNames = halls
                                .Select(hall => hall.Name)
                                .ToArray();
                                var selectedHallName = AnsiConsole.Prompt(new SelectionPrompt<string>().AddChoices(hallNames));
                                var selectedHall = hallsController.GetByName(selectedHallName);
                                if (selectedHall == null) break;
                                var schedule = new Schedule();
                                schedule.CreateFromFilm(selectedMovie, selectedHall.ID, date);

                                Console.ReadKey();
                                break;
                        }
                        break;
                    case AdminChoices.Revenue:
                        var money = new RevenueStatistics();
                        money.GetTotalRevenue();
                        break;
                }
                break;
            }
            else
            {
                Console.WriteLine("niet ingelogd");
            }
            break;
        case MainMenuOptions.Customer:
            ReservationMenuOption option = ReservationMenuOption.MakeReservation; // Start with MakeReservation option
            while (option != ReservationMenuOption.Back)
            {
                var schedules = ScheduleController.GetAvailableSchedules(startDate, endDate);

                // Display available films
                AnsiConsole.Write(new Rule($"[blue]Beschikbare Films Van {nowDateTime} Tot {endDateTime}:[/]").RuleStyle("blue"));
                var choices = schedules.Select(s => $"{s.Film.Title} - {s.StartDate}").ToList();
                var selectedMovieIndex = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("Kies een film").AddChoices(choices));

                // Get the selected schedule based on the selected movie
                var selectedSchedule = schedules[choices.IndexOf(selectedMovieIndex)];
                film = selectedSchedule.Film;
                // Display the details of the selected movie
                filmController.Display(film);

                // Prompt the user to make a reservation or go back
                option = AnsiConsole.Prompt(new SelectionPrompt<ReservationMenuOption>().Title("Maak een keuze").AddChoices(ReservationMenuOption.MakeReservation, ReservationMenuOption.Back));

                if (option == ReservationMenuOption.MakeReservation)
                {
                    var userName = AnsiConsole.Prompt(new TextPrompt<string>("Voer u naam in: "));
                    AnsiConsole.Write(new Rule("[red]Stoel Kosten[/]").RuleStyle("red"));
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    AnsiConsole.WriteLine("Classic: 20");
                    AnsiConsole.WriteLine("Loveseat: 25");
                    AnsiConsole.WriteLine("Extrabeenruimte: 25");
                    // Display seat type options and prompt the user to choose
                    var seatType = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("Kies u stoel")
                                                        .AddChoices("Classic ", "Loveseat", "Extrabeenruimte"));

                    // Prompt the user to enter a seat number
                    var seatNumber = AnsiConsole.Prompt(new TextPrompt<int>("Voer een getal in tussen 1 en 10"));

                    // Create ticket with selected schedule, user name, seat type, and seat number
                    var ticket = new Ticket();
                    ticket.GetSeatPrice(seatType, seatNumber); // Calculate ticket price based on seat type and number
                    ticket.CreateTicket(selectedSchedule, film.ID, userName, seatType, seatNumber);
                }
            }
            break;
    }
}

public enum MainMenuOptions
{
    Admin,
    Customer
}

public enum RevenueOrScheduleMovie
{
    SeeRevenue,
    ScheduleMovie

}

public enum AdminChoices
{
    AddMovie,
    MoviesOverView,
    Revenue
}

public enum ReservationMenuOption
{
    MakeReservation,
    Back

}

