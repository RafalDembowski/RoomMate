using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RoomMate.Entities.RoomsViewModels
{
    public class RoomToEditViewModel
    {
        [Required(ErrorMessage = "Nazwa pokoju jest wymagana.")]
        [MaxLength(256)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Wymagane jest podanie ceny pokoju za jedeną noc.")]
        public int Price { get; set; }
        [Required(ErrorMessage = "Opis pokoju jest wymagany.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Wymagane jest podanie maksymalnej liczby gości.")]
        public int NumberOfGuests { get; set; }
    }
}