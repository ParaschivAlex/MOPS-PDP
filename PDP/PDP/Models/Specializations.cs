using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace PDP.Models
{
    public class Specializations
    {
        [Key]
        public int SpecializationID { get; set; }

        [Required(ErrorMessage = "Price is required")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Specialization name is required")]
        public string Name { get; set; }
    }
}