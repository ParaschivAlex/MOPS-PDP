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
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Specializations
        public ActionResult Index()
        {
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }

            var specs = from spec in db.Specializations
                             orderby spec.Name
                             select spec;
            ViewBag.Specializations = specs;
            return View();
        }

        public ActionResult Show(int id)
        {
            try
            {
                Specialization spec = db.Specializations.Find(id);
                var docs = from doc in db.Doctors
                                 where doc.SpecializationID == spec.SpecializationID
                           select doc;
                //Console.WriteLine(doc);
                if (docs != null)
                {
                    ViewBag.Specializations = spec;
                    ViewBag.Doctors = docs;
                    return View(spec);
                }
                else
                {
                    throw new NullReferenceException("You can't check a category that has no doctors!");
                }
            }
            catch (Exception e)
            {
                TempData["message"] = e;
                return Redirect("/Specializations/Index");
            }
        }

        // GET: Specializations/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Specializations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "SpecializationID,Price,Name")] Specialization specialization)
        {
            if (ModelState.IsValid)
            {
                db.Specializations.Add(specialization);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(specialization);
        }

        // GET: Specializations/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Specialization specialization = db.Specializations.Find(id);
            if (specialization == null)
            {
                return HttpNotFound();
            }
            return View(specialization);
        }

        // POST: Specializations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "SpecializationID,Price,Name")] Specialization specialization)
        {
            if (ModelState.IsValid)
            {
                db.Entry(specialization).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(specialization);
        }

        // POST: Specializations/Delete/5
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            Specialization specialization = db.Specializations.Find(id);
            db.Specializations.Remove(specialization);
            db.SaveChanges();
            return RedirectToAction("Index");
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
