using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalClinic.Models
{
    public class Visit
    {
        public int VisitId { get; set; }
        public DateTime VisitDate { get; set; }

        virtual public Patient Patient { get; set; }
        virtual public Doctor Doctor { get; set; }
    }
}