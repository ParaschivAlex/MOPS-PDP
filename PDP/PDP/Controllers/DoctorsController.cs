using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;
using PDP.Models;

namespace PDP.Controllers
{
    public class DoctorsController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Doctors
        [Authorize(Roles = "User, Admin")]
        public ActionResult Index()
        {
            ViewBag.selectedOption = -1;
            ViewBag.searchString = "";
            ViewBag.sortOption = "order-by-names";
            if (Request.Params.Get("SelectOption") != null)
            {
                // System.Diagnostics.Debug.WriteLine(Request.Params.Get("SelectOption"));
                ViewBag.selectedOption = Int32.Parse(Request.Params.Get("SelectOption"));
            }
            if (Request.Params.Get("SearchString") != null)
            {
                // System.Diagnostics.Debug.WriteLine(Request.Params.Get("SearchString"));
                ViewBag.searchString = Request.Params.Get("SearchString");
            }
            if (Request.Params.Get("SortOption") != null)
            {
                // System.Diagnostics.Debug.WriteLine(Request.Params.Get("SortOption"));
                ViewBag.sortOption = Request.Params.Get("SortOption");
            }
            ViewBag.specializations = GetAllSpecializations();
            ViewBag.doctors = new List<Doctor>();
            // filter doctors
            foreach (Doctor doctor in db.Doctors.ToList())
            {
                String searchText = doctor.FirstName.ToLowerInvariant() + doctor.SecondName.ToLowerInvariant();
                foreach (Specializations specialization in ViewBag.specializations)
                {
                    if (doctor.SpecializationID == specialization.SpecializationID)
                    {
                        System.Diagnostics.Debug.WriteLine(Request.Params.Get("Founddd"));
                        searchText += specialization.Name;
                        break;
                    }
                }
                System.Diagnostics.Debug.WriteLine(Request.Params.Get(searchText));
                if (searchText.Contains(ViewBag.searchString.ToLowerInvariant()))
                {
                    if (ViewBag.selectedOption != -1)
                    {
                        if (ViewBag.selectedOption == doctor.SpecializationID)
                        {                 
                            ViewBag.doctors.Add(doctor);
                        }
                    } else
                    { 
                        ViewBag.doctors.Add(doctor);
                    }
                }
            }
            switch (ViewBag.sortOption)
            {
                case "order-by-names":
                    ((List<Doctor>)ViewBag.doctors).Sort(delegate (Doctor x, Doctor y)
                    {
                        return x.FirstName.CompareTo(y.FirstName);
                    });                
                    break;
                case "order-by-names-reverse":
                    ((List<Doctor>)ViewBag.doctors).Sort(delegate (Doctor x, Doctor y)
                    {
                        return y.FirstName.CompareTo(x.FirstName);
                    });
                    break;
                case "order-by-price":
                    ((List<Doctor>)ViewBag.doctors).Sort(delegate (Doctor x, Doctor y)
                    {
                        // TODO: Change it to how we actually calculate the price of a doctor
                        double xPrice = 0;
                        double yPrice = 0;
                        foreach (Specializations specialization in ViewBag.specializations)
                        {
                            if (x.SpecializationID == specialization.SpecializationID)
                            {
                                xPrice = specialization.Price;
                            }
                            if (y.SpecializationID == specialization.SpecializationID)
                            {
                                yPrice = specialization.Price;
                            }
                        }
                        xPrice *= x.PriceRate;
                        yPrice *= y.PriceRate;
                        return xPrice.CompareTo(yPrice);
                    });
                    break;
                case "order-by-price-reverse":
                    ((List<Doctor>)ViewBag.doctors).Sort(delegate (Doctor x, Doctor y)
                    {
                        // TODO: Change it to how we actually calculate the price of a doctor
                        double xPrice = 0;
                        double yPrice = 0;
                        foreach (Specializations specialization in ViewBag.specializations)
                        {
                            if (x.SpecializationID == specialization.SpecializationID)
                            {
                                xPrice = specialization.Price;
                            }
                            if (y.SpecializationID == specialization.SpecializationID)
                            {
                                yPrice = specialization.Price;
                            }
                        }
                        xPrice *= x.PriceRate;
                        yPrice *= y.PriceRate;
                        return yPrice.CompareTo(xPrice);
                    });
                    break;
            }
            return View();
        }

        // GET: Doctors/Details/5
        [Authorize(Roles = "User, Admin")]
        public ActionResult Details(int? id)
        {
            ViewBag.specializations = GetAllSpecializations();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Doctor doctor = db.Doctors.Find(id);
            if (doctor == null)
            {
                return HttpNotFound();
            }
            return View(doctor);
        }

        // GET: Doctors/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.specializations = GetAllSpecializationsForSelect();
            return View();
        }

        // POST: Doctors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "DoctorId,FirstName,SecondName,SpecializationID,IsAvailable,PriceRate,PhoneNumber,Photo,Email")] Doctor doctor)
        {
            if (ModelState.IsValid)
            {
                db.Doctors.Add(doctor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.specializations = GetAllSpecializationsForSelect();
            return View(doctor);
        }

        // GET: Doctors/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            ViewBag.specializations = GetAllSpecializationsForSelect();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Doctor doctor = db.Doctors.Find(id);
            if (doctor == null)
            {
                return HttpNotFound();
            }
            return View(doctor);
        }

        // POST: Doctors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "DoctorId,FirstName,SecondName,SpecializationID,IsAvailable,PriceRate,PhoneNumber,Photo,Email")] Doctor doctor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(doctor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        // GET: Doctors/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Doctor doctor = db.Doctors.Find(id);
            if (doctor == null)
            {
                return HttpNotFound();
            }
            return View(doctor);
        }

        // POST: Doctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Doctor doctor = db.Doctors.Find(id);
            db.Doctors.Remove(doctor);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [NonAction]
        public IEnumerable<Specializations> GetAllSpecializations()
        {
            var specs = from sp in db.Specializations
                        select sp;
            return specs;
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetAllSpecializationsForSelect()
        {
            var selectList = new List<SelectListItem>();

            var specs = from sp in db.Specializations
                        select sp;

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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
