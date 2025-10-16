using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Booking.Models
{
	public class Room
	{
            [Key]
            public int Id { get; set; }

            [Display(Name = "Број на соба")]
            [Required]
            public int Number { get; set; }

            [Required]
            [Range(1, int.MaxValue, ErrorMessage = "Внесете капацитет поголем или еднаков на 1!")]
            [Display(Name = "Капацитет")]
            public int Capacity { get; set; }

            [Display(Name = "Слика")]
            public String ImageURL { get; set; }

            [Required]
            [Range(1, int.MaxValue, ErrorMessage = "Внесете цена поголема или еднаква на 1!")]
            [Display(Name = "Цена по ноќ (МКД)")]
            public int Price { get; set; }

            [Display(Name = "Статус")]
            public String Status { get; set; }

            [ForeignKey("Hotel")]
            public int HotelId { get; set; }
            public Hotel Hotel { get; set; }

       
    }
}