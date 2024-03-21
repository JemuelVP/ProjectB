DatabaseConnection.CreateDatabase();
Film film = new Film();
Admin admin = new Admin();
bool active = true;

Console.WriteLine("Welkom bij YourEyes");
while(active)
{
    Console.WriteLine("Bent u een admin kies dan 1 om in te loggen");
    Console.WriteLine("Bent u een klant kies dan 2");
    Console.WriteLine("Om te stoppen kunt u op elk moment q invoeren");
    string? activeChoice = Console.ReadLine();
    if(activeChoice.ToLower() == "q" )
    {
        active = false;
    }
    else if (Convert.ToInt32(activeChoice) == 1)
    {
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
    }
    else if(Convert.ToInt32(activeChoice) == 2)
    {
        // Film Overview
        Console.WriteLine("Om beschikbare films te bekijken kies dan 1");
        System.Console.WriteLine("Om de films die binnenkort uitkomen kies 2");
        int customersChoice = Convert.ToInt32(Console.ReadLine());
        if(customersChoice == 1)
        {
            film.DisplayMovieTitle();
            Console.Write("Voer de titel van de film in: ");
            string movieTitle = Console.ReadLine();
            if(movieTitle !="")
            {
                film.DisplayMovieInfo(movieTitle);
            }
            
            Console.WriteLine("Om een resevering te maken voer 1 in");
            
        }
        else if(customersChoice == 2)
        {

        }
    }
    else
    {
        Console.WriteLine("Verkeerde input probeer het opnieuw");
    }

}
public enum MainMenuOptions
{
    Admin,
    Customer 
}

public enum ReservationMenuOptions
{
    Apple,
    Banana,
    Orange,
    Grape
}