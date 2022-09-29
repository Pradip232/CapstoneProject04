using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using eTraveller.Data;
using eTraveller.Models;
using Microsoft.Extensions.Logging;

namespace eTraveller.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<LocationsController> _logger;

        public LocationsController(ApplicationDbContext context, ILogger<LocationsController> logger)
        {
            _logger= logger;
            _context = context;
        }

        // GET: api/Locations
        [HttpGet]
        public async Task<IActionResult> GetLocations()
        {
            try {
                var locations = await _context.Locations.Include(l => l.Bookings).ToListAsync();

                if (locations == null)
                {
                    return NotFound();
                }
                return Ok(locations);
            }
            catch (Exception eo) {
                return BadRequest(eo.Message);
            }
        }

        // GET: api/Locations/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLocation(int? id)
        {
            if (!id.HasValue) {
                return BadRequest();
            }
            try
            {
                var location = await _context.Locations.FindAsync(id);

                if (location == null)
                {
                    return NotFound();
                }

                return Ok(location);
            }
            catch (Exception eo) {
                return BadRequest(eo.Message);
            }
        }

        // PUT: api/Locations/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLocation(int id, Location location)
        {
            if (id != location.LocationId)
            {
                return BadRequest();
            }

            _context.Entry(location).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LocationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Locations
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<IActionResult> PostLocation(Location location)
        {
            try
            {
                _context.Locations.Add(location);
                await _context.SaveChangesAsync();

                //To enforce that the HTTP responce code '201' "CREATED", package the responce.
                var result = CreatedAtAction("GetLocation", new { id = location.LocationId }, location);

                return Ok(result);
            }
            catch (Exception eo) {
                ModelState.AddModelError("POST", eo.Message);
                return BadRequest(ModelState);
            }
                  
        }

        // DELETE: api/Locations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocation(int? id)
        {
            if(!id.HasValue)
                return BadRequest();
            try
            {

                var location = await _context.Locations.FindAsync(id);
                if (location == null)
                {
                    return NotFound();
                }

                _context.Locations.Remove(location);
                await _context.SaveChangesAsync();

                return Ok(location);
            }
            catch (Exception eo) {
                ModelState.AddModelError("DELETE", eo.Message);
                return BadRequest(ModelState);
            }
        }

        private bool LocationExists(int id)
        {
            return _context.Locations.Any(e => e.LocationId == id);
        }
    }
}
