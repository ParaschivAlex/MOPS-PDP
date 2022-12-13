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
            consultation.price = doctor.CalcultateConsultationPrice(db.Specializations.Find(doctor.SpecializationID));

            string userId = User.Identity.GetUserId();
            consultation.User = db.Users.Single(t => t.Id == userId);
            consultation.UserId = userId;

            return View(consultation);
        }

        private bool IsTimeSlotAvailable(Consultation consultation)
        {
            var consultations = from com in db.Consultations
                                where com.DoctorId == consultation.DoctorId &&
                                com.date_day == consultation.date_day &&
                                com.slot_hour == consultation.slot_hour
                                select com;

            if (consultations.Any())
                return false;
            else
                return true;
        }

        // POST: Consultations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User, Admin")]
        public ActionResult Create(Consultation consultation)
        {
            consultation.User = db.Users.Single(t => t.Id == consultation.UserId);
            consultation.Doctor = db.Doctors.Single(t => t.DoctorId == consultation.DoctorId);

            if (!ModelState.IsValid)
            {
                ViewBag.ConsultationCreateError = "Model state is not valid...";
                return View(consultation);
            }

            if (consultation.date_day <= System.DateTime.Today)
            {
                ViewBag.ConsultationCreateError = "You can only make an appointment starting tomorrow...";
                return View(consultation);
            }

            if (!IsTimeSlotAvailable(consultation))
            {
                ViewBag.ConsultationCreateError = "Selected date and time is not free. Please choose other time slot or date";
                return View(consultation);
            }

            db.Consultations.Add(consultation);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Consulation/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Consultation consulation = db.Consultations.Find(id);
            if (consulation == null)
            {
                return HttpNotFound();
            }
            return View(consulation);
        }

        // POST: Consulations/Edit/5
        [HttpPut]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(Consultation consultation)
        {
            consultation.User = db.Users.Single(t => t.Id == consultation.UserId);
            consultation.Doctor = db.Doctors.Single(t => t.DoctorId == consultation.DoctorId);

            if (!ModelState.IsValid)
            {
                ViewBag.ConsultationCreateError = "Model state is not valid...";
                return View(consultation);
            }

            if (consultation.date_day <= System.DateTime.Today)
            {
                ViewBag.ConsultationCreateError = "You can only make an appointment starting tomorrow...";
                return View(consultation);
            }

            if (!IsTimeSlotAvailable(consultation))
            {
                ViewBag.ConsultationCreateError = "Selected date and time is not free. Please choose other time slot or date";
                return View(consultation);
            }

            db.Entry(consultation).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
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
        [HttpDelete]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User, Admin")]
        public ActionResult Delete(int id)
        {
            Consultation consultation = db.Consultations.Find(id);
            db.Consultations.Remove(consultation);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


    }
}
