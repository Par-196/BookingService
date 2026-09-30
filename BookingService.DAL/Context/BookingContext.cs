using BookingService.DAL.Entity;
using Microsoft.EntityFrameworkCore;

namespace BookingService.DAL.Context;

public class BookingContext(DbContextOptions<BookingContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Apartment> Apartments { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(BookingContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}