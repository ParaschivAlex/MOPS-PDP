using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace PDP.Models
{
    public class Doctor
    {
        [Key]
        public int DoctorID { get; set; }

        [Required(ErrorMessage = "Please enter your first name!")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter your second name!")]
        public string SecondName { get; set; }

        // Doctor's specialization
        public int SpecializationID { get; set; }

        // Whether the Doctor can be booked or not
        public bool IsAvailable { get; set; }

        public float PriceRate { get; set; }

        [MinLength(11), MaxLength(12)]
        public string PhoneNumber { get; set; }

        // A string which points to a doctor profile photo
        public string Photo { get; set; }

        // Calculated based on reviews
        public float Rating { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}