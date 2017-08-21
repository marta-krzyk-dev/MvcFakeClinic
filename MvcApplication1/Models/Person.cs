using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MedicalClinic.Models
{
    public abstract class Person
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        [DisplayName("First name")]
        [StringLength(160)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Surname is required")]
        [DisplayName("Surname")]
        [StringLength(160)]
        public string Surname { get; set; }

        [Display(Name = "Full name")]
        public string FullName
        {
            get
            {
                return Name + " " + Surname;
            }
        }

        //[Required(ErrorMessage = "Email Address is required")]
        [DisplayName("Email")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}",
        ErrorMessage = "Email is is not valid.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}