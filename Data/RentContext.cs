using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using web.Models;
namespace web.Data;

public class RentContext : IdentityDbContext<User> {

    public RentContext(DbContextOptions<RentContext> options) : base(options) { }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<Car> Cars { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<Payment> Payments { get; set; }

    protected override void OnModelCreating(ModelBuilder builder) {
        base.OnModelCreating(builder);
        builder.Entity<Location>()
            .HasMany(l => l.availableCars)
            .WithOne(c => c.Location)
            .HasForeignKey(c => c.LocationID);
    }
}