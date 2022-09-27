using eTraveller.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System;

namespace eTraveller.Areas.ETraveller.ViewModels
{
    public class ShowBookingViewModel
    {
        [Display(Name = "Select location")]
        [Required(ErrorMessage = "Please select the location for displaying the booking for locations...")]
        public Int32 LocationId { get; set; }
        public ICollection<Location> Locations { get; set; }
        public ICollection<Vehicle> Vehicles { get; set; }


        [Display(Name = "Total Amount")]
        public Int32 TotalPrice { get; set; }
    }
}
