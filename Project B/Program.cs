using Spectre.Console;
Film film = new Film();
Admin admin = new Admin();
bool active = true;
Console.WriteLine("Welkom bij YourEyes");
while (active)
{
    var MainMenuOption = AnsiConsole.Prompt(new SelectionPrompt<MainMenuOptions>().Title("Bent u een admin of een klant").AddChoices(
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
                Console.WriteLine("hallo");
            }
            else
            {
                Console.WriteLine("niet ingelogd");
            }
            break;
        case MainMenuOptions.Customer:
            var ReservationMenuOption = AnsiConsole.Prompt(new SelectionPrompt<ReservationMenuOptions>().Title("Beschikbare films").AddChoices(
            ReservationMenuOptions.OverviewMovies,
            ReservationMenuOptions.SearchMovies,
            ReservationMenuOptions.SelectMovie,
            ReservationMenuOptions.Back,
            ReservationMenuOptions.Quit));
            switch (ReservationMenuOption)
            {
                case ReservationMenuOptions.OverviewMovies:
                    var films = Film.GetAllMovies();
                    string[] movieInfoArray = films
                                    .Select(book => $"Title: {book.Title}, Year: {book.Year}")
                                    .ToArray();
                    var overviewMenu = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("Kies een film").AddChoices(movieInfoArray));
                    // Split the string by ','
                    string[] parts = overviewMenu.Split(',');

                    // Extract title and year
                    string title = parts[0].Split(':')[1].Trim();
                    int year = int.Parse(parts[1].Split(':')[1].Trim());
                    var selectedMovie = Film.GetMovieByTitleAndYear(title, year);
                    if (selectedMovie == null)
                    {
                        break;
                    }
                    selectedMovie.Display(selectedMovie);
                    Console.ReadKey();
                    break;
            }

            break;

    }
}


// while(active)
// {
//     Console.WriteLine("Bent u een admin kies dan 1 om in te loggen");
//     Console.WriteLine("Bent u een klant kies dan 2");
//     Console.WriteLine("Om te stoppen kunt u op elk moment q invoeren");
//     string? activeChoice = Console.ReadLine();
//     if(activeChoice.ToLower() == "q" )
//     {
//         active = false;
//     }
//     else if (Convert.ToInt32(activeChoice) == 1)
//     {
//        
//     }
//     else if(Convert.ToInt32(activeChoice) == 2)
//     {
//         // Film Overview
//        
//     }
//     else
//     {
//         Console.WriteLine("Verkeerde input probeer het opnieuw");
//     }

// }
public enum MainMenuOptions
{
    Admin,
    Customer
}

public enum ReservationMenuOptions
{
    OverviewMovies,
    SearchMovies,
    SelectMovie,
    Back,
    Quit
}
