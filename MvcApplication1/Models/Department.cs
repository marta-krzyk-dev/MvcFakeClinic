using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MedicalClinic.Models;
using System.ComponentModel;

namespace MedicalClinic.Models
{
    public class Department
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Department")]
        public string Name { get { return City + " Clinic"; } }

        [Required]
        [StringLength(50)]
        public string City { get; set; }

        [StringLength(50)]
        [Display(Name = "Postal code")]
        [RegularExpression(@"([0-9- ]{6})",
ErrorMessage = "Postal code is is not valid.")]
        public string PostalCode { get; set; }

        [Required]
        [StringLength(50)]
        public string Street { get; set; }

        [Required]
        [Display(Name = "House number")]
        [StringLength(50)]
        public string HouseNumber { get; set; }

        // [RegularExpression(@"/\(?([0-9]{3})\)?([ .-]?)([0-9]{3})\2([0-9]{4})/",
        //ErrorMessage = "Phone number is is not valid.")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        [DisplayName("Email")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}",
        ErrorMessage = "Email is is not valid.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DisplayName("Picture url")]
        public string ImageUrl { get; set; }

        virtual public ICollection<Doctor> Doctors { get; set; }
    }
}