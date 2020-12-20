using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoomMate.Entities.Rooms
{
    public class Address
    {
        [Key]
        [Column(Order = 1)]
        public Guid AddressID { get; set; }
        [Required(ErrorMessage = "Wymagane jest podanie miasta.")]
        [MaxLength(256)]
        public string City { get; set; }
        [Required(ErrorMessage = "Wymagane jest podanie ulicy.")]
        [MaxLength(256)]
        public string Street { get; set; }
        [Required(ErrorMessage = "Wymagane jest podanie kodu pocztowego.")]
        [MaxLength(50)]
        public string PostCode { get; set; }
        [Required(ErrorMessage = "Wymagane jest podanie numeru domu.")]
        [MaxLength(50)]
        public string Flat { get; set; }
        [Required]
        public decimal Lon { get; set; }
        [Required]
        public decimal Lat { get; set; }
        [Required]
        public virtual Room Room { get; set; }
    }
}