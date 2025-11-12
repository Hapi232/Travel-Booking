using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Travel_Booking.Models;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<TravelDestinationModel> TravelDestinations { get; set; }
    public DbSet<TravelBookingModel> TravelBookings { get; set; }
    public DbSet<FlavourModel> Flavours { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TravelDestinationModel>().ToTable("TravelDestinations");
        modelBuilder.Entity<TravelBookingModel>().ToTable("TravelBookings");
        modelBuilder.Entity<FlavourModel>().ToTable("Flavours");

        modelBuilder.Entity<FlavourModel>()
            .HasOne(f => f.TravelDestination)
            .WithMany()
            .HasForeignKey(f => f.TravelDestinationId);

        modelBuilder.Entity<TravelBookingModel>()
            .HasOne<TravelDestinationModel>()
            .WithMany()
            .HasForeignKey(b => b.TravelDestinationId);
    }
}
