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
        SelectedCustomerOption = AnsiConsole.Prompt(
            new SelectionPrompt<CustomerChoices>()
                .Title($"[blue]Welkom {User.Name} wat wilt u doen?[/]")
                .AddChoices(
                    CustomerChoices.FilmZoeken,
                    CustomerChoices.Films,
                    CustomerChoices.SeeUserStats,
                    CustomerChoices.LogOut,
                    CustomerChoices.Back
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
        var passWord = AnsiConsole.Prompt(
            new TextPrompt<string>("Voer een wachtwoord in: ").Secret()
        );
        userController.CreateUser(username, passWord);
        AnsiConsole.Write(
            new Rule("[blue]Gebruiker is aangemaakt[/]").RuleStyle("blue")
        );
    }
    private static void FilmZoeken()
    {
        DateTime startDate = DateTime.Now;
        DateTime endDate = DateTime.Now.AddDays(28);
        string MovieSearch = AnsiConsole.Prompt(new TextPrompt<string>("Zoek film categorie: "));
        List<Schedule> searchSchedules = FilmController.GetMovieByCategory(MovieSearch, startDate, endDate);
        Console.WriteLine(searchSchedules.Count);

        if (searchSchedules.Count > 0)
        {
            var choices = searchSchedules.Select(s => $"{s.Film.Title} - {s.StartDate.ToString("dd-MM-yyyy HH:mm")}").ToList();

            var selectedMovieIndex = AnsiConsole.Prompt(
                new SelectionPrompt<string>().Title("Kies een film").AddChoices(choices)
            );

        }
        else
        {
            Console.WriteLine($"No movies found for category '{MovieSearch}'.");
            Console.ReadLine();
        }
    }
    private void Inloggen()
    {
        var customerName = AnsiConsole.Prompt(new TextPrompt<string>("Voer je gebruikersnaam in: "));
        var customerPassword = AnsiConsole.Prompt(new TextPrompt<string>("Voer je wachtwoord in: ").Secret());
        User.UserLogin(customerName, customerPassword);
        AnsiConsole.Write(new Rule("[blue]Succesvol ingelogd[/]").RuleStyle("blue"));

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
        Film film = new();
        DateTime startDate = DateTime.Now;
        DateTime endDate = DateTime.Now.AddDays(28);
        var schedules = ScheduleController.GetAvailableSchedules(startDate, endDate);
        ReservationMenuOption selectedReservationOption = ReservationMenuOption.MakeReservation; // Start with MakeReservation option
        while (selectedReservationOption != ReservationMenuOption.Back)
        {
            // Display available films
            AnsiConsole.Write(
                new Rule(
                    $"[blue]Beschikbare Films Van {startDate.Date.ToShortDateString()} Tot {endDate.Date.ToShortDateString()}:[/]"
                ).RuleStyle("blue")
            );
            var choices = schedules
                .Select(s => $"{s.Film.Title} - {s.StartDate.ToString("dd-MM-yyyy HH:mm")}")
                .ToList();

            var selectedMovieIndex = AnsiConsole.Prompt(
                new SelectionPrompt<string>().Title("Kies een film").AddChoices(choices)
            );

            // Get the selected schedule based on the selected movie
            var selectedSchedule = schedules[choices.IndexOf(selectedMovieIndex)];
            film = selectedSchedule.Film;
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
                AnsiConsole.Write(new Rule("[blue]Stoel Kosten[/]").RuleStyle("blue"));

                // Display seat type options and prompt the user to choose
                var seatType = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Kies u stoel")
                        .AddChoices("Classic", "Loveseat", "Extrabeenruimte")
                );
                int seatTypeInt = -1;
                Console.WriteLine(seatType);
                switch (seatType)
                {
                    case "Classic":
                        seatTypeInt = 0;
                        break;
                    case "Loveseat":
                        seatTypeInt = 1;
                        break;
                    case "Extrabeenruimte":
                        seatTypeInt = 2;
                        break;
                }

                var availableSeats = ChairController.GetAvailableSeats(
                    selectedSchedule.ID,
                    selectedSchedule.Hall_ID,
                    seatTypeInt
                );

                // Prompt the user to enter a seat number
                var seatNumbers = AnsiConsole.Prompt(
                    new MultiSelectionPrompt<int>()
                        .Title("Select seats")
                        .AddChoices(availableSeats.Select(s => s.Position))
                );

                var selectedSeats = availableSeats.Where(s => seatNumbers.Contains(s.Position));
                if (selectedSeats == null)
                {
                    Console.WriteLine("Seat not found, try again!");
                    selectedReservationOption = ReservationMenuOption.Back;
                    continue;
                }
                // Create ticket with selected schedule, user name, seat type, and seat number

                var ticket = new Ticket();
                // je moet hier of een zaal object meegeven of het aantal stoelen
                foreach (var seat in selectedSeats)
                {
                    var seatPrice = ticket.GetSeatPrice(seatTypeInt, seat.Position, selectedSchedule);
                    AnsiConsole.Write(new Rule($"[blue]Prijs: {seatPrice} euro[/]").RuleStyle("blue"));
                }

                var confirmPurchase = AnsiConsole.Confirm(
                    "Wil je de bestelling bevestigen?",
                    false
                );
                if (confirmPurchase)
                {
                    double totalPrice = 0;
                    foreach (var seat in selectedSeats)
                    {
                        // Create the ticket
                        double finalPrice = ticket.CreateTicket(
                            selectedSchedule,
                            seat.ID,
                            film.ID,
                            ticket.GetSeatPrice(seatTypeInt, seat.Position, selectedSchedule)
                        );
                        totalPrice += finalPrice;
                        Ticket.DisplayTicketDetails(ticket, seat, finalPrice);
                    }
                    AnsiConsole.Write(new Rule($"[blue]Totaal bedrag: {totalPrice} euro[/]").RuleStyle("blue"));
                }
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
