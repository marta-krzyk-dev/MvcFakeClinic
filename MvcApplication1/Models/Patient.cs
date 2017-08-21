using MedicalClinic.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace MedicalClinic.Models
{
    public class Patient : Person
    {
        public DateTime BirthDate;

        public int Age
        {
            get
            {
                DateTime zeroTime = new DateTime(1, 1, 1);
                TimeSpan span = DateTime.Today - BirthDate;
                return (zeroTime + span).Year - 1;
            }
        }

        public virtual ICollection<Visit> Visits { get; set; }
    }
}
