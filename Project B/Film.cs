using System.Data.SQLite;

public class Film
{
    public int ID { get; set; }
    public string? Title { get; set; }
    public int Year { get; set; }
    public int Price { get; set; }
    public string? Description { get; set; }
    public string? Authors { get; set; }
    public string? Categories { get; set; }
    public string? Directors { get; set; }
    public int Age { get; set; }
    public int DurationInMin { get; set; }

    public static List<Film> GetAllMovies()
    {
        using DataBaseConnection db = new();
        var films = db.Movie.ToList();

        return films;
    }

    public static Film? GetMovieByTitleAndYear(string title, int year)
    {
        using DataBaseConnection db = new();
        var filmInfo = db.Movie.FirstOrDefault(film => film.Title == title && film.Year == year);
        return filmInfo;
    }

    public void Display(Film film)
    {
        Console.WriteLine("Movie Information:");
        Console.WriteLine($"Title: {film.Title}");
        Console.WriteLine($"Year: {film.Year}");
        Console.WriteLine($"Price: {film.Price}");
        Console.WriteLine($"Description: {film.Description}");
        Console.WriteLine($"Authors: {film.Authors}");
        Console.WriteLine($"Categories: {film.Categories}");
        Console.WriteLine($"Directors: {film.Directors}");
        Console.WriteLine($"Age: {film.Age}");
        Console.WriteLine($"Duration (minutes): {film.DurationInMin}");
    }
}
