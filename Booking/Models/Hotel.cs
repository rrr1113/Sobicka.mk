using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Booking.Models
{
	public class Hotel
	{
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Име на хотел")]
        public String Name { get; set; }

        [Display(Name = "Слика од хотел")]
        public String ImageURL { get; set; }

        [Required]
        [Display(Name = "Адреса")]
        public String Address { get; set; }

        [Display(Name = "Контакт телефон")]
        [RegularExpression(@"^[\d\s]+$", ErrorMessage = "Дозволени се само бројки и празни места!")] 
        public String PhoneNumber { get; set; }

        public virtual List<Room> Rooms { get; set; } = new List<Room>();

        [ForeignKey("Town")]
        public int TownId { get; set; }
        public Town Town { get; set; }
    }
}