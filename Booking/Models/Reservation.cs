using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Booking.Models
{
	public class Reservation
	{
        [Key]
        public int Id { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Името може да содржи само букви!")]
        [Display(Name = "Име")]
        public String Name { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Презимето може да содржи само букви!")]
        [Display(Name = "Презиме")]
        public String Surname { get; set; }

        [Required]
        [Display(Name = "Почетен датум")]
        [DataType(DataType.Date)]
        public DateTime DateOfReservation { get; set; }

        [Required]
        [Display(Name = "Краен датум")]
        [DataType(DataType.Date)]
        public DateTime EndOfReservation { get; set; }

        [Display(Name = "Вкупно за плаќање")]
        public int TotalPrice { get; set; }

        [ForeignKey("Room")]
        public int RoomId { get; set; }
        public Room Room { get; set; }

    }
}