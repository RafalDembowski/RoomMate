using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RoomMate.Entities.BookingsViewModels
{
    public class BookingViewModel
    {

        [Required(ErrorMessage = "Podanie daty jest wymagane.")]
        public DateTime InDate { get; set; }
        [Required(ErrorMessage = "Podanie daty jest wymagane.")]
        public DateTime OutDate { get; set; }
        [Required(ErrorMessage = "Podanie ilości gości jest wymagane.")]
        public int NumberOfGuests { get; set; }
    }
}