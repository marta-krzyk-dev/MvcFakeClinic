using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedicalClinic.Models
{
    public class Disease
    {
        [Key]
        public int DiseaseId { get; set; }

        [Required]
        [StringLength(150)]
        [Display(Name = "Disease")]
        public string Name { get; set; }

        virtual public Specialization Specialization { get; set; }
        virtual public ICollection<Symptom> Symptoms { get; set; }
    }
}