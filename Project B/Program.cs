using Spectre.Console;
bool active = true;
DataBaseConnection db = new();

// ChairController chairController = new ChairController();
// chairController.AddChairs();
// Console.WriteLine("Chairs added to the database.");
var hallsController = new CinemaHallController(db);
AnsiConsole.Write(new Rule($"Welkom bij YourEyes").RuleStyle("blue"));
var CustomerMenu = new Customer();
var AdminMenu = new Admin();
while (active)
{
    var MainMenuOption = AnsiConsole.Prompt(
        new SelectionPrompt<MainMenuOptions>()
            .Title("[blue]Bent u een admin of een klant[/]")
            .AddChoices(MainMenuOptions.Admin, MainMenuOptions.Klant)
    );
    switch (MainMenuOption)
    {
        case MainMenuOptions.Admin:
            AdminMenu.Run();
            break;
        case MainMenuOptions.Klant:
            CustomerMenu.Run();
            break;
    }
}

public enum MainMenuOptions
{
    Admin,
    Klant
}

public enum ReservationMenuOption
{
    
    MakeReservation,
    Back
}

public enum Logout
{
    Yes,
    No
}
