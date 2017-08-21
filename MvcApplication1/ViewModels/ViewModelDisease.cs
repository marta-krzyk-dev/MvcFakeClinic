using MedicalClinic.Models;
using System.Collections.Generic;

namespace MedicalClinic.ViewModels
{
    public class ViewModelDisease
    {
        public int DiseaseId { get; set; }
        public string Name { get; set; }

        public List<string> OtherSymptoms { get; set; } = new List<string>(); //the other symptom of the disease that user didn't check during the test
        public List<string> AssignedSymptoms { get; set; } = new List<string>(); //the symptoms the user checked in the test
    }
}