using System.Data.Entity;
using Spectre.Console;
public class FilmController
{
    public static List<Film> GetAllMovies()
    {
        using DataBaseConnection db = new();
        var films = db.Movie.ToList();
        return films;
    }
    public static List<Schedule> GetMovieByCategory(string cat, DateTime startDate, DateTime endDate)
    {
        using DataBaseConnection db = new();
        var schedules = db.Schedule.
        Include(s => s.Film).
        Where(s => s.Film != null && s.Film.Categories.ToLower().StartsWith(cat.ToLower())).
        Where(s => s.StartDate >= startDate && s.EndDate < endDate).
        ToList();
        foreach (var f in schedules)
        {
            db.Entry(f).Reference(f => f.Film).Load();
        }
        return schedules;
    }

    public void Display(Film film)
    {
        AnsiConsole.Write(new Rule("[green]Film Informatie[/]").RuleStyle("green"));
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($"Titel: {film.Title}");
        Console.WriteLine($"Jaar: {film.Year}");
        Console.WriteLine($"Beschrijving: {film.Description}");
        Console.WriteLine($"Acteurs: {film.Authors}");
        Console.WriteLine($"Categorieën: {film.Categories}");
        Console.WriteLine($"Regisseurs: {film.Directors}");
        Console.WriteLine($"Minimale leeftijd: {film.Age}");
        Console.WriteLine($"Duurt: {film.DurationInMin} minuten");
        Console.ResetColor();
    }

}