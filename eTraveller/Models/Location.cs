using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System;

namespace eTraveller.Models
{
    [Table("Locations")]
    public class Location
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int32 LocationId { get; set; }

        [Required(ErrorMessage = "{0} cannot be empty...")]
        [Display(Name = "Place Name")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Invalide data format...")]
        public String LocationName { get; set; }

        [Required(ErrorMessage = "{0} cannot be empty...")]
        [RegularExpression(@"^[0-9]+$")]
        public Int32 Distance { get; set; }

        #region Navigation property to transaction model - Booking
        public ICollection<Booking> Bookings { get; set; }
        #endregion
    }
}
