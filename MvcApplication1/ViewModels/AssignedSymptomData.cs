using MedicalClinic.Models;
using System.Collections.Generic;

namespace MedicalClinic.ViewModels
{
    //Class to help save data on symptom user checked in Test
    public class AssignedSymptomData
    {
        public int SymptomId { get; set; }
        public string Name { get; set; }
        public SymptomCategory Category { get; set; }
        public bool Assigned { get; set; }
    }

    public class AssignedSymptomDiseaseData
    {
        public int SymptomId { get; set; }
        public string Name { get; set; }
        public SymptomCategory Category { get; set; }
        public List<ViewModelDisease> Diseases { get; set; } = new List<ViewModelDisease>();
        public bool Assigned { get; set; }
    }
}