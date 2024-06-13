using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Net.NetworkInformation;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;

public class Customer
{
    private bool IsLoggedIn { get; set; }
    private FilmController filmController { get; set; }

    private Users User { get; set; }
    private CustomerChoices SelectedCustomerOption { get; set; }

    public Customer()
    {
        IsLoggedIn = false;
        filmController = new FilmController();
        User = new Users();
    }

    private void SetSelectedCustomerOption()
    {
        var choices = new List<CustomerChoices>
        {
            CustomerChoices.AccountAanmaken,
            CustomerChoices.Inloggen,
            CustomerChoices.FilmZoeken,
            CustomerChoices.Films,
            CustomerChoices.Terug
        };
        if (User.LoggedIn)
        {
            choices.Remove(CustomerChoices.AccountAanmaken);
            choices.Remove(CustomerChoices.Inloggen);
            choices.Remove(CustomerChoices.FilmZoeken);
            choices.Remove(CustomerChoices.Films);
            choices.Remove(CustomerChoices.Terug);
            choices.Add(CustomerChoices.FilmZoeken);
            choices.Add(CustomerChoices.Films);
            choices.Add(CustomerChoices.ToonMijnReserveringen);
            choices.Add(CustomerChoices.FilmSuggesties);
            choices.Add(CustomerChoices.Uitloggen);
        }
        SelectedCustomerOption = AnsiConsole.Prompt(
            new SelectionPrompt<CustomerChoices>()
                .Title($"[blue]Welkom {User.Name} wat wilt u doen?[/]")
                .AddChoices(choices)
        );
    }

    public void Run()
    {
        while (true)
        {
            SetSelectedCustomerOption();
            if (SelectedCustomerOption == CustomerChoices.Terug)
            {
                Console.Clear();
                break;
            }
            switch (SelectedCustomerOption)
            {
                case CustomerChoices.AccountAanmaken:
                    AccountAanmaken();
                    break;
                case CustomerChoices.Inloggen:
                    Inloggen();
                    break;
                case CustomerChoices.FilmZoeken:
                    FilmZoeken();
                    break;
                case CustomerChoices.Films:
                    FilmsBekijken();
                    break;
                case CustomerChoices.ToonMijnReserveringen:
                    Ticket.SeeUserStats(User.ID);
                    break;
                case CustomerChoices.FilmSuggesties:
                    GetSchedulesForSuggestion(User.ID);
                    break;
                case CustomerChoices.Uitloggen:
                    Uitloggen();
                    break;
            }
        }
    }

    private void AccountAanmaken()
    {
        if (IsLoggedIn)
            return;
        UserController userController = new();
        var username = AnsiConsole.Prompt(
            new TextPrompt<string>("Voer een [blue]gebruikersnaam[/] in: ")
        );
        var password = AnsiConsole.Prompt(
            new TextPrompt<string>("Voer een [blue]wachtwoord[/] in: ").Secret()
        );
        // checks if user was succesfully created or not
        bool userCreated = userController.CreateUser(username, password);
        if (userCreated)
        {
            // only shows message if username is not a duplicate
            AnsiConsole.Write(new Rule("[blue]Gebruiker is aangemaakt[/]").RuleStyle("blue"));
        }
    }

    private void Inloggen()
    {
        Console.Clear();
        var customerName = AnsiConsole.Prompt(
            new TextPrompt<string>("Voer uw [blue]gebruikersnaam[/] in: ")
        );
        var customerPassword = AnsiConsole.Prompt(
            new TextPrompt<string>("Voer uw [blue]wachtwoord[/] in: ").Secret()
        );
        User.UserLogin(customerName, customerPassword);
    }

    private void Uitloggen()
    {
        Console.Clear();
        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<Logout>()
                .Title("[red]Weet u zeker dat u wilt uitloggen?[/]")
                .AddChoices(Logout.Ja, Logout.Nee)
        );
        if (choice == Logout.Nee)
        {
            User = new Users();
        }
    }

    // Method to get the list of unique movie categories from the database
    private List<string> GetScheduledMovieCategories(DateTime startDate, DateTime endDate)
    {
        using (var db = new DataBaseConnection()) // Assuming DataBaseConnection is your DbContext
        {
            var categories = db
                .Schedule.Where(s => s.StartDate >= startDate && s.EndDate <= endDate)
                .Select(s => s.Movie_ID) // Select movie IDs
                .Distinct() // Get distinct movie IDs
                .Select(movieId =>
                    db.Movie.Where(m => m.ID == movieId).Select(m => m.Categories).FirstOrDefault()
                ) // Get category for each movie ID
                .ToList();
            return categories;
        }
    }

    // Method to search for films by category
    private void FilmZoeken()
    {
        Console.Clear();
        DateTime startDate = DateTime.Now;
        DateTime endDate = DateTime.Now.AddDays(28);

        // Get the list of unique movie categories from the database
        List<string> categories = GetScheduledMovieCategories(startDate, endDate);

        // Display the list of categories for the user to choose from
        string selectedCategory = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Selecteer een film categorie:")
                .PageSize(10)
                .MoreChoicesText("[grey](Scroll naar beneden om meer categorieën te zien)[/]")
                .AddChoices(categories)
        );

        List<Schedule> searchSchedules = FilmController.GetMovieByCategory(
            selectedCategory,
            startDate,
            endDate
        );

        if (searchSchedules.Count > 0)
        {
            FilmTicketKopen(searchSchedules);
        }
        else
        {
            Console.WriteLine($"Geen films gevonden voor '{selectedCategory}'.");
        }
    }

    // Method to view available films and make reservations
    private void FilmsBekijken()
    {
        Console.Clear();
        DateTime startDate = DateTime.Now;
        DateTime endDate = DateTime.Now.AddDays(28);
        ReservationMenuOption selectedReservationOption = ReservationMenuOption.MaakEenReservatie; // Start with MakeReservation option
        while (selectedReservationOption != ReservationMenuOption.Terug)
        {
            // Display available films
            AnsiConsole.Write(
                new Rule(
                    $"[blue]Beschikbare Films Van {startDate.Date.ToShortDateString()} Tot {endDate.Date.ToShortDateString()}:[/]"
                ).RuleStyle("blue")
            );

            // Get all schedules for the given date range
            var allSchedules = ScheduleController.GetAvailableSchedules(startDate, endDate);

            // Display the titles of available films
            var filmTitles = allSchedules.Select(s => s.Film.Title).Distinct().ToList();
            var selectedMovie = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Kies een film")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Scroll naar beneden om meer films te zien)[/]")
                    .AddChoices(filmTitles)
            );

            // Filter schedules by the selected movie
            var selectedMovieSchedules = allSchedules
                .Where(s => s.Film.Title == selectedMovie)
                .ToList();

            // Proceed to buy tickets for the selected movie
            if (selectedMovieSchedules.Count > 0)
            {
                FilmTicketKopen(selectedMovieSchedules);
            }
            else
            {
                Console.WriteLine($"Geen beschikbare vertoningen gevonden voor '{selectedMovie}'.");
            }

            // Break the loop after processing the selected movie
            break;
        }
    }

    // Method to purchase tickets for a selected schedule
    public void FilmTicketKopen(List<Schedule> schedules)
    {
        while (true)
        {
            var choices = schedules.Select(s => $"{s.Film.Title}").Distinct().ToList();

            var selectedMovieTitle = AnsiConsole.Prompt(
                new SelectionPrompt<string>().Title("Kies een film").AddChoices(choices)
            );

            // Get the schedules for the selected movie
            var selectedSchedules = schedules
                .Where(s => s.Film.Title == selectedMovieTitle)
                .ToList();
            var newChoices = selectedSchedules
                .Select(s =>
                {
                    if (s.SoldOut)
                        return $"{s.Film.Title} - {s.StartDate.ToString("dd-MM-yyyy HH:mm")} - uitverkocht";
                    else
                        return $"{s.Film.Title} - {s.StartDate.ToString("dd-MM-yyyy HH:mm")}";
                })
                .ToList();

            var selectedScheduleOption = AnsiConsole.Prompt(
                new SelectionPrompt<string>().Title("Kies een datum").AddChoices(newChoices)
            );

            var selectedSchedule = selectedSchedules[newChoices.IndexOf(selectedScheduleOption)];
            var film = selectedSchedule.Film;

            // Display the details of the selected movie
            var filmController = new FilmController();
            filmController.Display(film);

            // Prompt the user to make a reservation or go back
            var selectedReservationOption = AnsiConsole.Prompt(
                new SelectionPrompt<ReservationMenuOption>()
                    .Title("Maak een keuze alstublieft")
                    .AddChoices(
                        ReservationMenuOption.MaakEenReservatie,
                        ReservationMenuOption.Terug
                    )
            );

            if (selectedReservationOption == ReservationMenuOption.MaakEenReservatie)
            {
                if (!User.LoggedIn)
                {
                    var userName = AnsiConsole.Prompt(new TextPrompt<string>("Voer uw naam in: "));
                }

                var age = AnsiConsole.Prompt(new TextPrompt<int>("Voer uw leeftijd in: "));
                var ticketAge = new Ticket();
                ticketAge.CheckAge(film, age); // Checks age against age restriction of the movie

                var targetHall = new CinemaHallController(new DataBaseConnection()).GetByID(
                    selectedSchedule.Hall_ID
                );
                if (targetHall == null)
                    return;

                while (true)
                {
                    int width = -1;
                    int height = -1;
                    switch (targetHall.Size)
                    {
                        case "Small":
                            width = 12;
                            height = 14;
                            break;
                        case "Medium":
                            width = 18;
                            height = 19;
                            break;
                        case "Large":
                            width = 30;
                            height = 20;
                            break;
                    }
                    ConsoleCanvas canvas = new(width, height);

                    Console.CursorVisible = false; // Hide the cursor
                    List<Tuple<int, int>> listSelectedChairs = new();
                    canvas.Draw(
                        selectedSchedule.ID,
                        targetHall.Size,
                        width,
                        height,
                        listSelectedChairs,
                        film.Title
                    );
                    var selectedChairs = new List<int>();
                    var isSelectingChair = true;
                    // Main loop to handle cursor movement
                    do
                    {
                        ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                        switch (keyInfo.Key)
                        {
                            case ConsoleKey.UpArrow:
                                canvas.MoveCursor(0, -1);
                                break;
                            case ConsoleKey.DownArrow:
                                canvas.MoveCursor(0, 1);
                                break;
                            case ConsoleKey.LeftArrow:
                                canvas.MoveCursor(-1, 0);
                                break;
                            case ConsoleKey.RightArrow:
                                canvas.MoveCursor(1, 0);
                                break;
                            case ConsoleKey.Spacebar:
                                using (DataBaseConnection db = new DataBaseConnection())
                                {
                                    var chair = db.Chair.FirstOrDefault(c =>
                                        c.Row == canvas.cursorY
                                        && c.Column == canvas.cursorX
                                        && c.CinemaHallID == targetHall.ID
                                    );
                                    if (chair != null)
                                    {
                                        var getTicket = db.Ticket.FirstOrDefault(t =>
                                            t.Schedule_ID == selectedSchedule.ID
                                            && t.Chair_ID == chair.ID
                                        );
                                        if (getTicket != null)
                                        {
                                            break; // Skip to the next iteration
                                        }
                                        if (selectedChairs.Contains(chair.ID))
                                        {
                                            selectedChairs.Remove(chair.ID);
                                            listSelectedChairs.Remove(
                                                Tuple.Create(canvas.cursorY, canvas.cursorX)
                                            );
                                            break;
                                        }
                                        selectedChairs.Add(chair.ID);
                                        listSelectedChairs.Add(
                                            Tuple.Create(canvas.cursorY, canvas.cursorX)
                                        );
                                    }
                                }
                                break;
                            case ConsoleKey.Enter:
                                if (selectedChairs.Count == 0)
                                {
                                    // If enter is pressed without chair selection, confirm cancellation
                                    var confirmCancellation = AnsiConsole.Confirm(
                                        "U heeft geen stoelen geselecteerd, wilt u de reservering annuleren?"
                                    );
                                    if (confirmCancellation)
                                    {
                                        AnsiConsole.Write(
                                            new Rule(
                                                "[red]Uw reservering is geannuleerd[/]"
                                            ).RuleStyle("red")
                                        );
                                        return;
                                    }
                                    else
                                    {
                                        AnsiConsole.Write(
                                            new Rule(
                                                "[blue]Bestelling is niet geannuleerd. U kunt nu verder met het selecteren van uw stoelen[/]"
                                            ).RuleStyle("blue")
                                        );
                                        canvas.Draw(
                                            selectedSchedule.ID,
                                            targetHall.Size,
                                            width,
                                            height,
                                            listSelectedChairs,
                                            film.Title
                                        );
                                    }
                                    break;
                                }
                                else
                                {
                                    // Continue if chairs are selected
                                    isSelectingChair = false;
                                    Console.WriteLine(selectedChairs);
                                }
                                break;
                        }
                        // Redraw canvas with updated cursor position
                        canvas.Draw(
                            selectedSchedule.ID,
                            targetHall.Size,
                            width,
                            height,
                            listSelectedChairs,
                            film.Title
                        );
                    } while (isSelectingChair);
                    Console.Clear();

                    var ticket = new Ticket();
                    bool qualifyForDiscount = User.LoggedIn && Ticket.UserTicketDiscount(User.ID);

                    double totalPriceKassabon = 0;

                    int ClassicSeatsCounter = 0;
                    int LoveSeatsCounter = 0;
                    int ExtraLegroomCounter = 0;

                    double totalClassicseatPrice = 0;
                    double totalLoveseatPrice = 0;
                    double totalExtraLegroomPrice = 0;
                    foreach (var chairId in selectedChairs)
                    {
                        var db = new DataBaseConnection();
                        var chair = db.Chair.FirstOrDefault(c => c.ID == chairId);
                        int seatType = -1;
                        if (chair?.SeatType == 0)
                        {
                            seatType = 0;
                        }
                        else if (chair?.SeatType == 1)
                        {
                            seatType = 1;
                        }
                        if (chair?.SeatType == 2)
                        {
                            seatType = 2;
                        }
                        if (chair != null)
                        {
                            int chairX = chair.Column;
                            int chairY = chair.Row;

                            // Calculate the seat price using the actual chair coordinates
                            var seatPrice = ticket.GetSeatPrice(
                                seatType,
                                chairY,
                                chairX,
                                selectedSchedule,
                                qualifyForDiscount
                            );

                            if (seatType == 0)
                            {
                                ClassicSeatsCounter++;
                                totalClassicseatPrice += seatPrice;
                            }
                            if (seatType == 1)
                            {
                                ExtraLegroomCounter++;
                                totalExtraLegroomPrice += seatPrice;
                            }
                            if (seatType == 2)
                            {
                                LoveSeatsCounter++;
                                totalLoveseatPrice += seatPrice;
                            }

                            totalPriceKassabon += seatPrice;
                        }
                    }

                    AnsiConsole.Write(new Rule("[blue]Prijs overzicht[/]\n").LeftJustified());
                    Ticket.DecideSeatTypeName(
                        ClassicSeatsCounter,
                        LoveSeatsCounter,
                        ExtraLegroomCounter,
                        totalClassicseatPrice,
                        totalLoveseatPrice,
                        totalExtraLegroomPrice
                    );

                    //empty line to divide the total price and the seats
                    var rule = new Rule();
                    AnsiConsole.Write(rule);

                    //chekcs if the user is qualified for discount if true it calculates the discount and decreases the total price
                    if (qualifyForDiscount)
                    {
                        double discountPercentage = 0.10;
                        double discountAmount = totalPriceKassabon * discountPercentage;
                        totalPriceKassabon -= discountAmount;
                        AnsiConsole.Markup(
                            $"[blue]Korting:[/][white]{discountAmount}[/] [blue]euro[/]\n"
                        );
                    }
                    AnsiConsole.Markup(
                        $"[blue]Totale Prijs:[/][white]{totalPriceKassabon}[/] [blue]euro[/]\n"
                    );

                    var confirmPurchase = AnsiConsole.Confirm("Wilt u de reservering bevestigen?");
                    if (confirmPurchase)
                    {
                        var db = new DataBaseConnection();
                        var currentUser = db.Users.FirstOrDefault(u => u.ID == User.ID);
                        if (currentUser != null)
                        {
                            currentUser.Visits += 1;
                        }
                        var totalPrice = 0.0;
                        string reservationNumber = Ticket.GenerateReservationNumber();
                        db.SaveChanges();
                        foreach (var chairId in selectedChairs)
                        {
                            // Retrieve chair object by ID from the database
                            var chair = db.Chair.FirstOrDefault(c => c.ID == chairId);
                            int seatType = -1;

                            if (chair?.SeatType == 0)
                            {
                                seatType = 0;
                            }
                            else if (chair?.SeatType == 1)
                            {
                                seatType = 1;
                            }
                            if (chair?.SeatType == 2)
                            {
                                seatType = 2;
                            }
                            if (chair != null)
                            {
                                // Assuming chair has properties for X and Y coordinates
                                int chairX = chair.Column;
                                int chairY = chair.Row;
                                // Use chairX and chairY in your logic to calculate the final price
                                var finalPrice = ticket.CreateTicket(
                                    selectedSchedule,
                                    chairId,
                                    film.ID,
                                    ticket.GetSeatPrice(
                                        seatType,
                                        chairY,
                                        chairX,
                                        selectedSchedule,
                                        qualifyForDiscount
                                    ),
                                    User.ID,
                                    reservationNumber
                                );

                                // Increment total price
                                totalPrice += finalPrice;

                                // Display ticket details for the current chair
                                Ticket.DisplayTicketDetails(
                                    seatType,
                                    chairY,
                                    chairX,
                                    finalPrice,
                                    reservationNumber
                                );
                            }
                        }
                    }
                    else
                    {
                        AnsiConsole.WriteLine("Uw bestelling is geannuleerd.");
                    }
                    break;
                }
                break;
            }
            else if (selectedReservationOption == ReservationMenuOption.Terug)
            {
                Console.Clear();
                break;
            }
        }
    }

    public void GetSchedulesForSuggestion(int userID)
    {
        using (DataBaseConnection db = new DataBaseConnection())
        {
            string mostWatchedGenre = Ticket.FiveTicketsGenre(userID);
            List<int> movieIds = db
                .Movie.Where(m => m.Categories == mostWatchedGenre)
                .Select(m => m.ID)
                .ToList(); // all movies with category

            List<Schedule> schedules = db
                .Schedule.Where(s => movieIds.Contains(s.Movie_ID))
                .Include(s => s.Film)
                .ToList(); // all movies with category in schedule

            List<int> userTickets = db
                .Ticket.Where(t => t.User_ID == userID)
                .Select(t => t.Movie_ID)
                .ToList(); // movies the user has tickets to

            List<int> suggestedMovieIds = movieIds.Except(userTickets).ToList();

            if (suggestedMovieIds.Any())
            {
                AnsiConsole.Markup("[blue]Suggesties gebaseerd op uw favoriete genre: [/]\n");
                List<string> suggestedMovies = new List<string>();
                foreach (int movieId in suggestedMovieIds)
                {
                    var movie = db.Movie.FirstOrDefault(m => m.ID == movieId);
                    if (movie != null)
                    {
                        suggestedMovies.Add(movie.Title);
                    }
                }
                if (suggestedMovies.Any())
                {
                    string selectedMovie = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title(
                                "[blue]U kunt een suggestie selecteren om deze te reserveren [/]\n"
                            )
                            .AddChoices(suggestedMovies)
                    );

                    var selectedMovieId = db
                        .Movie.FirstOrDefault(m => m.Title == selectedMovie)
                        ?.ID;
                    var selectSchedule = schedules
                        .Where(s => s.Movie_ID == selectedMovieId)
                        .ToList();
                    if (selectSchedule.Any())
                    {
                        this.FilmTicketKopen(selectSchedule);
                    }
                    else
                    {
                        AnsiConsole.Markup(
                            $"[red]Op het moment zijn er geen vertoningen gevonden voor '{selectedMovie}'.[/]"
                        );
                    }
                }
            }
            else
            {
                AnsiConsole.Markup(
                    "[red]Helaas hebben wij nog niet genoeg informatie om je suggesties te geven[/] \n"
                );
                AnsiConsole.Markup("[red]Probeer het in de toekomst opnieuw[/]\n");
            }
        }
    }

    public enum CustomerChoices
    {
        AccountAanmaken,
        FilmZoeken,

        Inloggen,
        Films,
        ToonMijnReserveringen,
        Uitloggen,
        Terug,
        FilmSuggesties
    }
}
