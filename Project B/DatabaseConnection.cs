using Microsoft.EntityFrameworkCore;

class DataBaseConnection : DbContext
{
    public DbSet<Film> Movie { get; set; }
    public DbSet<Users> Users { get; set; }
    public DbSet<Schedule> Schedule { get; set; }

    public DbSet<CinemaHall> Hall { get; set; }

    public DbSet<Ticket> Ticket { get; set; }
    public DbSet<Chair> Chair { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Schedule>()
            .HasOne(s => s.Film)
            .WithMany(f => f.Schedules)
            .HasForeignKey(s => s.Movie_ID);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var databasePath = Path.GetFullPath("../../../DataBase\\movie.sqlite");
        optionsBuilder.UseSqlite($"Data Source={databasePath}");
        optionsBuilder.EnableSensitiveDataLogging();
        // options.UseSqlite($"Data source = ../../../DataBase/movie.sqlite");
    }
}
