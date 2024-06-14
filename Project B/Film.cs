public class Film
{
    public int ID { get; set; }
    public string? Title { get; set; }
    public int Year { get; set; }
    public string? Description { get; set; }
    public string? Authors { get; set; }
    public string? Categories { get; set; }
    public string? Directors { get; set; }
    public int Age { get; set; }
    public int DurationInMin { get; set; }

    public List<Schedule> Schedules { get; set; }
}
