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

        [Required(ErrorMessage = "Please pick a start date!")]
        DateTime startDate { get; set; }

        [Required(ErrorMessage = "Please pick an end date!")]
        DateTime endDate { get; set; }

        public double price { get; set; }
        public bool canceled { get; set; }

        public virtual Doctor Doctor { get; set; }
        public virtual ApplicationUser user { get; set; }

    }
}
