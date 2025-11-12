using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Travel_Booking.Data;
using Travel_Booking.Models;

namespace Travel_Booking.Controllers
{
    public class TravelDestinationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TravelDestinationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TravelDestination
        public async Task<IActionResult> Index()
        {
            var destinations = await _context.TravelDestinations.ToListAsync();

            var viewModel = destinations.Select(d => new TravelDestinationViewModel
            {
                Id = d.Id,
                Name = d.Name,
                Description = d.Description,
                Price = d.Price,
                StartTripString = d.StartTrip.ToString("yyyy-MM-dd"),
                EndTripString = d.EndTrip.ToString("yyyy-MM-dd"),
                Quantity = d.Quantity,
                IsAvailable = d.IsAvailable
            }).ToList();

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Filter(string search, string availableFilter)
        {
            var destinations = _context.TravelDestinations.AsQueryable();

            if (!string.IsNullOrEmpty(search))
                destinations = destinations.Where(d => d.Name.Contains(search) || d.Description.Contains(search));

            if (!string.IsNullOrEmpty(availableFilter))
            {
                bool isAvailable = availableFilter == "true";
                destinations = destinations.Where(d => d.IsAvailable == isAvailable);
            }

            var viewModel = await destinations.Select(d => new TravelDestinationViewModel
            {
                Id = d.Id,
                Name = d.Name,
                Description = d.Description,
                Price = d.Price,
                Quantity = d.Quantity,
                IsAvailable = d.IsAvailable,
                StartTripString = d.StartTrip.ToString("yyyy-MM-dd"),
                EndTripString = d.EndTrip.ToString("yyyy-MM-dd")
            }).ToListAsync();

            return PartialView("_TravelDestinationsTable", viewModel);
        }

        // GET: TravelDestination/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            if (id == null)
                return BadRequest();

            var destination = await _context.TravelDestinations
                .FirstOrDefaultAsync(m => m.Id == id);

            if (destination == null)
                return NotFound();

            var viewModel = new TravelDestinationViewModel
            {
                Id = destination.Id,
                Name = destination.Name,
                Description = destination.Description,
                Price = destination.Price,
                ImageUrl = destination.ImageUrl,
                StartTripString = destination.StartTrip.ToString("yyyy-MM-dd"),
                EndTripString = destination.EndTrip.ToString("yyyy-MM-dd"),
                Quantity = destination.Quantity,
                IsAvailable = destination.IsAvailable
            };

            return View(viewModel);
        }

        // GET: TravelDestination/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TravelDestination/Create
        [HttpPost]
        public async Task<IActionResult> Create(TravelDestinationModel destination)
        {
            if (destination.EndTrip < destination.StartTrip)
            {
                ModelState.AddModelError("EndTrip", "End Trip cannot be earlier than Start Trip.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(destination);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(destination);
        }

        // GET: TravelDestination/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == null)
                return BadRequest();

            var destination = await _context.TravelDestinations.FindAsync(id);
            if (destination == null)
                return NotFound();

            destination.StartTrip = destination.StartTrip.ToLocalTime();
            destination.EndTrip = destination.EndTrip.ToLocalTime();

            return View(destination);
        }

        // POST: TravelDestination/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(int id, TravelDestinationModel destination)
        {
            if (id != destination.Id)
                return NotFound();

            if (destination.EndTrip < destination.StartTrip)
            {
                ModelState.AddModelError("EndTrip", "End Trip cannot be earlier than Start Trip.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(destination);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await TravelDestinationExists(destination.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            return View(destination);
        }

        // GET: TravelDestination/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == null)
                return BadRequest();

            var destination = await _context.TravelDestinations
                .FirstOrDefaultAsync(m => m.Id == id);

            if (destination == null)
                return NotFound();

            var viewModel = new TravelDestinationViewModel
            {
                Id = destination.Id,
                Name = destination.Name,
                Description = destination.Description,
                Price = destination.Price,
                ImageUrl = destination.ImageUrl,
                StartTripString = destination.StartTrip.ToString("yyyy-MM-dd"),
                EndTripString = destination.EndTrip.ToString("yyyy-MM-dd"),
                Quantity = destination.Quantity,
                IsAvailable = destination.IsAvailable
            };

            return View(viewModel);
        }

        // POST: DeleteConfirmed
        [HttpPost, ActionName("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var destination = await _context.TravelDestinations.FindAsync(id);
            if (destination != null)
            {
                _context.TravelDestinations.Remove(destination);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> TravelDestinationExists(int id)
        {
            return await _context.TravelDestinations.AnyAsync(e => e.Id == id);
        }
    }
}
