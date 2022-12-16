using Microsoft.AspNet.Identity;
using PDP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PDP.Controllers
{
    public class DoctorsController : Controller
    {
        public ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index(string sortOrder, string search, string currentFilter)
        {
            ViewBag.CurrentSort = sortOrder;

            if (search == null)
            {
                search = currentFilter;
            }

            ViewBag.CurrentFilter = search;

            var doctors = from d in db.Doctors
                          select d;

            if (!String.IsNullOrEmpty(search))
            {
                doctors = doctors.Where(d => d.FirstName.Contains(search) || d.SecondName.Contains(search));
            }

            switch (sortOrder)
            {                         
                case "1": //crescator dupa nume
                    doctors = doctors.OrderBy(f => f.FirstName);;
                    break;
                case "2": //descrescator dupa nume
                    doctors = doctors.OrderByDescending(f => f.FirstName);
                    break;
                case "3": //crescator dupa prenume
                    doctors = doctors.OrderBy(f => f.SecondName); ;
                    break;
                case "4": //descrescator dupa prenume
                    doctors = doctors.OrderByDescending(f => f.SecondName);
                    break;
                default:
                    doctors = doctors.OrderBy(f => f.FirstName);
                    break;
            }

            var numberOfDocs = doctors.Count();

            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }

            ViewBag.total = numberOfDocs;
            ViewBag.Doctors = doctors;
            ViewBag.specializations = GetSpecializations();
            //ViewBag.SearchString = search;
            //db.SaveChanges();
            return View(doctors.ToList());
        }

        public ActionResult Details(int id)
        {
            System.Diagnostics.Debug.WriteLine("====================");
            System.Diagnostics.Debug.WriteLine(id);
            System.Diagnostics.Debug.WriteLine("====================");
            ViewBag.specializations = GetSpecializations();
            Doctor doctor = db.Doctors.Find(id);

            System.Diagnostics.Debug.WriteLine("====================");
            System.Diagnostics.Debug.WriteLine(doctor.DoctorId);
            System.Diagnostics.Debug.WriteLine("====================");

            ViewBag.Doctor = doctor;
            ViewBag.Specialization = doctor.Specialization;
            ViewBag.currentUser = User.Identity.GetUserId();
            return View(doctor);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            Doctor doctor = new Doctor();
            doctor.SpecializationList = GetSpecializations();
            return View(doctor);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Create(Doctor doctor)
        {
            try
            {               
                    db.Doctors.Add(doctor);
                    db.SaveChanges();
                    return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View(doctor);
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            Doctor doctor = db.Doctors.Find(id);
            return View(doctor);

        }
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public ActionResult Edit(int id, Doctor requestDoctor)
        {

            try
            {
                    Doctor doctor = db.Doctors.Find(id);
                    doctor.FirstName = requestDoctor.FirstName;
                doctor.SecondName = requestDoctor.SecondName;
                doctor.IsAvailable = requestDoctor.IsAvailable;
                doctor.PriceRate = requestDoctor.PriceRate;
                doctor.PhoneNumber = requestDoctor.PhoneNumber;
                doctor.Email = requestDoctor.Email;
                doctor.Photo = requestDoctor.Photo;
                db.SaveChanges();
                    TempData["message"] = "The doctor has been modified!";     
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return View(requestDoctor);
            }

        }
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Doctor doctor = db.Doctors.Find(id);
            db.Doctors.Remove(doctor);
            db.SaveChanges();
            TempData["message"] = "The doctor has been deleted!";
            return RedirectToAction("Index");
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetSpecializations()
        {
            var selectList = new List<SelectListItem>();
            var specs = from sp in db.Specializations select sp;

            foreach (var spec in specs)
            {
                selectList.Add(new SelectListItem
                {
                    Value = spec.SpecializationID.ToString(),
                    Text = spec.Name.ToString()
                });
            }
            return selectList;
        }
    }
}