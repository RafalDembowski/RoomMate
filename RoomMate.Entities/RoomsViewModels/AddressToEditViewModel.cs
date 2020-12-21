using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RoomMate.Entities.RoomsViewModels
{
    public class AddressToEditViewModel
    {
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
        public decimal Lon { get; set; }
        public decimal Lat { get; set; }
    }
}
