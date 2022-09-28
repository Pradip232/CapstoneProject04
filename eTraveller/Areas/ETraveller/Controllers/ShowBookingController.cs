using eTraveller.Areas.ETraveller.ViewModels;
using eTraveller.Data;
using eTraveller.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace eTraveller.Areas.ETraveller.Controllers
{
    [Area("ETraveller")]
    [Authorize]
    public class ShowBookingController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<ShowBookingController> _logger;

        public Location Location;
        public Vehicle Vehicle;
        public ShowBookingViewModel _viewModel;

        public ShowBookingController(ApplicationDbContext dbContext, ILogger<ShowBookingController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        public IActionResult Index()
        {
            PopulateDropdownListToSelectLocations();
            _logger.LogInformation("------Extracted Bookings------");
            return View();
        }

        private void PopulateDropdownListToSelectLocations()
        {
            ShowBookingViewModel viewmodel = new ShowBookingViewModel();

            List<SelectListItem> locations = new List<SelectListItem>();
            locations.Add(new SelectListItem
            {
                Text = "--------Select Location---------",
                Value = "",
                Selected = true
            });
            locations.AddRange(new SelectList(_dbContext.Locations, "LocationId", "LocationName"));
            ViewData["LocationsCollection"] = locations;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index([Bind("LocationId")] ShowBookingViewModel viewModel)
        {

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            List<SelectListItem> selectedLocations = new List<SelectListItem>();

            //selectedLocations.Add(new SelectList())
            //Int32 Distance =

            //viewModel.TotalPrice = (Location.Distance * Vehicle.CostPerKm) + Vehicle.Maintainance;

            bool BookingExists = _dbContext.Bookings.Any(b => b.LocationId == viewModel.LocationId);
            //Int32 TotalPrice = _dbContext.Locations.; 
            if (!BookingExists)
            {
                ModelState.AddModelError("", "No booking were found for selected location...");
                PopulateDropdownListToSelectLocations();
                return View(viewModel);
            }
            return RedirectToAction(actionName: "GetBookingsOfLocation", controllerName: "Bookings", routeValues: new { area = "eTraveller", FilterLocationId = viewModel.LocationId });

        }
    }
}
