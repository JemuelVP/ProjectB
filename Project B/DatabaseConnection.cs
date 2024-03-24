using Microsoft.EntityFrameworkCore;

class DataBaseConnection : DbContext
{
    public DbSet<Film> Movie { get; set; }
    public DbSet<Admin> Admin { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite($"Data source = C:\\Users\\moham\\ProjectB\\Project B\\bin\\Debug\\net7.0\\movie.sqlite");

}