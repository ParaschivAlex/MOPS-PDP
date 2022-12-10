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
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(db.Specializations.ToList());
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
        public ActionResult Create([Bind(Include = "SpecializationID,Price,Name")] Specializations specializations)
        {
            if (ModelState.IsValid)
            {
                db.Specializations.Add(specializations);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(specializations);
        }

        // GET: Specializations/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Specializations specializations = db.Specializations.Find(id);
            if (specializations == null)
            {
                return HttpNotFound();
            }
            return View(specializations);
        }

        // POST: Specializations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "SpecializationID,Price,Name")] Specializations specializations)
        {
            if (ModelState.IsValid)
            {
                db.Entry(specializations).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(specializations);
        }

        // GET: Specializations/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Specializations specializations = db.Specializations.Find(id);
            if (specializations == null)
            {
                return HttpNotFound();
            }
            return View(specializations);
        }

        // POST: Specializations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Specializations specializations = db.Specializations.Find(id);
            db.Specializations.Remove(specializations);
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
