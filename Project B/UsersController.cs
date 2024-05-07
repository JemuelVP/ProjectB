public class UserController
{
    public bool CreateUser(string name, string password, int Isadmin = 1)
    {
        using DataBaseConnection db = new();
        // check if user already exists (case sensitive)
        Users existingUser = db.Users.FirstOrDefault(u => u.Name == name);
        if (existingUser != null)
        {
            Console.WriteLine($"Gebruikersnaam ({name}) bestaat al, kies een andere naam: ");
            return false;
        }
        // if user does not already exist, creates new user
        Users newUser = new Users
        {
            Name = name,
            Password = password,
            IsAdmin = Isadmin
        };
        db.Users.Add(newUser);
        db.SaveChanges();
        return true;
    }
}


