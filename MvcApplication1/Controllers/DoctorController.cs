using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MedicalClinic.Models;
using MedicalClinic.DAL;
using System.Data.Entity.Infrastructure;
using System.Net;
using MedicalClinic.ViewModels;

namespace MvcApplication1.Controllers
{
    public class DoctorController : Controller
    {
        private ClinicContext db = new ClinicContext();

        //
        // GET: /Doctor/

        public ActionResult Index(string sortOrder, string searchString, string paging, string SpecList)
        {
            IQueryable<Doctor> doctors;

            try
            {
                doctors = from d in db.Doctors
                          select d;
            }
            catch (Exception e)
            {
                ViewBag.Error = e.InnerException;
                return View();
            }

            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            //SEARCH A STRING IN DOCTORS
            if (!String.IsNullOrEmpty(searchString))
            {
                ViewBag.searchString = searchString;

                doctors = from d in db.Doctors
                          where d.Surname.ToUpper().Contains(searchString.ToUpper()) 
                          || d.Name.ToUpper().Contains(searchString.ToUpper())
                          select d;
            }

            ViewBag.SpecList = SpecList;

            if (!String.IsNullOrEmpty(SpecList) && SpecList != "Select all")
            {
                int specId;
                if (int.TryParse(SpecList, out specId))
                {
                    PopulateSpecializationDropDownList(specId);
                    Specialization spec = db.Specializations.Find(specId);
                    if (spec != null)
                    {
                        //doctors = doctors.Where(d => d.Specializations.FirstOrDefault(s => s.SpecializationId == specId) != null);
                        doctors = from d in doctors
                                  where d.Specializations.FirstOrDefault(s => s.SpecializationId == specId) != null
                                  select d;
                    }
                }
                else
                    PopulateSpecializationDropDownList();
            }
            else
                PopulateSpecializationDropDownList();

            //ORDER DOCTORS
            switch (sortOrder)
            {
                case "name_desc":
                    doctors = doctors.OrderByDescending(d => d.Surname); break;
                case "Date":
                    doctors = doctors.OrderBy(d => d.HireDate); break;
                case "date_desc":
                    doctors = doctors.OrderByDescending(d => d.HireDate); break;
                default:
                    doctors = doctors.OrderBy(d => d.Surname);  break;
            }

            return View(doctors.ToList());
           
        }

        private void PopulateDepartmentsDropDownList(object selectedDepartment = null)
        {
            var departmentsQuery = from d in db.Departments
                                   orderby d.City
                                   select d;

            ViewBag.ID = new SelectList(departmentsQuery, "ID", "City", selectedDepartment);

        }

        private void PopulateSpecializationDropDownList(object selectedSpecialization = null)
        {
            var specializationQuery = from s in db.Specializations
                                      orderby s.DoctorName
                                      select s;

            ViewBag.SpecializationList = new SelectList(specializationQuery, "SpecializationId", "DoctorName", selectedSpecialization);

            //var specList = new SelectList(specializationQuery, "SpecializationId", "DoctorName", selectedSpecialization);

            //var items = specList.ToList();
            //items.Insert(0, (new SelectListItem { Text = "[None]", Value = "-1" }));

            //items.Select(x =>
            //                      new SelectListItem()
            //                      {
            //                          Text = x.ToString(),
            //                          Value = x.Value
            //                      }); ;

            //ViewBag.SpecializationList = items.AsEnumerable();
        }

        public ActionResult Details(int id = 0)
        {
            Doctor doctor = db.Doctors.Find(id);
            if (doctor == null)
            {
                return HttpNotFound();
            }

            return View(doctor);
        }

        //
        // GET: /Doctor/Create

        public ActionResult Create()
        {
            var doctor = new Doctor();
            doctor.Specializations = new List<Specialization>();

            PopulateAssignedDoctorData(doctor);
            PopulateDepartmentsDropDownList();

            return View(doctor);
        }



        //
        // POST: /Doctor/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Surname,Email,HireDate,ImageUrl,DepartmentID")]Doctor doctor, string selectedSpecializations)
        {
            try
            {
                //specializations are added before model validation, os in case of an error, selected specializations are restored
                if (selectedSpecializations != null)
                {
                    doctor.Specializations = new List<Specialization>();
                    foreach (var s in selectedSpecializations)
                    {
                        var specToAdd = db.Specializations.Find(int.Parse(s.ToString()));
                        doctor.Specializations.Add(specToAdd);
                    }
                }
                if (ModelState.IsValid)
                {
                    db.Doctors.Add(doctor);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception/*RetryLimitExceededException / dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }

            PopulateAssignedDoctorData(doctor); // provide an empty collection for the foreach loop in the view
            PopulateDepartmentsDropDownList(doctor.DepartmentID);
            return View(doctor);
        }
        //
        // GET: /Doctor/Edit/5

        public ActionResult Edit(int? id)
        {
            if (id == null) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }

            //Find doctor in database
            Doctor doctor = db.Doctors.Include(d => d.Specializations).Include(d => d.Department).Where(d => d.ID == id).Single();

            PopulateAssignedDoctorData(doctor);
            PopulateDepartmentsDropDownList(doctor.DepartmentID);
            if (doctor == null)
            {
                return HttpNotFound();
            }
            
            return View(doctor);
        }

        private void PopulateAssignedDoctorData(Doctor doctor)
        {
            var allSpecializations = db.Specializations.OrderBy(s => s.Name);

            //get ids of doctor's specializations
            var doctorSpecializations = new HashSet<int>(doctor.Specializations.Select(s => s.SpecializationId));

            var viewModel = new List<AssignedDoctorData>();

            foreach (var s in allSpecializations)
            {
                viewModel.Add(new AssignedDoctorData{
                        SpecializationID = s.SpecializationId,
                        Name = s.Name,
                        Assigned = doctorSpecializations.Contains(s.SpecializationId),//bool defines if doctor has this specialization
                        IconUrl = s.IconUrl
                    } 
                );
            }

            ViewBag.Specializations = viewModel; //pass list of view models to the view in a ViewBag property
        }
    

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, string[] selectedSpecializations)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var doctorToUpdate = db.Doctors.Include(d => d.Specializations).Where(d => d.ID == id).Single();

            if (TryUpdateModel(doctorToUpdate, "",
               new string[] { "Name", "Surname", "Email", "HireDate", "ImageUrl", "DepartmentID" }))
            {
                try
                {
                    UpdateDoctorSpecializations(selectedSpecializations, doctorToUpdate);

                    db.Entry(doctorToUpdate).State = EntityState.Modified;

                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (Exception/*RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
                
            }
            PopulateDepartmentsDropDownList(doctorToUpdate.DepartmentID);
                PopulateAssignedDoctorData(doctorToUpdate);

            return View(doctorToUpdate);
        }

        private void UpdateDoctorSpecializations(string[] selectedSpecializations, Doctor doctorToUpdate)
        {
            //user didn't select any check boxes
            if (selectedSpecializations == null) { 
                doctorToUpdate.Specializations = new List<Specialization>();
                return;
            }

            var selectedSpecializationsHS = new HashSet<string>(selectedSpecializations); //copy list into a hashset for optimalized search
            var doctorSpecializations = new HashSet<int>(doctorToUpdate.Specializations.Select(s=> s.SpecializationId)); // copy all ids of doctor's specialization into a hashset

            foreach (var s in db.Specializations)
            {
                if (selectedSpecializationsHS.Contains(s.SpecializationId.ToString()))
                {
                    //the doctor has that specialization
                    if (false == doctorSpecializations.Contains(s.SpecializationId))
                    //the specialization in not yet in doctor's list, user wants to add a specialization
                    {
                        doctorToUpdate.Specializations.Add(s);
                    }
                    //otherwise, the specialization is already in doctor's list
                }
                else
                {
                    if (doctorSpecializations.Contains(s.SpecializationId)) //the specialization was deselected by user, user wants to delete it from doctor's list
                    {
                        doctorToUpdate.Specializations.Remove(s);
                    }
                }
            }
        }


        //
        // GET: /Doctor/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Doctor doctor = db.Doctors.Find(id);
            if (doctor == null)
            {
                return HttpNotFound();
            }
            return View(doctor);
        }

        //
        // POST: /Doctor/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Doctor doctor = db.Doctors.Find(id);

            db.People.Remove(doctor);
           
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}