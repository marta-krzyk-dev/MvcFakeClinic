using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalClinic.Models
{
    public enum Role { Doctor, Nurse, Secretary, OfficeWorker, Cleaner, Security };

    public class Employee : Person
    {
        public Role? Role { get; set; }

        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        //[Display(Name = "Hire Date")]
        //public DateTime HireDate { get; set; }

        [DataType(DataType.Currency)] //The DataType attribute specifies a more specific data type than the database intrinsic type. 
        [Column(TypeName = "money")] //set the database datatype to money
        public decimal? Payment { get; set; }

        //public virtual Department Department { get; set; } // 1-many relationship
    }
}