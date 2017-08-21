using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalClinic.Models
{
    public class AddressSummary
    {
        public string City { get; set; }
        public string Country { get; set; }
        public bool InEurope { get; set; }
    }
}