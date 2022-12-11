using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Linq;
using System.Web;

namespace PDP.Models
{
    public class Consultation
    {
        [Key]
        public int ConsultationID { get; set; }

        [Required(ErrorMessage = "Please pick a day!")]
        [DataType(DataType.Date)]
        public DateTime date_day { get; set; }

        [Required(ErrorMessage = "Please select a slot!")]
        public string slot_hour { get; set; }

        public double price { get; set; }
        public bool canceled { get; set; }

        public virtual Doctor Doctor { get; set; }
        public virtual ApplicationUser user { get; set; }

    }
}
