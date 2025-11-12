using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Travel_Booking.Data;
using Travel_Booking.Models;

namespace Travel_Booking.Controllers
{
    [Authorize]
    public class FlavourController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FlavourController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Add(Guid travelId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized();

            var exists = await _context.Flavours
                .AnyAsync(f => f.UserId == userId && f.TravelDestinationId == travelId);

            if (!exists)
            {
                var flavour = new FlavourModel
                {
                    UserId = userId,
                    TravelDestinationId = travelId
                };
                _context.Flavours.Add(flavour);
                await _context.SaveChangesAsync();
            }

            return Ok(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> Remove(Guid travelId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized();

            var flavour = await _context.Flavours
                .FirstOrDefaultAsync(f => f.UserId == userId && f.TravelDestinationId == travelId);

            if (flavour != null)
            {
                _context.Flavours.Remove(flavour);
                await _context.SaveChangesAsync();
            }

            return Ok(new { success = true });
        }
        public async Task<IActionResult> MyFlavours()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var favourites = await _context.Flavours
                .Include(f => f.TravelDestination)
                .Where(f => f.UserId == userId)
                .Select(f => f.TravelDestination)
                .ToListAsync();

            return View(favourites);
        }

        public async Task<IActionResult> IsAdded(Guid travelId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Json(new { isAdded = false });

            var exists = await _context.Flavours
                .AnyAsync(f => f.UserId == userId && f.TravelDestinationId == travelId);

            return Json(new { isAdded = exists });
        }
    }
}
