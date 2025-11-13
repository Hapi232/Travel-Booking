using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Travel_Booking.Data;
using Travel_Booking.Models;

namespace Travel_Booking.Areas.Identity.Pages.Account.Manage
{
    public class TravelHistoryModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

        public TravelHistoryModel(UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public IList<TravelBookingModel> Bookings { get; set; } = default!;

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                Bookings = await _context.TravelBookings
    .Where(b => b.UserId == user.Id)
    .Join(_context.TravelDestinations,
          b => b.TravelDestinationId,
          d => d.Id,
          (b, d) => new TravelBookingModel
          {
              Id = b.Id,
              UserId = b.UserId,
              TravelDestinationId = b.TravelDestinationId,
              Quantity = b.Quantity,
              Price = b.Price,
              Status = b.Status,
              BookedAt = b.BookedAt,
              TravelDestination = d
          })
    .OrderByDescending(b => b.BookedAt)
    .ToListAsync();

            }
        }
    }
}
