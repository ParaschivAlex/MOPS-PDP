using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using PDP.Models;

namespace PDP.Controllers
{
    public class ConsultationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Consultations
        [Authorize(Roles = "User, Admin")]
        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();

            IEnumerable<PDP.Models.Consultation> aux = db.Consultations.Where(t => t.User.Id == userId);
            List<Consultation> consultations = aux.ToList();
            return View(consultations);
        }

        // GET: Consultations/Create/{DoctorId}
        [Authorize(Roles = "User, Admin")]
        public ActionResult Create(int id)
        {
            var doctor = db.Doctors.Find(id);
            Consultation consultation = new Consultation();
            consultation.Doctor = doctor;
            consultation.DoctorId = doctor.DoctorId;

            string userId = User.Identity.GetUserId();
            consultation.User = db.Users.Single(t => t.Id == userId);
            consultation.UserId = userId;

            return View(consultation);
        }


        // POST: Consultations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User, Admin")]
        public ActionResult Create(Consultation consultation)
        {
            consultation.User = db.Users.Single(t => t.Id == consultation.UserId);
            consultation.Doctor = db.Doctors.Single(t => t.DoctorId == consultation.DoctorId);

            if (ModelState.IsValid)
            {
                consultation.canceled = true;
                db.Consultations.Add(consultation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            throw new Exception("INVALID");
        }

        // GET: Consulation/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Consultation consulations = db.Consultations.Find(id);
            if (consulations == null)
            {
                return HttpNotFound();
            }
            return View(consulations);
        }

        // POST: Consulations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(Consultation consultation)
        {
            consultation.User = db.Users.Single(t => t.Id == consultation.UserId);
            consultation.Doctor = db.Doctors.Single(t => t.DoctorId == consultation.DoctorId);

            if (ModelState.IsValid)
            {
                db.Entry(consultation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(consultation);
        }


        // GET: Consultations/Delete/5
        [Authorize(Roles = "User, Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Consultation consultation = db.Consultations.Find(id);
            if (consultation == null)
            {
                return HttpNotFound();
            }
            return View(consultation);
        }


        // POST: Consultations/Delete/{ConsultationID}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User, Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Consultation consultation = db.Consultations.Find(id);
            db.Consultations.Remove(consultation);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


    }
}
