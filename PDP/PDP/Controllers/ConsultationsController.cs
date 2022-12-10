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
    public class ConsultationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Specializations
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(db.Specializations.ToList());
        }

    }
}
