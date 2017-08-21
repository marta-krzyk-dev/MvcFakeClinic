using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalClinic.Models
{
    public enum SymptomCategory { Dermatological, Emotions, Pains, Viral }; //Keep the alphabetical order

    public class Symptom
    {
        public int SymptomId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public SymptomCategory Category { get; set; }

        [ForeignKey("DiseaseId ")]
        virtual public ICollection<Disease> Diseases { get; set; }
    }


}