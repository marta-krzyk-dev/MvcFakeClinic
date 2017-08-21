using MedicalClinic.DAL;
using MedicalClinic.Models;
using MedicalClinic.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication1.Controllers
{
    public class BindController : Controller
    {
        private ClinicContext db = new ClinicContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Names(IList<string> names)
        {
            names = names ?? new List<string>();
            return View(names);
        }

        public ActionResult AddressesSimple(IList<AddressSummary> addresses)
        {
            addresses = addresses ?? new List<AddressSummary>();
            return View(addresses);
        }

        public ActionResult Addresses(FormCollection formData)
        {
            var addresses = new List<AddressSummary>();
            try
            {
                UpdateModel(addresses, formData);
            }
            catch (InvalidOperationException ex)
            {
                ViewBag.Message = ex.Message;
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }

            return View(addresses);
        }

        public ActionResult Symptoms(IList<AssignedSymptomDiseaseData> assignedSymptoms)
        {
            assignedSymptoms = assignedSymptoms ?? SeedAssignedSymptoms();

            return View(assignedSymptoms);
        }

        private IList<AssignedSymptomDiseaseData> SeedAssignedSymptoms()
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public ActionResult SimplestTest()
        {
            //SOLUTION 1 - Query Expression 
            var dictionary = new Dictionary<SymptomCategory, List<AssignedSymptomDiseaseData>>();

            foreach (SymptomCategory cat in Enum.GetValues(typeof(SymptomCategory)))
            {
                var asdQuery = from s in db.Symptoms
                               where s.Category == cat
                               orderby s.Name
                               select new AssignedSymptomDiseaseData { SymptomId = s.SymptomId, Assigned = false, Name = s.Name };

                dictionary.Add(cat, asdQuery.ToList());
            }

            return View(dictionary);

            //var query = db.Symptoms
            //            .AsEnumerable() //Convert IQueryable to IEnumerable
            //            .GroupBy(s => s.Category) //Create IGrouping<SymptomCategory,Symptom> groups
            //            .Select(group => group.OrderBy(s => s.Name))//For each group execute sorting
            //            .OrderBy(s => s.First().Category) //Sort groups by category name
            //            .ToDictionary(group => group); //Convert into a list

            //return View(query);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SimplestTest(Dictionary<SymptomCategory, List<AssignedSymptomDiseaseData>> model)
        //Test results
        {
            if (null == model)
            {
                ModelState.AddModelError("", "Error occurred. Try again later.");
                return View("SimplestTestResult");
            }

            var assignedSymptomList = model.ToList();

            foreach (var item in assignedSymptomList)
            {
                ;
            }

            return View("SimplestTestResult");
        }


        public ActionResult CreateTest()
        {
            var query = db.Symptoms
                            .AsEnumerable() //Convert IQueryable to IEnumerable
                            .GroupBy(s => s.Category) //Create IGrouping<SymptomCategory,Symptom> groups
                            .Select(group => group.OrderBy(s => s.Name))//For each group execute sorting
                            .OrderBy(s => s.First().Category) //Sort groups by category name
                            .SelectMany(group => group) //Select all the groups IOrderedEnumerable<Symptom> into one IEnumerable<Symptom>
                            .ToList(); //Convert into a list

            //Convert symptoms into AssignedSymptomDatas
            var resultList = new List<AssignedSymptomDiseaseData>();
            query.ForEach(s => resultList.Add(new AssignedSymptomDiseaseData { SymptomId = s.SymptomId, Name = s.Name, Category = s.Category, Assigned = false }));

            return View(resultList.FirstOrDefault());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTest(AssignedSymptomDiseaseData asd)
        {
            return View(asd);
        }
        //[HttpPost]
        //    [ValidateAntiForgeryToken]
        //    public ActionResult Test(Dictionary<SymptomCategory, ICollection<AssignedSymptomDiseaseData>> symptomGroups) 
        //        //Test results
        //    {
        //        var result = new List<Symptom>(); //, ICollection<Disease>>();

        //        //Get possible diseases based on symptoms
        //        foreach (var group in symptomGroups.Values) //for each category of symptoms
        //        {
        //            foreach (var symptom in group) //get symptoms
        //            {
        //                if (symptom.Assigned)
        //                {
        //                    var s = db.Symptoms.Find(symptom.SymptomId);    //Find symptom in database based on AssignedSymptomDiseaseData.SymptomId
        //                    if (s == null)
        //                        break;

        //                    result.Add(s); //Save symptom to a result list
        //                }
        //            }
        //        }
        //        return View("TestResult", result);
        //    }


    }
}
