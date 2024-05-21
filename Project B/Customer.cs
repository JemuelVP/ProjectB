using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
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
        var choices = new List<CustomerChoices>{
            CustomerChoices.AccountAanmaken,
            CustomerChoices.Inloggen,
            CustomerChoices.FilmZoeken,
            CustomerChoices.Films,
            CustomerChoices.Back
            };
        if (User.LoggedIn)
        {
            choices.Remove(CustomerChoices.AccountAanmaken);
            choices.Remove(CustomerChoices.Inloggen);
            choices.Remove(CustomerChoices.FilmZoeken);
            choices.Remove(CustomerChoices.Films);
            choices.Remove(CustomerChoices.Back);
            AnsiConsole.Write(new Rule("[blue]Succesvol ingelogd[/]").RuleStyle("blue"));
            choices.Add(CustomerChoices.FilmZoeken);
            choices.Add(CustomerChoices.Films);
            choices.Add(CustomerChoices.SeeUserStats);
            choices.Add(CustomerChoices.LogOut);
            choices.Add(CustomerChoices.Back);
        }
        SelectedCustomerOption = AnsiConsole.Prompt(
            new SelectionPrompt<CustomerChoices>()
                .Title($"[blue]Welkom {User.Name} wat wilt u doen?[/]")
                .AddChoices(
                    choices
                )
        );
    }
    public void Run()
    {
        while (true)
        {
            SetSelectedCustomerOption();
            if (SelectedCustomerOption == CustomerChoices.Back)
            {
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
                case CustomerChoices.SeeUserStats:
                    Ticket.SeeUserStats(User.ID);
                    break;
                case CustomerChoices.LogOut:
                    Uitloggen();
                    break;
            }
        }

    }

    private void AccountAanmaken()
    {
        if (IsLoggedIn) return;
        UserController userController = new();
        var username = AnsiConsole.Prompt(
            new TextPrompt<string>("Voer een gebruikersnaam in: ")
        );
        var password = AnsiConsole.Prompt(
            new TextPrompt<string>("Voer een wachtwoord in: ").Secret()
        );
        // checks if user was succesfully created or not
        bool userCreated = userController.CreateUser(username, password);
        if (userCreated)
        {
            // only shows message if username is not a duplicate
            AnsiConsole.Write(
                new Rule("[blue]Gebruiker is aangemaakt[/]").RuleStyle("blue")
            );
        }
    }
    private void FilmZoeken()
    {
        DateTime startDate = DateTime.Now;
        DateTime endDate = DateTime.Now.AddDays(28);
        string MovieSearch = AnsiConsole.Prompt(new TextPrompt<string>("Zoek film categorie: "));
        List<Schedule> searchSchedules = FilmController.GetMovieByCategory(MovieSearch, startDate, endDate);
        Console.WriteLine(searchSchedules.Count);

        if (searchSchedules.Count > 0)
        {
            FilmTicketKopen();

        }
        else
        {
            Console.WriteLine($"Geen films gevonden voor '{MovieSearch}'.");
            Console.ReadLine();
        }
    }
    private void Inloggen()
    {
        var customerName = AnsiConsole.Prompt(new TextPrompt<string>("Voer je gebruikersnaam in: "));
        var customerPassword = AnsiConsole.Prompt(new TextPrompt<string>("Voer je wachtwoord in: ").Secret());
        User.UserLogin(customerName, customerPassword);

    }
    private void Uitloggen()
    {
        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<Logout>()
                .Title("[red]Weet je zeker dat je wilt uitloggen?[/]")
                .AddChoices(Logout.Yes, Logout.No)
        );
        if (choice == Logout.Yes)
        {
            User = new Users();
        }
    }
    private void FilmsBekijken()
    {

        DateTime startDate = DateTime.Now;
        DateTime endDate = DateTime.Now.AddDays(28);
        ReservationMenuOption selectedReservationOption = ReservationMenuOption.MakeReservation; // Start with MakeReservation option
        while (selectedReservationOption != ReservationMenuOption.Back)
        {
            // Display available films
            AnsiConsole.Write(
                new Rule(
                    $"[blue]Beschikbare Films Van {startDate.Date.ToShortDateString()} Tot {endDate.Date.ToShortDateString()}:[/]"
                ).RuleStyle("blue")
            );
            FilmTicketKopen();
            break;
        }
    }

    private void FilmTicketKopen()
    {
        ReservationMenuOption selectedReservationOption = ReservationMenuOption.MakeReservation; // Start with MakeReservation option
        DateTime startDate = DateTime.Now;
        DateTime endDate = DateTime.Now.AddDays(28);
        var schedules = ScheduleController.GetTitlesForScheduledMovies(startDate, endDate);
        var AllSchedules = ScheduleController.GetAvailableSchedules(startDate,endDate);
        Film film = new();
        var choices = schedules
                .Select(s => $"{s.Film.Title}")
                .ToList();

        var selectedMovieIndex = AnsiConsole.Prompt(
            new SelectionPrompt<string>().Title("Kies een film").AddChoices(choices)
        );

            // Get the selected schedule based on the selected movie
            var selectedSchedule = schedules[choices.IndexOf(selectedMovieIndex)];
            // start code here
            var newChoices = AllSchedules.Where(s => s.Film.Title == selectedSchedule.Film.Title)
                                    .Select(s =>
                                    {

                                        if (s.SoldOut)
                                            return $"{s.Film.Title} - {s.StartDate.ToString("dd-MM-yyyy HH:mm")} - uitverkocht";
                                        else
                                            return $"{s.Film.Title} - {s.StartDate.ToString("dd-MM-yyyy HH:mm")}";
                                    })
                                    .ToList();
            var newSelectedSchedules = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("Kies een datum").AddChoices(newChoices));

            selectedSchedule = schedules[choices.IndexOf(selectedMovieIndex)];

            film = selectedSchedule.Film;
            // end code here
            // Display the details of the selected movie
            filmController.Display(film);

        // Prompt the user to make a reservation or go back
        selectedReservationOption = AnsiConsole.Prompt(
            new SelectionPrompt<ReservationMenuOption>()
                .Title("Maak een keuze")
                .AddChoices(
                    ReservationMenuOption.MakeReservation,
                    ReservationMenuOption.Back
                )
        );

        if (selectedReservationOption == ReservationMenuOption.MakeReservation)
        {
            var userName = AnsiConsole.Prompt(new TextPrompt<string>("Voer u naam in: "));
            var age = AnsiConsole.Prompt(new TextPrompt<int>("Voer uw leeftijd in: "));
            var ticketAge = new Ticket();
            ticketAge.CheckAge(film, age); // checks age against age movie
            // AnsiConsole.Write(new Rule("[blue]Stoel Kosten[/]").RuleStyle("blue"));
            var targetHall = new CinemaHallController(new DataBaseConnection()).GetByID(selectedSchedule.Hall_ID);
            if (targetHall == null) return;
            // var availableSeats = ChairController.GetAvailableSeats(
            //     selectedSchedule.ID,
            //     selectedSchedule.Hall_ID
            // );
            while (true)
            {
                int width = -1;
                int height = -1;
                switch(targetHall.Size)
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


                // Draw the canvas
                Console.Clear();
                List<Tuple<int,int>> listSelectedChairs = new();
                canvas.Draw(selectedSchedule.ID, targetHall.Size,width,height, listSelectedChairs);
                var selectedChairs = new List<int>();
                var isSelectingChair = true;
                // Main loop to handle cursor movement
                while (isSelectingChair)
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
                                var chair = db.Chair.FirstOrDefault(c => c.Row == canvas.cursorY && c.Column == canvas.cursorX && c.CinemaHallID == targetHall.ID);
                                if (chair != null)
                                {
                                    var getTicket = db.Ticket.FirstOrDefault(t => t.Schedule_ID == selectedSchedule.ID && t.Chair_ID == chair.ID);
                                    if (getTicket != null)
                                    {
                                        break; // Skip to the next iteration
                                    }
                                    if (selectedChairs.Contains(chair.ID))
                                    {
                                        selectedChairs.Remove(chair.ID);
                                        listSelectedChairs.Remove(Tuple.Create(canvas.cursorY, canvas.cursorX));
                                        break;
                                    }
                                    selectedChairs.Add(chair.ID);
                                    listSelectedChairs.Add(Tuple.Create(canvas.cursorY, canvas.cursorX));
                                }
                            }
                            break;
                        case ConsoleKey.Enter:
                            isSelectingChair = false;
                            Console.WriteLine(selectedChairs);
                            break;
                    }
                    // Redraw canvas with updated cursor position
                    canvas.Draw(selectedSchedule.ID, targetHall.Size, width, height, listSelectedChairs);
                    // AnsiConsole.WriteLine($"Aantal geslecteerde stoelen: {selectedChairs.Count}");
                }
                Console.Clear();
                int seatType = -1;
                using (DataBaseConnection db = new DataBaseConnection())
                {
                    var chairType = db.Chair.FirstOrDefault(c => c.Row == canvas.cursorY && c.Column == canvas.cursorX && c.CinemaHallID == 1);
                    if (chairType?.SeatType == 0)
                    {
                        seatType = 0;
                    }
                    else if (chairType?.SeatType == 1)
                    {
                        seatType = 1;
                    }
                    if (chairType?.SeatType == 2)
                    {
                        seatType = 2;
                    }
                }
                var ticket = new Ticket();
                // je moet hier of een zaal object meegeven of het aantal stoelen
                foreach (var chairId in selectedChairs)
                {
                    var db = new DataBaseConnection();
                    var chair = db.Chair.FirstOrDefault(c => c.ID == chairId);
                    if (chair != null)
                    {
                        int chairX = chair.Column;
                        int chairY = chair.Row;

                        // Calculate the seat price using the actual chair coordinates
                        var seatPrice = ticket.GetSeatPrice(seatType, chairY, chairX, selectedSchedule);
                        AnsiConsole.Write(new Rule($"[blue]Prijs: {seatPrice} euro[/]").RuleStyle("blue"));
                    }
                }
                var confirmPurchase = AnsiConsole.Confirm("Wil je de bestelling bevestigen?");
                if (confirmPurchase)
                {
                    var db = new DataBaseConnection();
                    var totalPrice = 0.0;
                    foreach (var chairId in selectedChairs)
                    {
                        // Retrieve chair object by ID from the database
                        var chair = db.Chair.FirstOrDefault(c => c.ID == chairId);

                        if (chair != null)
                        {
                            // Assuming chair has properties for X and Y coordinates
                            int chairX = chair.Column;
                            int chairY = chair.Row;

                            // Use chairX and chairY in your logic to calculate the final price
                            var finalPrice = ticket.CreateTicket(selectedSchedule, chairId, film.ID, ticket.GetSeatPrice(seatType, chairY, chairX, selectedSchedule), User.ID);

                            // Increment total price
                            totalPrice += finalPrice;

                            // Display ticket details for the current chair
                            Ticket.DisplayTicketDetails(seatType, chairY, chairX, finalPrice);
                        }
                    }
                }
                break;
            }
        }

    }

    public enum CustomerChoices
    {
        AccountAanmaken,
        FilmZoeken,

        Inloggen,
        Films,

        SeeUserStats,
        LogOut,
        Back,
    }

}
