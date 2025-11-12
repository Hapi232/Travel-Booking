using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Travel_Booking.Models;

namespace Travel_Booking.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<TravelDestinationModel> TravelDestinations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //foreach (var entity in modelBuilder.Model.GetEntityTypes())
            //{
            //    entity.SetTableName(entity.DisplayName());
            //}

            modelBuilder.Entity<TravelDestinationModel>().ToTable("TravelDestinations");
        }
        public DbSet<TravelDestinationViewModel> TravelDestinationViewModel { get; set; } = default!;
    }
}
