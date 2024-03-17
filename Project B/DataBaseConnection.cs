using Microsoft.EntityFrameworkCore;

class DataBaseConnection : DbContext
{
    public DbSet<Film> Films { get; set; }
    public string DbPath {get;}

    public DataBaseConnection()
    {
        DbPath = "movies.sqlite";
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite($"Data source ={DbPath}");

}