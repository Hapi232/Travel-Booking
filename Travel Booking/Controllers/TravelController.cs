using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Travel_Booking.Data;
using Travel_Booking.Models;

namespace Travel_Booking.Controllers
{
    [Authorize]
    public class TravelController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TravelController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Travel/List
        public async Task<IActionResult> Index()
        {
            var destinations = await _context.TravelDestinations.ToListAsync();
            return View(destinations);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Filter(string searchText = "", decimal minPrice = 0, decimal maxPrice = 10000)
        {
            var destinations = await _context.TravelDestinations
                .Where(d => d.IsAvailable
                            && d.Price >= minPrice
                            && d.Price <= maxPrice
                            && (d.Name.Contains(searchText) || d.Description.Contains(searchText)))
                .ToListAsync();

            return PartialView("_DestinationListPartial", destinations);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return BadRequest();

            var destination = await _context.TravelDestinations
                .FirstOrDefaultAsync(d => d.Id == id);

            if (destination == null) return NotFound();

            return View(destination);
        }
    }
}
