using System;
using System.ComponentModel.DataAnnotations;

namespace ProjektInzynierski.Application.Models
{
    public class FinalizeCartModel
    {
        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
    }
}