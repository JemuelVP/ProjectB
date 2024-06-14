using Spectre.Console;

public class AdminController
{
    public static List<Film> GetAllMovies()
    {
        using DataBaseConnection db = new();
        var films = db.Movie.ToList();

        return films;
    }

    public static Film? GetMovieByTitle(string title)
    {
        using DataBaseConnection db = new();
        var filmInfo = db.Movie.FirstOrDefault(film => film.Title == title);
        return filmInfo;
    }

    public void AddMovie(
        string title,
        int year,
        string description,
        string authors,
        string categories,
        string directors,
        int age,
        int durationInMin
    )
    {
        using DataBaseConnection db = new();
        var newMovie = new Film
        {
            Title = title,
            Year = year,
            Description = description,
            Authors = authors,
            Categories = categories,
            Directors = directors,
            Age = age,
            DurationInMin = durationInMin
        };

        db.Movie.Add(newMovie);
        db.SaveChanges();
    }
    public static void ReserveringZoeken()
    {
        using (DataBaseConnection db = new DataBaseConnection())
        {
            AnsiConsole.WriteLine("Voer een reserveringsnummer in:");
            string? userInput = Console.ReadLine();
            List<Ticket> userTickets = db
                .Ticket.Where(t => t.ReservationNumber == userInput)
                .ToList();

            var ticketsPerSchedule = userTickets
                .GroupBy(t => new
                {
                    t.Movie_ID,
                    t.Schedule_ID,
                    t.DateBought
                })
                .GroupBy(g => g.Key.Schedule_ID)
                .Select(scheduleGroup => new
                {
                    MovieID = scheduleGroup.First().Key.Movie_ID,
                    MovieName = db
                        .Movie.FirstOrDefault(movie =>
                            movie.ID == scheduleGroup.First().Key.Movie_ID
                        )
                        ?.Title,
                    DateBought = scheduleGroup.First().Key.DateBought.ToString("yyyy-MM-dd HH:mm"),
                    ScheduleDate = db
                        .Schedule.FirstOrDefault(s => s.ID == scheduleGroup.First().Key.Schedule_ID)
                        ?.StartDate.ToString("yyyy-MM-dd HH:mm"),
                    TicketCount = scheduleGroup.Sum(innerGroup => innerGroup.Count()),
                    TotalPrice = scheduleGroup.Sum(innerGroup => innerGroup.Sum(t => t.Price))
                })
                .ToList();

            if (ticketsPerSchedule.Any())
            {
                var table = new Table();

                table.Border = TableBorder.Rounded;
                table.AddColumn("[blue]Film[/]");
                table.AddColumn("[blue]Reserveringsnummer[/]");
                table.AddColumn("[blue]Tickets gekocht[/]");
                table.AddColumn("[blue]Totale Prijs[/]");
                table.AddColumn("[blue]Datum gekocht[/]");
                table.AddColumn("[blue]Datum van vertoning[/]");

                foreach (var scheduleInfo in ticketsPerSchedule)
                {
                    table.AddRow(
                        new Markup($"[blue]{scheduleInfo.MovieName}[/]"),
                        new Markup($"[blue]{userInput}[/]"),
                        new Markup($"[blue]{scheduleInfo.TicketCount}[/]"),
                        new Markup($"[blue]{scheduleInfo.TotalPrice.ToString()} euro[/]"), // Formatting as currency
                        new Markup($"[blue]{scheduleInfo.DateBought}[/]"),
                        new Markup($"[blue]{scheduleInfo.ScheduleDate}[/]")
                    );
                }

                // Create a bordered panel with a specific color
                var panel = new Panel(table)
                    .Header($"[bold blue]Reservering van reserveringsnummer: {userInput}[/]")
                    .BorderColor(Color.Blue);

                AnsiConsole.Render(panel);
            }
            else
            {
                AnsiConsole.Write(new Rule("[red]Geen reservering gevonden[/]").RuleStyle("red"));
            }
        }
    }
}
