using Microsoft.EntityFrameworkCore;

public class DataBaseConnection : DbContext
{
    public DbSet<Film> Movie { get; set; }
    public DbSet<Admin> Admin { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options) =>
        options.UseSqlite($"Data source = ../../../DataBase/movie.sqlite");
}
