using System.ComponentModel.DataAnnotations;

namespace ProjektInzynierski.Application.Models.Users
{
    public class UserRegistrationModel
    {
        [Required(ErrorMessage = "Nazwa użytkownika jest wymagana.")]
        [StringLength(50, ErrorMessage = "Nazwa użytkownika nie może mieć więcej niż 50 znaków.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Adres e-mail jest wymagany.")]
        [EmailAddress(ErrorMessage = "Podaj poprawny adres e-mail.")]
        public string UserEmail { get; set; }

        [Required(ErrorMessage = "Numer telefonu jest wymagany.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane.")]
        [MinLength(6, ErrorMessage = "Hasło musi mieć co najmniej 6 znaków.")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Hasła muszą być identyczne.")]
        public string ConfirmPassword { get; set; }
    }

}
