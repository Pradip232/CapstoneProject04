using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System;

namespace eTraveller.Models
{
    [Table("Vehicles")]
    public class Vehicle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int32 VehicleId { get; set; }

        [Display(Name = "Vehicle Name")]
        [Required(ErrorMessage = "{0} expects vehicle name. Please provide appropriate data.")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Invalide data format...")]
        public String VehicleName { get; set; }

        [Display(Name = "Cost per Km")]
        [Required(ErrorMessage = "{0} cannot be empty...")]
        public Int32 CostPerKm { get; set; }

        [Display(Name = "Maintainance Cost")]
        public Int32 Maintainance { get; set; }

        #region Navigation property to transaction model- Booking
        public ICollection<Booking> Bookings { get; set; }
        #endregion
    }
}
