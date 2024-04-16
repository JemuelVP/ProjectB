using System.Data.Entity.Core.Common.CommandTrees;
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

    public static Film? GetMovieByTitle (string title)
    {
        using var db = new DataBaseConnection();
        var filmInfo = db.Movie.FirstOrDefault(film => film.Title == title);
        return filmInfo;
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