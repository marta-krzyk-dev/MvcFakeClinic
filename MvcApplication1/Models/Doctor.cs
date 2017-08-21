using MedicalClinic.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedicalClinic.Models
{
    public class Doctor : Person
    {
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Hire date")]
        public DateTime HireDate { get; set; } = DateTime.Now;

        [Display(Name = "Years of experience")]
        public int? YearsOfExperienceInt {
                                            get
                                            {
                                                DateTime zeroTime = new DateTime(1, 1, 1);
                                                TimeSpan span = DateTime.Today - HireDate;
                                                return (zeroTime + span).Year - 1;
                                            }
                                        }

        [Display(Name = "Years of experience")]
        public string YearsOfExperience
        {
            get
            {
                if (YearsOfExperienceInt == 1)
                    return "1 year of experience";
                else
                    return YearsOfExperienceInt.ToString() + " years of experience";
            }
        }

        public Image Image { get; set; } = new Image();

        [DisplayName("Image url")]
        public string ImageUrl { get; set; }

        [DisplayName("Accreditation url")]
        [StringLength(200, ErrorMessage = "Maximum length is 200")]
        public string AccreditationUrl { get; set; }

        [DisplayName("Accreditation text")]
        public string AccreditationText { get; set; }

        public virtual ICollection<Specialization> Specializations { get; set; } //many-many relationship

        public string SpecializationsHeader {
            get {
                if (Specializations == null || Specializations.Count > 1)
                    return "Specializations";
                else
                    return "Specialization";
            }
        }

        public int DepartmentID { get; set; }

        public virtual Department Department { get; set; } // 1-many relationship
    }
}