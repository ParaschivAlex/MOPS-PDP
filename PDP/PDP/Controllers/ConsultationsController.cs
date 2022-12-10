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

        // GET: Specializations
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();

            return View(db.Consultations.Where(t => t.user.Id == userId));
        }


        [Authorize(Roles = "Admin")]
        public ActionResult Create(int id)
        {
            return View(id);
        }


    }
}
