using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System;
using eTraveller.ValidationTest;

namespace eTraveller.Models
{
    [Table("Bookings")]
    public class Booking
    {
        [Display(Name = "Booking ID")]
        [Key]
        public Int32 BookingId { get; set; }

        [Display(Name = "Booking Date")]
        [ValidationOneMonth]
        [Required]
        public DateTime BookingDate { get; set; }

        [Display(Name = "Count of Passengers")]
        [Required]
        [Range(1,4)]

        public Int32 PassengerCount { get; set; }

        #region Navigation property to Location model
        [Display(Name = "Select your location")]
        public Int32 LocationId { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        [ForeignKey(nameof(Booking.LocationId))]
        public Location Location { get; set; }
        #endregion

        #region Navigation property to Vehicle Model
        [Display(Name = "Select vehicle type")]
        public Int32 VehicleId { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        [ForeignKey(nameof(Booking.VehicleId))]
        public Vehicle Vehicle { get; set; }
        #endregion

        #region Navigation property to Passenger Model
        [Display(Name = "Passenger Name")]
        public int PassengerId { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        [ForeignKey(nameof(Booking.PassengerId))]
        public Passenger Passenger { get; set; }
        #endregion
    }
}
