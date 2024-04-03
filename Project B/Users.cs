public class Users
{
    public int ID { get; set; }
    public string? Name { get; set; }
    public string? Password { get; set; }
    public int IsAdmin { get; set; }


    public bool LoggedIn = false;

    public void Login(string? name, string? password)
    {
        using DataBaseConnection db = new();
        var admin = db.Users.FirstOrDefault(admin =>
            admin.Name == name && admin.Password == password && admin.IsAdmin == 0
        );
        if (admin != null)
        {
            LoggedIn = true;
        }
        else
        {
            Console.WriteLine("Verkeerde gegevens ingevuld, probeer het opnieuw");
        }
    }
}
