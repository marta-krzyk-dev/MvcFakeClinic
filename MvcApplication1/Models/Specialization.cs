using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace MedicalClinic.Models
{
    public class Specialization
    {
        [Key]
        public int SpecializationId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        [Display (Name="Specialist title")]
        public string DoctorName { get; set; }


        public string DoctorNamePlural { get { return DoctorName + "s"; } }

        //[Required]
        [StringLength(1000)]
        public string Description { get; set; }

        public string IconUrl { get; set; }

        public string ImageUrl { get; set; }

        public virtual ICollection<Doctor> Doctors { get; set; }
    }
}
