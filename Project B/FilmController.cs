using Spectre.Console;
public class FilmController
{
    public static List<Film> GetAllMovies()
    {
        using DataBaseConnection db = new();
        var films = db.Movie.ToList();
        return films;
    }

    public static Film? GetMovieByCategory(string categories)
    {
        using DataBaseConnection db = new();
        var filmInfo = db.Movie.FirstOrDefault(film => film.Categories == categories);
        return filmInfo;
    }
    public void Display(Film film)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        AnsiConsole.WriteLine("Film Informatie: ");
        Console.ResetColor();
        Console.ForegroundColor = ConsoleColor.Blue;
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