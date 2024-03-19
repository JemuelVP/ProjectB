DatabaseConnection.CreateDatabase();

Admin admin = new Admin();

Console.WriteLine("Voer je naam in");

string? name = Console.ReadLine();

Console.WriteLine("Voer je wachtwoord in");

string? password = Console.ReadLine();

admin.Login(name, password);

// if (admin.LoggedIn == true)
// {
//     Console.WriteLine("Succesvol ingelogd");
// }
// else if (name != name["Naam"])
// {
//     Console.WriteLine("niet ingelogd");
// }
