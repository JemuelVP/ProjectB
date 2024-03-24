using Microsoft.EntityFrameworkCore;

class DataBaseConnection : DbContext
{
    public DbSet<Film> Movie { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite($"Data source = ../../../DataBase/movie.sqlite");

}