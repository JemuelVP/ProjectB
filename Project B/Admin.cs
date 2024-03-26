public class Admin
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Password { get; set; }
    public bool LoggedIn = false;

    public void Login(string? name, string? password)
    {
        using DataBaseConnection db = new();
        var admin = db.Admin.FirstOrDefault(admin =>
            admin.Name == name && admin.Password == password
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
