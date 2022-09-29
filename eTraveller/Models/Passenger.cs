using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System;

namespace eTraveller.Models
{
    [Table("Passengers")]
    public class Passenger
    {
        [Display(Name = "Passenger ID")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int32 PassengerId { get; set; }

        [Display(Name = "Passenger Name")]
        [Required(ErrorMessage = "{0} cannot be empty.")]
        public String PassengerName { get; set; }

        [Display(Name = "Aadhar Number")]
        [Required]
        [RegularExpression(@"^[0-9]+$")]
        [StringLength(12, ErrorMessage = "Please enter valid {0}")]
        public String AadharNumber { get; set; }

        [Display(Name = "Email Id")]
        [EmailAddress]
        public String Email { get; set; }

        [Display(Name = "Contact Number")]
        [Required(ErrorMessage = "{0} cannot be empty....If you don't have contact number then please share your relatives contact number...")]
        [StringLength(10, ErrorMessage = "{0} must be of 10 digits only...")]
        public String PhoneNumber { get; set; }

        [Display(Name = "Age")]
        [Required]
        [Range(1,100,ErrorMessage ="Please enter valid age")]
        //[RegularExpression(@"^[1-9]+$", ErrorMessage = "{0} must be positive number only...")]
        public Int32 Age { get; set; }


        #region Navigation property to transaction model- Booking
        public ICollection<Booking> Bookings { get; set; }
        #endregion
    }
}
