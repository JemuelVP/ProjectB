public class UserController
{
    public void CreateUser(string name, string password, int Isadmin = 1)
    {
        using DataBaseConnection db = new();
        var newUser = new Users
        {
            Name = name,
            Password = password,
            IsAdmin = Isadmin
        };
        db.Users.Add(newUser);
        db.SaveChanges();
    }
}
