using MedicalClinic.DAL;
using MedicalClinic.Models;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace MedicalClinic.Controllers
{
    public class SpecializationController : Controller
    {
        private ClinicContext db = new ClinicContext();

        //
        // GET: /Specializations/

        public ActionResult Index()
        {
            var list = db.Specializations.OrderBy(s => s.Name).ToList();
            return View(list);
        }

        //
        // GET: /Specializations/Details/5

        public ActionResult Details(int id = 0)
        {
            Specialization specialization = db.Specializations.Find(id);
            var doctors = specialization.Doctors;

            if (specialization == null)
            {
                return HttpNotFound();
            }
            return View(specialization);
        }

        //
        // GET: /Specializations/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Specializations/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Specialization specialization)
        {
            if (ModelState.IsValid)
            {
                db.Specializations.Add(specialization);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(specialization);
        }

        //
        // GET: /Specializations/Edit/5

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Specialization specialization = db.Specializations.Where(i => i.SpecializationId == id).Single();

            PopulateAssignedDoctorData(specialization);

            if (specialization == null)
            {
                return HttpNotFound();
            }

            return View(specialization);
        }

        private void PopulateAssignedDoctorData(Specialization specialization)
        {
            //var allCourses = db.Specializations;
            //var instructorCourses = new HashSet<int>(instructor.Courses.Select(c => c.CourseID));
            //var viewModel = new List<AssignedCourseData>();

            //foreach (var course in allCourses)
            //{
            //    viewModel.Add(new AssignedDoctorData {



            //CourseID = course.CourseID,             Title = course.Title,             Assigned = instructorCourses.Contains(course.CourseID)         });
            //}
            //ViewBag.Courses = viewModel;
        }

        //
        // POST: /Specializations/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Specialization specialization)
        {
            if (ModelState.IsValid)
            {
                db.Entry(specialization).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(specialization);
        }

        //
        // GET: /Specializations/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Specialization specialization = db.Specializations.Find(id);
            if (specialization == null)
            {
                return HttpNotFound();
            }
            return View(specialization);
        }

        //
        // POST: /Specializations/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Specialization specialization = db.Specializations.Find(id);
            db.Specializations.Remove(specialization);
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