using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Travel_Booking.Data;
using Travel_Booking.Models;

namespace Travel_Booking.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            try
            {
                var destinations = await _context.TravelDestinations
                                                    .Where(d => d.IsAvailable)
                                                    .OrderBy(d => Guid.NewGuid())
                                                    .Take(4)
                                                    .ToListAsync();

                var viewModel = destinations.Select(d => new TravelDestinationViewModel
                {
                    Id = d.Id,
                    Name = d.Name,
                    Description = d.Description,
                    ImageUrl = d.ImageUrl,
                    IsAvailable = d.IsAvailable,
                    StartTripString = d.StartTrip.ToString("yyyy-MM-dd"),
                    EndTripString = d.EndTrip.ToString("yyyy-MM-dd")
                }).ToList();

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading homepage data");
                return RedirectToAction("Error");
                throw ex;
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
