using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RoomMate.Entities.UsersViewModels
{
    public class UserRegistrationViewModel
    {
        [Display(Name = "Login")]

        [Required(ErrorMessage = "Login jest wymagany.")]
        public string UserName { get; set; }
        [Display(Name = "Imię")]

        [Required(ErrorMessage = "Imię jest wymagane.")]
        public string FirstName { get; set; }
        [Display(Name = "Nazwisko")]

        [Required(ErrorMessage = "Nazwisko jest wymagane")]
        public string LastName { get; set; }
        [Display(Name = "Adres email")]

        [Required(ErrorMessage = "Email jest wymagany.")]
        [EmailAddress(ErrorMessage = "Niepoprawny format adresu email")]
        public string Email { get; set; }
        [Display(Name = "Hasło")]
        [Required(ErrorMessage = "Hasło jest wymagane.")]
        [StringLength(256, MinimumLength = 8, ErrorMessage = "Hasło musi się składać z minimum 8 znaków.")]
        public string PasswordHash { get; set; }
    }
}
