DatabaseConnection.CreateDatabase();

Admin admin = new Admin();

admin.Login("admin", "123");

if (admin.LoggedIn == true) {
    Console.WriteLine("hallo");
}

else {
    Console.WriteLine("niet ingelogd");
}