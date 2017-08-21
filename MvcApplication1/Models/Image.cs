using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedicalClinic.Models
{
    public class Image
    {
        [DisplayName("Image url")]
        public string Url { get; set; }

        [DisplayName("Accreditation url")]
  
        public string AccreditationUrl { get; set; }

        [DisplayName("Accreditation text")]
        [StringLength(200, ErrorMessage = "Maximum length is 200")]
        public string AccreditationText { get; set; }
    }
}