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
            return View();
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
                return View();
            }
        }
    }
}