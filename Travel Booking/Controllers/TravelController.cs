using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
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
            try
            {
                var destinations = await _context.TravelDestinations.ToListAsync();
                return View(destinations);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }

        [AllowAnonymous]
        public async Task<IActionResult> Filter(string searchText = "", decimal minPrice = 0, decimal maxPrice = 10000)
        {
            try
            {
                var destinations = await _context.TravelDestinations
                    .Where(d => d.IsAvailable
                                && d.Price >= minPrice
                                && d.Price <= maxPrice
                                && (d.Name.Contains(searchText) || d.Description.Contains(searchText)))
                    .ToListAsync();

                return PartialView("_DestinationListPartial", destinations);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest();

                var destination = await _context.TravelDestinations.FirstOrDefaultAsync(d => d.Id == id);

                if (destination == null)
                    return NotFound();

                return View(destination);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> PurchasePage(Guid travelId)
        {
            try
            {
                if (travelId == Guid.Empty)
                    return BadRequest();

                var destination = await _context.TravelDestinations
                    .FirstOrDefaultAsync(d => d.Id == travelId);

                if (destination == null)
                    return NotFound();

                return View(destination);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Purchase(Guid travelId, int quantity, string bankName, string accountNumber)
        {
            if (travelId == Guid.Empty || quantity < 1)
                return BadRequest();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var destination = await _context.TravelDestinations.FindAsync(travelId);
            if (destination == null)
                return NotFound();

            if (!destination.IsAvailable || destination.Quantity < quantity)
            {
                TempData["ErrorMessage"] = "Not enough stock or destination unavailable.";
                return RedirectToAction("PurchasePage", new { travelId });
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                bool paymentSuccess = !string.IsNullOrEmpty(bankName) && !string.IsNullOrEmpty(accountNumber);

                if (!paymentSuccess)
                {
                    TempData["ErrorMessage"] = "Payment failed. Please check bank info.";
                    return RedirectToAction("PurchasePage", new { travelId });
                }

                var booking = new TravelBookingModel
                {
                    UserId = userId,
                    TravelDestinationId = travelId,
                    Quantity = quantity,
                    Price = destination.Price * quantity,
                    Status = "Paid",
                    BookedAt = DateTime.Now
                };

                _context.TravelBookings.Add(booking);

                destination.Quantity -= quantity;

                if (destination.Quantity == 0)
                    destination.IsAvailable = false;

                _context.TravelDestinations.Update(destination);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                TempData["SuccessMessage"] = "Purchase successful!";
                return RedirectToAction("Details", new { id = travelId });
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                TempData["ErrorMessage"] = "Purchase failed. Please try again.";
                return RedirectToAction("PurchasePage", new { travelId });
                throw;
            }
        }
    }
}
