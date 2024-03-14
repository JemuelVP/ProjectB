class Film
{
    public string Title;
    public int Year;
    public int Price;
    public string Description;
    public List<string> Author;
    public List<string> Category;
    public List<string> Director;
    public int Age;
    public int DurationInMin;


    public Film(string title, int year, int price, string description,List<string> author, List<string> category, List<string> director, int age, int durationInMin)
    {
        Title = title;
        Year = year;
        Price = price;
        Description = description;
        Author = author;
        Category = category;
        Director = director;
        Age = age;
        DurationInMin = durationInMin;
    }

}