using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalClinic.Models
{
    public class Exercise
    {
        public int ExerciseId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string[] ImageFiles { get; set; }
    }
}