using Spectre.Console;

public class Users
{
    public int ID { get; set; }
    public string? Name { get; set; }
    public string? Password { get; set; }
    public int IsAdmin { get; set; }
    public bool LoggedIn = false;
    public bool DiscountReceived { get; set; }
    public int Visits { get; set; } = 0;

    public bool Login(string? name, string? password)
    {
        bool LoggedIn = false;

        using DataBaseConnection db = new();
        var admin = db.Users.FirstOrDefault(admin =>
            admin.Name == name && admin.Password == password && admin.IsAdmin == 0
        );
        if (admin != null)
        {
            ID = admin.ID;
            Name = admin.Name;
            Password = admin.Password;
            IsAdmin = admin.IsAdmin;
            LoggedIn = true;
            AnsiConsole.Write(new Rule("[blue]Succesvol ingelogd[/]").RuleStyle("blue"));
            return true;
            
        }
        else
        {
            Console.WriteLine("Verkeerde gegevens ingevuld, probeer het opnieuw");
            return false;
        }
    }

    public void UserLogin(string? name, string? password)
    {
        using DataBaseConnection db = new();
        var user = db.Users.FirstOrDefault(user =>
            user.Name == name && user.Password == password && user.IsAdmin == 1
        );
        if (user != null)
        {
            ID = user.ID;
            Name = user.Name;
            Password = user.Password;
            IsAdmin = user.IsAdmin;
            LoggedIn = true;
        }
        else
        {
            AnsiConsole.WriteLine("Verkeerde gegevens ingevuld, probeer het opnieuw");
            return;
        }
    }
    public bool ChangePassword(string currentPassword, string newPassword)
    {
        using DataBaseConnection db = new();
        var admin = db.Users.FirstOrDefault(u => u.ID == this.ID);
        {
            if (admin != null && admin.Password == currentPassword)
            {
                admin.Password = newPassword;
                db.SaveChanges();
                return true;
            }
        return false;
        }
    }
}
