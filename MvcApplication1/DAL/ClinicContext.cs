using System.Data.Entity;
using MedicalClinic.Models;
using System.Collections.Generic;
using System;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;

namespace dummyNamespace
{
}

namespace MedicalClinic.DAL
{
    public class ClinicContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, add the following
        // code to the Application_Start method in your Global.asax file.
        // Note: this will destroy and re-create your database with every model change.
        // 
        // System.Data.Entity.Database.SetInitializer(new System.Data.Entity.DropCreateDatabaseIfModelChanges<MvcApplication1.Models.ClinicContext>());

        public ClinicContext() : base("ClinicContext") 
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //name tables with singular forms, e.g. Doctor not Doctors
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            //configure the many-to-many join table
            modelBuilder.Entity<Specialization>()
                .HasMany(s => s.Doctors).WithMany(d => d.Specializations)
                .Map(t => t.MapLeftKey("SpecializationID")
                    .MapRightKey("ID")
                    .ToTable("SpecializationDoctor"));
        }


        public DbSet<Person> People { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<Treatment> Treatments { get; set; }
        public DbSet<Visit> Visits { get; set; }
        public DbSet<Symptom> Symptoms { get; set; }
        public DbSet<Disease> Diseases { get; set; }
    }
}
