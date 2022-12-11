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
        [Authorize(Roles = "Admin")]
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


    }
}
