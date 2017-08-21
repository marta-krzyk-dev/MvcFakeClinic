using MedicalClinic.DAL;
using MedicalClinic.Models;
using MedicalClinic.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MedicalClinic.Controllers
{
    public class HomeController : Controller
    {
        private ClinicContext db = new ClinicContext();

        public ActionResult Index()
        {
            return View();
        }

        private IList<AssignedSymptomData> SeedAssignedSymptoms()
        {
            var query = db.Symptoms
            .AsEnumerable() //Convert IQueryable to IEnumerable
            .GroupBy(s => s.Category) //Create IGrouping<SymptomCategory,Symptom> groups
            .Select(group => group.OrderBy(s => s.Name))//For each group execute sorting
            .OrderBy(s => s.First().Category) //Sort groups by category name
            .SelectMany(group => group) //Select all the groups IOrderedEnumerable<Symptom> into one IEnumerable<Symptom>
            .ToList(); //Convert into a list

            //Convert symptoms into AssignedSymptomDatas
            var model = new List<AssignedSymptomData>();
            query.ForEach(s => model.Add(new AssignedSymptomData { SymptomId = s.SymptomId, Name = s.Name, Category = s.Category, Assigned = false }));

            return model;
        }

        //SIMPLIER VERSION using ICollection<>
        [HttpGet]
        public ActionResult Test()
        {
            var symptoms = SeedAssignedSymptoms();
            return View(symptoms); 
        }

        [HttpPost]
        public ActionResult Test(List<AssignedSymptomData> model)
        //Test results
        {
            if (null == model)
            {
                ModelState.AddModelError("", "Error occurred. Try again later.");
                return View("TestResult");
            }

            model.RemoveAll(s => s.Assigned == false); //Remove all the unchecked symptoms from the list

            var result = new List<ViewModelDisease>();
            var assignedSymptomsIds = from s in model
                                      select s.SymptomId;

            //Load other symptoms' names associated with the diseases
            foreach (var assignedSymptomData in model)
            {
                Symptom symptom = db.Symptoms.Find(assignedSymptomData.SymptomId);
                if (symptom == null)
                    continue;

                foreach (var d in symptom.Diseases)
                {
                    if (result.SingleOrDefault(x => x.DiseaseId == d.DiseaseId) != null)
                    {
                        //The disease is already in the result list
                        continue;
                    }
                    else
                    {
                        var newDisease = new ViewModelDisease { DiseaseId = d.DiseaseId, Name = d.Name };

                        var assignedSymptoms = d.Symptoms.Where(s => assignedSymptomsIds.Contains(s.SymptomId)).Select(s => s.Name).OrderBy(s => s);
                        var otherSymptoms = d.Symptoms.Where(s => !assignedSymptomsIds.Contains(s.SymptomId)).Select(s => s.Name).OrderBy(s => s);

                        newDisease.AssignedSymptoms.AddRange(assignedSymptoms);
                        newDisease.OtherSymptoms.AddRange(otherSymptoms); //Add Disease's symptoms' names as List<string>

                        result.Add(newDisease);
                    }
                }
            }

            return View("TestResult", result.OrderBy(d => d.Name).ToList()); //order by disease's name
        }
    }
}

