using System.ComponentModel.DataAnnotations;

namespace ProjektInzynierski.Infrastructure.Models
{
    public class UserRegistrationModel
    {
        [Required(ErrorMessage = "Nazwa użytkownika jest wymagana.")]
        [StringLength(50, ErrorMessage = "Nazwa użytkownika nie może mieć więcej niż 50 znaków.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "E-mail jest wymagany.")]
        [EmailAddress(ErrorMessage = "Nieprawidłowy format e-mail.")]
        public string UserEmail { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane.")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Hasło musi mieć co najmniej 6 znaków.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Numer telefonu jest wymagany.")]
        [Phone(ErrorMessage = "Nieprawidłowy format numeru telefonu.")]
        public string Phone { get; set; }
    }


}
