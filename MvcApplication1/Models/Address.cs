using System.ComponentModel.DataAnnotations;

namespace MedicalClinic.Models
{
    public class Address
    {
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
    }
}