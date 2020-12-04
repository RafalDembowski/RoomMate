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
        [Required]
        [MaxLength(256)]
        public string City { get; set; }
        [Required]
        [MaxLength(256)]
        public string Street { get; set; }
        [Required]
        [MaxLength(50)]
        public string PostCode { get; set; }
        [Required]
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