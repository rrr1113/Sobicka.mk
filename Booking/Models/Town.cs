using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Booking.Models
{
	public class Town
	{
        [Key]
        public int Id { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Градот може да содржи само букви и празни места!")]
        [Display(Name = "Град")]
        public String Name { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Државата може да содржи само букви и празни места!")]
        [Display(Name = "Држава")]
        public String Country { get; set; }

        [Display(Name = "Број на хотели")]
        public int NumberOfHotels { get; set; }

        public virtual List<Hotel> Hotels { get; set; } = new List<Hotel>();
    }
}