﻿using System.ComponentModel.DataAnnotations;

namespace ProjektInzynierski.Models
{
    public class UserLoginModel
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }

}
