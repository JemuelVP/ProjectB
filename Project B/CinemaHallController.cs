class CinemaHallController
{
    private DataBaseConnection db;

    public CinemaHallController(DataBaseConnection db)
    {
        this.db = db;
    }

    public List<CinemaHall> GetAllHalls()
    {
        return this.db.Hall.ToList();
    }

    public CinemaHall? GetByName(string name)
    {
        return this.db.Hall.First(hall => hall.Name == name);
    }
}