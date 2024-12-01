using System.ComponentModel.DataAnnotations;

{
    public class UserLoginModel
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }

}
