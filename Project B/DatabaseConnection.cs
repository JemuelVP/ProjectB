using Microsoft.EntityFrameworkCore;

class DataBaseConnection : DbContext
{
    public DbSet<Film> Movie { get; set; }
    public DbSet<Admin> Admin { get; set; }
    public DbSet<Schedule> Schedule { get; set; }

    public DbSet<CinemaHall> Hall { get; set; }

    public DbSet<Ticket> Ticket { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Schedule>()
        .HasOne(s => s.Film)
        .WithMany(f => f.Schedules)
        .HasForeignKey(s => s.FilmID);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite($"Data source = ../../../DataBase/movie.sqlite");
    }

}