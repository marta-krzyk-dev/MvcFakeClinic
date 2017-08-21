namespace MedicalClinic.Migrations
{
    using MedicalClinic.DAL;
    using MedicalClinic.Models;
    
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Validation;
    using System.Linq;

    // This class allows you to configure how Migrations behaves for your context. 
    internal sealed class Configuration : DbMigrationsConfiguration<MedicalClinic.DAL.ClinicContext>
    {
        private MedicalClinic.DAL.ClinicContext context;

        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        private void WriteExceptionToTxtFile(Exception e)
        {
            
            foreach (var eve in e.Data)
            {
                Console.WriteLine("Exception data: \"{0}\"",
                    eve.ToString());

               
                    System.IO.File.WriteAllLines(@"C:\Users\Marta\Desktop\EntityValidationErrors.txt", new string[] { DateTime.Now.ToString(), "Type: ", eve.GetType().ToString()});
            };

        }

        private void WriteExceptionToTxtFile(DbEntityValidationException e)
        {
            foreach (var eve in e.EntityValidationErrors)
            {
                Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                    eve.Entry.Entity.GetType().Name, eve.Entry.State);
                foreach (var ve in eve.ValidationErrors)
                {
                    System.IO.File.WriteAllLines(@"C:\Users\Marta\Desktop\EntityValidationErrors.txt", new string[] { DateTime.Now.ToString(), "Property: ", ve.PropertyName, "Error: ", ve.ErrorMessage });
                }
            };

        }

    protected override void Seed(MedicalClinic.DAL.ClinicContext context)
        {
            this.context = context; //Use a private object to speed up work in MatchDiseaseSpecSymptoms

            try
            {
                SeedSpecializations(context);
                SeedDepartments(context);
                SeedDoctors(context);
                SeedSymptoms(context);
                SeedDiseases(context);
                SeedDiseasesWithSymptoms(context);
            }
            catch (DbEntityValidationException e)
            {
                WriteExceptionToTxtFile(e);
            }
            finally
            {
                context.SaveChanges();
            }
           

            //Connect doctors to specializations
            //AddOrUpdateDoctor(context, "Immunologist", "Vasquez");
            //AddOrUpdateDoctor(context, "Audiologist", "Rock");
            //AddOrUpdateDoctor(context, "Audiologist", "Denhen");
            //AddOrUpdateDoctor(context, "Anesthesiologist", "Miguel");
            //AddOrUpdateDoctor(context, "Anesthesiologist", "Inouye");
            //AddOrUpdateDoctor(context, "Cardiologist", "Inouye");
            //AddOrUpdateDoctor(context, "Cardiologist", "Allred");
            //AddOrUpdateDoctor(context, "Dermatologist", "Rock");
            //AddOrUpdateDoctor(context, "Endocrinologist", "Miguel");
            //AddOrUpdateDoctor(context, , );
            //AddOrUpdateDoctor(context, , );
            //AddOrUpdateDoctor(context, , ); 
            
           
            }

        private void SeedDiseasesWithSymptoms(ClinicContext context)
        {
            MatchDiseaseSpecSymptoms("Sprain", "General", "Ankle ache", "Muscle pain");
            MatchDiseaseSpecSymptoms("Osteoarthritis", "General", "Joint pain", "Snapping hip", "Snapping knee");
            MatchDiseaseSpecSymptoms("Influenza", "General", "Sore throat", "Cold", "Cough");
            MatchDiseaseSpecSymptoms("Smallpox", "General", "Rash", "Itching");
            MatchDiseaseSpecSymptoms("Sunstroke", "General", "Headache", "Insomnia", "Itching", "Skin redness", "Flaking skin", "Blisters");
            MatchDiseaseSpecSymptoms("Allergy", "Dermatology", "Rash", "Itching", "Skin redness");
            MatchDiseaseSpecSymptoms("Food poisoning", "General", "Stomach ache", "Diarrea", "Fever");
            MatchDiseaseSpecSymptoms("Otitis", "Audiology", "Earache");
            MatchDiseaseSpecSymptoms("Neurosis", "General", "Nervousness", "Anxiety", "Depression", "Agitation");
            MatchDiseaseSpecSymptoms("Anemia", "General", "Paleness", "Fatigue");
            MatchDiseaseSpecSymptoms("Sunburn", "Dermatology", "Flaking skin", "Skin redness");
            MatchDiseaseSpecSymptoms("Eczema", "Dermatology", "Flaking skin", "Blisters");
            MatchDiseaseSpecSymptoms("Acne", "Dermatology", "Whiteheads", "Blackheads", "Cysts", "Spots and pimples");
            context.SaveChanges();
        }

        private void MatchDiseaseSpecSymptoms(string diseaseName, string specializationName, params string[] symptoms)
        {
            try
            {
                Disease diseaseToUpdate = this.context.Diseases.Where(d => d.Name.Contains(diseaseName)).FirstOrDefault();

                if (diseaseToUpdate == null) //Disease not found, end the function
                    return;

                diseaseToUpdate.Specialization = context.Specializations.Where(s => s.Name.Contains(specializationName)).FirstOrDefault(); //return specialization or null

                diseaseToUpdate.Symptoms = new List<Symptom>();

                Symptom newSymptom = new Symptom();

                foreach (var symptomName in symptoms)
                {
                    newSymptom = context.Symptoms.Where(s => s.Name == symptomName).FirstOrDefault();

                    if (newSymptom != null)
                    {
                        diseaseToUpdate.Symptoms.Add(newSymptom); //Add symptom to Diseases's collection
                        newSymptom.Diseases.Add(diseaseToUpdate);//Add disease to Symptom's collection
                    }
                }
            }
            catch (Exception e)
            {
                WriteExceptionToTxtFile(e);
            }
            
        }

        private void SeedDiseases(ClinicContext context)
        {
            context.Diseases.AddOrUpdate(
                d => d.Name,

                new Disease { Name = "Sprain" },
                new Disease { Name = "Osteoarthritis" },
                new Disease { Name = "Influenza" },
                new Disease { Name = "Smallpox" },
                new Disease { Name = "Sunstroke" },
                new Disease { Name = "Allergy" },
                new Disease { Name = "Food poisoning"},
                new Disease { Name = "Otitis" },
                new Disease { Name = "Acne" },
                new Disease { Name = "Neurosis" },
                new Disease { Name = "Anemia" },
                new Disease { Name = "Sunburn" },
                new Disease { Name = "Eczema" },
                new Disease { Name = "Acne" }
                //new Disease { Name = "" },

                );

            context.SaveChanges();
        }

        private void SeedSymptoms(ClinicContext context)
        {
            context.Symptoms.AddOrUpdate(

                  s => s.Name,
                  new Symptom { Name = "Headache", Category = SymptomCategory.Pains },
                  new Symptom { Name = "Ankle ache", Category = SymptomCategory.Pains },
                  new Symptom { Name = "Earache", Category = SymptomCategory.Pains },
                  new Symptom { Name = "Stomach ache", Category = SymptomCategory.Pains },
                  new Symptom { Name = "Joint pain", Category = SymptomCategory.Pains },
                  new Symptom { Name = "Muscle pain", Category = SymptomCategory.Pains },

                  new Symptom { Name = "Snapping hip", Category = SymptomCategory.Pains },
                  new Symptom { Name = "Snapping knee", Category = SymptomCategory.Pains }, 
                  
                  new Symptom { Name = "Cold", Category = SymptomCategory.Viral },
                  new Symptom { Name = "Sore throat", Category = SymptomCategory.Viral },
                  new Symptom { Name = "Diarrea", Category = SymptomCategory.Viral },
                  new Symptom { Name = "Cough", Category = SymptomCategory.Viral },
                  new Symptom { Name = "Fever", Category = SymptomCategory.Viral },

                  new Symptom { Name = "Rash", Category = SymptomCategory.Dermatological },
                  new Symptom { Name = "Itching", Category = SymptomCategory.Dermatological },
                  new Symptom { Name = "Skin redness", Category = SymptomCategory.Dermatological },
                  new Symptom { Name = "Whiteheads", Category = SymptomCategory.Dermatological },
                  new Symptom { Name = "Blackheads", Category = SymptomCategory.Dermatological },
                  new Symptom { Name = "Cysts", Category = SymptomCategory.Dermatological },
                  new Symptom { Name = "Blisters", Category = SymptomCategory.Dermatological },
                  new Symptom { Name = "Flaking skin", Category = SymptomCategory.Dermatological },
                  new Symptom { Name = "Paleness", Category = SymptomCategory.Dermatological },
                  new Symptom { Name = "Spots and pimples", Category = SymptomCategory.Dermatological },

                  new Symptom { Name = "Insomnia", Category = SymptomCategory.Emotions },
                  new Symptom { Name = "Nervousness", Category = SymptomCategory.Emotions },
                  new Symptom { Name = "Anxiety", Category = SymptomCategory.Emotions },
                  new Symptom { Name = "Depression", Category = SymptomCategory.Emotions },
                  new Symptom { Name = "Fatigue", Category = SymptomCategory.Emotions },
                  new Symptom { Name = "Agitation", Category = SymptomCategory.Emotions }
                  );

            context.SaveChanges();
        }

        private void SeedDepartments(ClinicContext context)
        {
            //context.Departments.Add(



            //    new Department
            //    {
            //        Address = new Address { City = "Oregon", PostalCode = "45-415", Street = "Hood River", HouseNumber = "A45" },
            //        Doctors = new List<Doctor>(),
            //        PhoneNumber = "123444555",
            //        ImageUrl = "/Content/Images/Departments/dep1.jpg"
            //    });

            //context.Departments.Add(
            //    new Department { Address = new Address { City = "Conrodo", PostalCode = "66-000", Street = "Muchasanso Square", HouseNumber = "F242" }, Doctors = new List<Doctor>(), PhoneNumber = "800999555", ImageUrl = "/Content/Images/Departments/dep2.jpg" });

            //context.Departments.Add(
            //    new Department { Address = new Address { City = "Franklinton", PostalCode = "20-200", Street = "North Bend", HouseNumber = "34T" }, Doctors = new List<Doctor>(), PhoneNumber = "123440555", ImageUrl = "/Content/Images/Departments/dep3.jpg" });

            context.Departments.AddOrUpdate(

                   d => d.PhoneNumber,   // Use Name (or some other unique field) instead of Id

                 new Department
                 {
                     City = "Oregon", PostalCode = "45-415", Street = "Hood River", HouseNumber = "A45",
                     Doctors = new List<Doctor>(),
                     PhoneNumber = "123444555",
                     Email="oregon@clinic.com",
                     ImageUrl = "/Content/Images/Departments/dep1.jpg"
                 },

                 new Department { City = "Conrodo", PostalCode = "66-000", Street = "Muchasanso Square", HouseNumber = "F242", Doctors = new List<Doctor>(), PhoneNumber = "800999555", Email = "conrodo@clinic.com", ImageUrl = "/Content/Images/Departments/dep2.jpg" },

                 new Department { City = "Franklinton", PostalCode = "20-200", Street = "North Bend", HouseNumber = "34T", Doctors = new List<Doctor>(), PhoneNumber = "123440555", Email = "franklinton@clinic.com", ImageUrl = "/Content/Images/Departments/dep3.jpg" });

            context.SaveChanges();

            // new Department { City = "", PostalCode = "", Street = "", HouseNumber = "" , Doctors = new List<Doctor>(), PhoneNumber = "123444555" }
        }

        private void SeedDoctors(ClinicContext context)
        {
            context.Doctors.AddOrUpdate(

                    d => d.Surname,   // Use Name (or some other unique field) instead of Id

                 new Doctor { Surname = "Allright", Name = "Andrew", HireDate = new DateTime(2013, 01, 04),
                    Specializations = context.Specializations.Where(s => s.Name.Contains("Cardio")).ToList(),
                    ImageUrl = "/Content/Images/Employees/doctor14.jpg",
                    AccreditationUrl = @"http://www.freepik.com/free-photo/doctor-smiling-with-stethoscope_916503.htm",
                     AccreditationText="Image created by Kues - Freepik.com",
                    DepartmentID = context.Departments.SingleOrDefault(s => s.City == "Oregon").ID,
                    Email="allright.adrew@fakeclinic.com"},

                new Doctor { Surname="Denhen", Name = "Maria", HireDate = new DateTime(2014,05,13),
                    Specializations = context.Specializations.Where(s => s.Name.Contains("Immu")).ToList(),
                    ImageUrl = "/Content/Images/Employees/doctor15.jpg",
                    DepartmentID = context.Departments.SingleOrDefault(s => s.City == "Oregon").ID,
                    Email = "maria.denhen@fakeclinic.com"},

                new Doctor { Surname="Miguella", Name = "Exie", HireDate = new DateTime(2015,01,12),
                    Specializations = context.Specializations.Where(s => s.Name.Contains("Immu")).ToList(),
                    ImageUrl = "/Content/Images/Employees/doctor16.jpg",
                    AccreditationUrl = @"http://www.freepik.com/free-photo/portrait-of-confident-young-medical-doctor-holding-a-clipboard_1162633.htm",
                    AccreditationText= "Image created by Dragana_Gordic - Freepik.com",
                    DepartmentID = context.Departments.SingleOrDefault(s => s.City == "Conrodo").ID,
                    Email = "miguel.exie@fakeclinic.com"
                },
                new Doctor { Surname="Johnson", Name = "Paul", HireDate = new DateTime(2012,11,12),
                    Specializations = context.Specializations.Where(s => s.Name.Contains("General")).ToList(),
                    ImageUrl = "/Content/Images/Employees/doctor17.jpg",
                    AccreditationUrl= @"http://www.freepik.com/free-photo/doctor-standing-with-a-folder-and-a-stethoscope_998781.htm",
                    AccreditationText = "Image created by Iaros - Freepik.com",
                    DepartmentID = context.Departments.SingleOrDefault(s => s.City == "Conrodo").ID,
                    Email = "paul.johnson@fakeclinic.com"
                },
                new Doctor { Surname="Allred", Name = "Terry", HireDate = new DateTime(2013,09,13),
                    Specializations = context.Specializations.Where(s => s.Name.Contains("Anesthesiology") || s.Name.Contains("General")).ToList(),
                    ImageUrl = "/Content/Images/Employees/doctor18.jpg",
                    AccreditationUrl=@"http://www.freepik.com/free-photo/doctor-presenting-something-over-isolated-white-background_1197468.htm",
                    AccreditationText = "Image created by Luis_molinero - Freepik.com",
                    DepartmentID = context.Departments.SingleOrDefault(s => s.City == "Conrodo").ID,
                    Email = "terry.allred@fakeclinic.com"
                },

                new Doctor { Surname="Inouye", Name = "Leon", HireDate = new DateTime(2010,08,30),
                    Specializations = context.Specializations.Where(s => s.Name.Contains("Audio")).ToList(),
                    ImageUrl = "/Content/Images/Employees/doctor19.jpg",
                    AccreditationUrl = @"http://www.freepik.com/free-photo/race-women-looking-young-horizontal-standing_1240915.htm",
                    AccreditationText = "Image created by Iaros - Freepik.com",
                    DepartmentID = context.Departments.SingleOrDefault(s => s.City == "Franklinton").ID,
                    Email = "leon.inouye@fakeclinic.com"
                },

                new Doctor { Surname="Vasquez", Name = "Ann", HireDate = new DateTime(2010,10,27),
                    Specializations = context.Specializations.Where(s => s.Name.Contains("Dermatology")).ToList(),
                    ImageUrl = "/Content/Images/Employees/doctor20.jpg",
                    AccreditationText = "Image created by Iaros - Freepik.com",
                    AccreditationUrl = @"http://www.freepik.com/free-photo/health-standing-smiling-looking-surgeon-men_1241048.htm",
                    DepartmentID = context.Departments.SingleOrDefault(s => s.City.Contains("Franklinton")).ID,
                    Email = "ann.vasquez@fakeclinic.com"
                },

                new Doctor
                {
                    Surname = "Webber",
                    Name = "Peter",
                    HireDate = new DateTime(2012, 01, 04),
                    Specializations = context.Specializations.Where(s => s.Name.Contains("Neuro")).ToList(),
                    ImageUrl = "/Content/Images/Employees/doctor8.jpg",
                    AccreditationUrl = @"http://www.freepik.com/free-photo/doctor-tablet-s-screen-and-copy-space_1210139.htm",
                    AccreditationText = "Image created by Freepik",
                    DepartmentID = context.Departments.SingleOrDefault(s => s.City == "Oregon").ID,
                    Email = "peter.webber@fakeclinic.com"
                },

                new Doctor
                {
                    Surname = "Squaredrop",
                    Name = "Caroline",
                    HireDate = new DateTime(2010, 11, 10),
                    Specializations = context.Specializations.Where(s => s.Name.Contains("Endocrinology")).ToList(),
                    ImageUrl = "/Content/Images/Employees/doctor4.jpg",
                    AccreditationUrl = @"http://www.freepik.com/free-photo/female-doctor-portrait_1198055.htm",
                    AccreditationText="Image created by Freepik",
                    DepartmentID = context.Departments.SingleOrDefault(s => s.City == "Franklinton").ID,
                    Email = "caroline.squaredrop@fakeclinic.com"
                },

                new Doctor
                {
                    Surname = "Reed",
                    Name = "Frances",
                    HireDate = new DateTime(2008, 02, 10),
                    Specializations = context.Specializations.Where(s => s.Name.Contains("Dent")).ToList(),
                    ImageUrl = "/Content/Images/Employees/doctor5.jpg",
                    DepartmentID = context.Departments.SingleOrDefault(s => s.City == "Conrodo").ID,
                    Email = "frances.reed@fakeclinic.com"
                },

                new Doctor
                {
                    Surname = "Wright",
                    Name = "Rebecca",
                    HireDate = new DateTime(2004, 02, 10),
                    Specializations = context.Specializations.Where(s => s.Name.Contains("Dent")).ToList(),
                    ImageUrl = "/Content/Images/Employees/doctor6.jpg",
                    DepartmentID = context.Departments.SingleOrDefault(s => s.City == "Conrodo").ID,
                    Email = "rebecca.wright@fakeclinic.com"
                },

                new Doctor
                {
                    Surname = "Malice",
                    Name = "Stanislava",
                    HireDate = new DateTime(2008, 02, 10),
                    Specializations = context.Specializations.Where(s => s.Name.Contains("Endo")).ToList(),
                    ImageUrl = "/Content/Images/Employees/doctor7.jpg",
                    DepartmentID = context.Departments.SingleOrDefault(s => s.City == "Franklinton").ID,
                    Email = "stanislava.malice@fakeclinic.com"
                }

        );

            foreach (var d in context.Doctors)
            {
                AddOrUpdateDoctorDepartment(context, d.ID, d.DepartmentID);
            }

            context.SaveChanges();
        }

        private void SeedSpecializations(ClinicContext context)
        {
            
                context.Specializations.AddOrUpdate( 
                    
                    s => s.Name,   // Use Name (or some other unique field) instead of Id
      
               new Specialization {
                   Name = "Audiology", DoctorName = "Audiologist", Description ="Audiologists specialize in ear related issues, particularly with regard to hearing loss in children. These doctors work with deaf and mute children to assist in their learning to communicate.",
                 IconUrl = "/Content/Images/Specializations/Icons/ear_icon.png",
                 ImageUrl = "/Content/Images/Specializations/BigImages/audiology.jpg",
                 Doctors = new List<Doctor>() },

               new Specialization {Name ="Anesthesiology",
                   DoctorName = "Anesthesiologist", Description ="Anesthesiologists study the effects and reactions to anesthetic medicines and administer them to a variety of patients with pain-killing needs. They assess illnesses that require this type of treatment and the dosages appropriate for each specific situation.",
                   IconUrl = "/Content/Images/Specializations/Icons/anesto_icon.ico",
                   ImageUrl = "/Content/Images/Specializations/BigImages/anesthesiology.jpeg",
                   Doctors = new List<Doctor>()},

               new Specialization {Name ="Cardiology",
                   DoctorName = "Cardiologist",
                   Description ="Cardiologists specify in the study and treatment of the heart and the many diseases and issues related to it. They assess the medical and family history of patients to determine potential risk for certain cardiovascular diseases and take action to prevent them.",
                   IconUrl = "/Content/Images/Specializations/Icons/cardio_icon.png",
                   ImageUrl = "/Content/Images/Specializations/BigImages/cardiology.jpg",
                   Doctors = new List<Doctor>()},

               new Specialization {Name ="Dermatology",
                   DoctorName = "Dermatologist", Description ="Dermatologists study skin and the structures, functions and diseases related to it. They examine patients to check for such risk factors as basal cell carcinoma (which signals skin cancer) and moles that may eventually cause skin disease if not treated in time.",
                 IconUrl = "/Content/Images/Specializations/Icons/derma_icon.png",
                 ImageUrl = "/Content/Images/Specializations/BigImages/dermatology.jpg",
                   Doctors = new List<Doctor>() },

               new Specialization {
                   Name ="Endocrinology", DoctorName = "Endocrinologist",
                   Description ="Endocrinologists specify in illnesses and issues related to the endocrine system and its glands. They study hormone levels in this area to determine and predict whether or not a patient will encounter an endocrine system issue in the future.",
                   IconUrl = "/Content/Images/Specializations/Icons/lab_icon.png",
                   ImageUrl = "/Content/Images/Specializations/BigImages/endocrinology.jpg",
               },

               new Specialization {Name ="Immunology",
                   DoctorName = "Immunologist", Description ="Immunologists study the immune system in a variety of organisms, including humans. They determine the weaknesses related to this system and what can be done to override these weaknesses.",
                 IconUrl = "/Content/Images/Specializations/Icons/immunity_icon.png",
                 ImageUrl = "/Content/Images/Specializations/BigImages/immumology.jpg",
                   Doctors = new List<Doctor>()},

                new Specialization
                {
                    Name = "General medicine",
                    DoctorName = "General practitioner",
                    Description = "A general practitioner, also called a GP or generalist, is a physician who does not specialize in one particular area of medicine.",
                    IconUrl = "/Content/Images/Specializations/Icons/pill_icon.png",
                    ImageUrl = "/Content/Images/Specializations/BigImages/general.jpg",
                    Doctors = new List<Doctor>()
                },

                new Specialization
                {
                    Name = "Neurology",
                    DoctorName = "Neurologist",
                    Description = "Neurology is a branch of medicine dealing with disorders of the nervous system. Neurology deals with the diagnosis and treatment of all categories of conditions and disease involving the central and peripheral nervous system (and its subdivisions, the autonomic nervous system and the somatic nervous system); including their coverings, blood vessels, and all effector tissue, such as muscle. Neurological practice relies heavily on the field of neuroscience, which is the scientific study of the nervous system.",
                    IconUrl = "/Content/Images/Specializations/Icons/brain_icon.png",
                    ImageUrl = "/Content/Images/Specializations/BigImages/neurology.jpg",
                    Doctors = new List<Doctor>()
                },

                  new Specialization
                  {
                      Name = "Dentistry",
                      DoctorName = "Dentist",
                      Description = "Dentists work with the human mouth, examining teeth and gum health and preventing and detecting various different issues, such as cavities and bleeding gums. Typically, patients are advised to go to the dentist twice a year in order to maintain tooth health. Dentistry is important to one's overall health. Dental treatments are carried out by the dental team, which often consists of a dentist and dental auxiliaries (dental assistants, dental hygienists, dental technicians, as well as dental therapists). Most dentists either work in private practices (primary care), dental hospitals or (secondary care) institutions (prisons, armed forces bases, etc.).",
                      IconUrl = "/Content/Images/Specializations/Icons/dental_icon.png",
                      ImageUrl = "/Content/Images/Specializations/BigImages/dentistry.jpeg",
                      Doctors = new List<Doctor>()
                  }

           // new Specialization {Name ="", Description=""},
           );

            context.SaveChanges();
        }

        void AddOrUpdateDoctor(ClinicContext context, string specialization, string doctorSurname)
        {
            var spec = context.Specializations.FirstOrDefault(c => c.Name == specialization);
            if (spec == null)
                return;
            var doc = spec.Doctors.FirstOrDefault(d => d.Surname == doctorSurname);
            if (doc == null)
                spec.Doctors.Add(context.Doctors.FirstOrDefault(d => d.Surname == doctorSurname)); 
        }

        void AddOrUpdateDoctorDepartment(ClinicContext context, int doctorID, int departmentID)
        {
            var doctor = context.Doctors.Find(doctorID);
            if (doctor == null)
                return;
            var dep = context.Departments.Find(departmentID);
            if (dep == null)
                return;

            dep.Doctors.Add(doctor);
        }
    }
}
