using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalClinic.ViewModels
{
    public class AssignedDoctorData
    {
        public int SpecializationID { get; set; }
        public string Name { get; set; }
        public bool Assigned { get; set; }
        public string IconUrl { get; set; }
    }
}