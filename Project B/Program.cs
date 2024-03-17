using (var db = new DataBaseConnection())
{
    Console.WriteLine("Querying for films");

    var results = db.Films.Add(new Film{
        Title = "SpiderMan",
        Year = 2024,
        Price = 120,
        Description = "THE BOMB",
        Authors = "ARAB AND VALPOORT",
        Categories = "ACTION",
        Directors = "VALPOORT AND ARAB",
        Age = 36,
        DurationInMin = 120
    });
    db.SaveChanges();
    Console.WriteLine(results);
}
