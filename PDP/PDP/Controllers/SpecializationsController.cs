using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PDP.Models;

namespace PDP.Controllers
{
    public class SpecializationsController : Controller
    {
        public ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var specializations = from spec in db.Specializations
                             orderby spec.Name
                             select spec;
            ViewBag.Specializations = specializations;
            //System.Diagnostics.Debug.WriteLine("----VIEWBAG SPECIALIZATIONS---");
            //System.Diagnostics.Debug.Write(ViewBag.Specializations);
            return View(specializations.ToList());

        }


        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Create(Specialization spec)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("----BEFORE ADDING---");
                db.Specializations.Add(spec);
                System.Diagnostics.Debug.WriteLine("-----BEFORE SAVING---");
                db.SaveChanges();
                System.Diagnostics.Debug.WriteLine("---BEFORE RETURN----");
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View(spec);
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            Specialization specialization = db.Specializations.Find(id);
            return View(specialization);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public ActionResult Edit(int id, Specialization requestSpecialization)
        {
            try
            {
                Specialization specialization = db.Specializations.Find(id);
                System.Diagnostics.Debug.WriteLine("---CAUTA PT EDIT----");
                    System.Diagnostics.Debug.WriteLine("---A FOST GASIT PT EDIT----");
                    specialization.Name = requestSpecialization.Name;
                    specialization.Price = requestSpecialization.Price;
                    db.SaveChanges();
                    System.Diagnostics.Debug.WriteLine("---A FOST EDITAT----");
                    TempData["message"] = "The specialization has been modified!";
                    return RedirectToAction("Index");

            }
            catch (Exception)
            {
                return View(requestSpecialization);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Specialization specialization = db.Specializations.Find(id);
            List<int> doctors_to_delete = db.Doctors.Where(fr => fr.SpecializationID == id).Select(f => f.DoctorId).ToList();

            foreach (var doc_del in doctors_to_delete)
            {
                Doctor doc = db.Doctors.Find(doc_del);
                db.Doctors.Remove(doc);
            }
            db.Specializations.Remove(specialization);
            TempData["message"] = "The specialization has been deleted!";
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}