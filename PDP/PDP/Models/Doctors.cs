using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Web.Mvc;

namespace PDP.Models
{
    public class Doctor
    {
        [Key]
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "Please enter a first name!")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter a second name!")]
        public string SecondName { get; set; }

        // Doctor's specialization
        [Required(ErrorMessage = "Please pick a specialization!")]
        public int SpecializationID { get; set; }
        public virtual Specialization Specialization { get; set; }
        public IEnumerable<SelectListItem> SpecializationList { get; set; }


        // Whether the Doctor can be booked or not
        public bool IsAvailable { get; set; }

        [Required(ErrorMessage = "Please enter a price rate!")]
        [Range(typeof(double), "0.01", "100.00", ErrorMessage = "enter decimal value")]
        public double PriceRate { get; set; }

        [Required(ErrorMessage = "The phone number is mandatory.")]
        [RegularExpression(@"^(\+4|)?[0-9]{6,}", ErrorMessage = "This phone number is not valid.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "The email is mandatory.")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "This email is not valid.")]
        public string Email { get; set; }

        // A string which points to a doctor profile photo
        [Required(ErrorMessage = "Please enter a photo url!")]
        public string Photo { get; set; }

        // Calculated based on reviews
        public double Rating { get; set; }

        public virtual ICollection<Consultation> Consultations { get; set; }

        // Calculates consultation price
        public double CalcultateConsultationPrice(Specialization specialization)
        {
            return specialization.Price * this.PriceRate;
        }
    }
}
