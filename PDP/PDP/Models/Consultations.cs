﻿using System;
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
        [Range(0, 48, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int slot_hour { get; set; }

        [Range(typeof(double), "0.01", "100000.00", ErrorMessage = "enter decimal value")]
        [RegularExpression(@"^\[0-9]{1,6}\.[0-9]{2}$", ErrorMessage = "enter decimal value of format $9.99")]
        public double price { get; set; }
        public bool canceled { get; set; }

        [Required]
        public int DoctorId { get; set; }
        public virtual Doctor Doctor { get; set; }

        [Required]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
