using System.ComponentModel.DataAnnotations;

namespace ProjektInzynierski.Models
{
    public class UserLoginModel
    {
        [Required(ErrorMessage = "Nazwa użytkownika jest wymagana.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
