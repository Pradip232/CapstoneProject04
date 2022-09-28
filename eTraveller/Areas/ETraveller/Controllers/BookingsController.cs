using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using eTraveller.Data;
using eTraveller.Models;
using Microsoft.AspNetCore.Authorization;

namespace eTraveller.Areas.ETraveller.Controllers
{
    [Area("ETraveller")]
    [Authorize]
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ETraveller/Bookings
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Bookings.Include(b => b.Location).Include(b => b.Vehicle);
            return View(await applicationDbContext.ToListAsync());
        }
        public async Task<IActionResult> Index2()
        {
            var applicationDbContext = _context.Bookings.Include(b => b.Location).Include(b => b.Vehicle);
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> GetBookingsOfLocation(int FilterLocationId)
        {
            var viewModel = await _context.Bookings.Where(b => b.LocationId == FilterLocationId).Include(b => b.Location).ToListAsync();
            return View(viewName: "Index2", model: viewModel);
        }

        // GET: ETraveller/Bookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Location)
                .Include(b => b.Passenger)
                .Include(b => b.Vehicle)
                .FirstOrDefaultAsync(m => m.BookingId == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // GET: ETraveller/Bookings/Create
        public IActionResult Create()
        {
            ViewData["LocationId"] = new SelectList(_context.Locations, "LocationId", "LocationName");
            ViewData["PassengerId"] = new SelectList(_context.Passengers, "PassengerId", "PassengerName");
            ViewData["VehicleId"] = new SelectList(_context.Vehicles, "VehicleId", "VehicleName");
            return View();
        }

        // POST: ETraveller/Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookingId,BookingDate,PassengerCount,LocationId,VehicleId,PassengerId")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LocationId"] = new SelectList(_context.Locations, "LocationId", "LocationName", booking.LocationId);
            ViewData["PassengerId"] = new SelectList(_context.Passengers, "PassengerId", "PassengerName", booking.PassengerId);
            ViewData["VehicleId"] = new SelectList(_context.Vehicles, "VehicleId", "VehicleName", booking.VehicleId);
            return View(booking);
        }

        // GET: ETraveller/Bookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            ViewData["LocationId"] = new SelectList(_context.Locations, "LocationId", "LocationName", booking.LocationId);
            ViewData["PassengerId"] = new SelectList(_context.Passengers, "PassengerId", "PassengerName", booking.PassengerId);
            ViewData["VehicleId"] = new SelectList(_context.Vehicles, "VehicleId", "VehicleName", booking.VehicleId);
            return View(booking);
        }

        // POST: ETraveller/Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookingId,BookingDate,PassengerCount,LocationId,VehicleId,PassengerId")] Booking booking)
        {
            if (id != booking.BookingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.BookingId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["LocationId"] = new SelectList(_context.Locations, "LocationId", "LocationName", booking.LocationId);
            ViewData["PassengerId"] = new SelectList(_context.Passengers, "PassengerId", "PassengerName", booking.PassengerId);
            ViewData["VehicleId"] = new SelectList(_context.Vehicles, "VehicleId", "VehicleName", booking.VehicleId);
            return View(booking);
        }

        // GET: ETraveller/Bookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Location)
                .Include(b => b.Passenger)
                .Include(b => b.Vehicle)
                .FirstOrDefaultAsync(m => m.BookingId == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // POST: ETraveller/Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.BookingId == id);
        }
    }
}
