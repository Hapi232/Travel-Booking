using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Travel_Booking.Models;

namespace Travel_Booking.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<TravelDestinationModel> TravelDestinations { get; set; }
        public DbSet<TravelBookingModel> TravelBookings { get; set; }
        public DbSet<FlavourModel> Flavours { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TravelDestinationModel>().ToTable("TravelDestinations");
            modelBuilder.Entity<TravelBookingModel>().ToTable("TravelBookings");
            modelBuilder.Entity<FlavourModel>().ToTable("Flavours");

            modelBuilder.Entity<TravelBookingModel>()
                .HasOne(b => b.TravelDestination)
                .WithMany(d => d.TravelBookings)
                .HasForeignKey(b => b.TravelDestinationId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FlavourModel>()
                .HasOne(f => f.TravelDestination)
                .WithMany(d => d.Flavours)
                .HasForeignKey(f => f.TravelDestinationId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
