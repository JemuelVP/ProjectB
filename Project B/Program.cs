using Spectre.Console;

bool active = true;
DataBaseConnection db = new();

var hallsController = new CinemaHallController(db);

var CustomerMenu = new Customer();
var AdminMenu = new Admin();
while (active)
{
    string fontPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "3d.flf");
    var font = FigletFont.Load(fontPath);
    Console.Clear();
    AnsiConsole.Write(new FigletText(font, "Welkom bij").Centered().Color(Color.Blue));
    AnsiConsole.Write(new FigletText(font, "Your Eyes\n").Centered().Color(Color.Blue));

    AnsiConsole.Write(new Rule("[blue]Druk op enter om door te gaan[/]").RuleStyle("blue"));
    Console.ReadLine();
    Console.Clear();
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
    MaakEenReservatie,
    Terug
}

public enum Logout
{
    Ja,
    Nee
}
