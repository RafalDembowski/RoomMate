
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RoomMate.Entities.Rooms;
using RoomMate.Entities.Users;

namespace RoomMate.Entities.Bookings
{
    public class Booking
    {
        [Key]
        [Column(Order = 1)]
        public Guid BookingID { get; set; }
        [Required(ErrorMessage = "Podanie daty jest wymagane.")]
        public DateTime InDate { get; set; }
        [Required(ErrorMessage = "Podanie daty jest wymagane.")]
        public DateTime OutDate { get; set; }
        [Required(ErrorMessage = "Podanie ilości gości jest wymagane.")]
        public int NumberOfGuests { get; set; }
        public float? TotalPrice { get; set; }
        [Required]
        public virtual Room Room { get; set; }
        [Required]
        public virtual User User { get; set; }
    }
}
