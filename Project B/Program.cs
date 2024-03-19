DatabaseConnection.CreateDatabase();

Admin admin = new Admin();

admin.Login("admin", "123");

if (admin.LoggedIn == true) {
    Console.WriteLine("hallo");
}

else {
    Console.WriteLine("niet ingelogd");
}


// test Revenue class

// CinemaHall1 Cinemahall1 = new CinemaHall1(1);
// List<int> idsToSell = new List<int> {1,2,3,4,5};
// foreach (Chair chair in Cinemahall1.Chairs)
// {
//     if (idsToSell.Contains(chair.Id))
//     {
//         chair.Sold = true;
//     }
// }

// Revenue revenue = new Revenue();
// double totalrev = revenue.TotalRevenue(Cinemahall1.Chairs);
// Console.WriteLine(totalrev);