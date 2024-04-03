using Spectre.Console;
public class FilmController
{
    public static List<Film> GetAllMovies()
    {
        using DataBaseConnection db = new();
        var films = db.Movie.ToList();
        return films;
    }

    
    public static List<Film> GetMovieByCategory(string category)
    {
        using DataBaseConnection db = new();
        var MovieInfo = db.Movie.Where(Movie => Movie.Categories.ToLower() == category).ToList();
        return MovieInfo;

    }

    public void Display(Film film)
    {
        AnsiConsole.Write(new Rule("[green]Film Informatie[/]").RuleStyle("green"));
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"Titel: {film.Title}");
        Console.WriteLine($"Jaar: {film.Year}");
        Console.WriteLine($"Beschrijving: {film.Description}");
        Console.WriteLine($"Auteurs: {film.Authors}");
        Console.WriteLine($"Categorieën: {film.Categories}");
        Console.WriteLine($"Directeuren: {film.Directors}");
        Console.WriteLine($"Minimale leeftijd: {film.Age}");
        Console.WriteLine($"Duurt: {film.DurationInMin} minuten");
        Console.ResetColor();
    }

}