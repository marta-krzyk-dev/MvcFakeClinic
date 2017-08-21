using MedicalClinic.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalClinic.Models
{
    public class Treatment
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Treatment")]
        public string Name { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Currency)] //The DataType attribute specifies a more specific data type than the database intrinsic type. 
        [Column(TypeName = "money")] //set the database datatype to money
        public decimal Price { get; set; }

        //navigation properties
        public virtual Specialization Specialization { get; set; } //1-1 relationship
    }
}
