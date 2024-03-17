class Film
{
    public int ID { get; set; }
    public required string Title { get; set; }
    public int Year { get; set; }
    public int Price { get; set; }
    public required string Description { get; set; }
    public required string Authors { get; set; }
    public required string Categories { get; set; }
    public required string Directors { get; set; }
    public int Age { get; set; }
    public int DurationInMin { get; set; }
    

    // public Film(int id, string title, int year, int price, string description,string author, string category, string director, int age, int durationInMin)
    // {
    //     Id = id;
    //     Title = title;
    //     Year = year;
    //     Price = price;
    //     Description = description;
    //     Authors = author;
    //     Categories = category;
    //     Directors = director;
    //     Age = age;
    //     DurationInMin = durationInMin;
    // }


}