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
    public void AddMovie(string title, int year, string description, string authors, string categories, string directors, int age, int durationInMin)
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

}