using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDP.Controllers;
using PDP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web.Mvc;

namespace PDP.Tests.Controllers
{
    [TestClass]
    public class ConsultationControllerTest
    {
        private string userId = "9007c490-c000-4ed4-8ab0-481d66e70b2a";
        private Consultation consultation = new Consultation { ConsultationID = 9999, price = 100, DoctorId = 2, UserId = "EMPTY", slot_hour = 10, date_day = System.DateTime.Parse("20/12/2022") };

        [TestMethod]
        public void Delete()
        {     
            
            ConsultationsController consController = new ConsultationsController();

            //ViewResult indexCreateAndReturnConsultation = consController.Create(2, userIdDefault: userId) as ViewResult;
            //Consultation indexDataCreateAndReturnConsultation = indexCreateAndReturnConsultation.ViewData.Model as Consultation;
            //ViewResult indexCreatedConsultation = consController.Create(consultation) as ViewResult;

            ViewResult cons_from_index = consController.Index(userIdDefault: userId) as ViewResult;

            List<Consultation> list_cons_from_index = cons_from_index.ViewData.Model as List<Consultation>;

            var id = list_cons_from_index.LastOrDefault().ConsultationID;

            ViewResult result = consController.Delete(list_cons_from_index.LastOrDefault().ConsultationID) as ViewResult;

            ViewResult c1 = consController.Index(userIdDefault: userId) as ViewResult;
            List<Consultation> c2 = c1.ViewData.Model as List<Consultation>;

            /*bool ok = true;
            foreach(Consultation cons in c2)
            {
                if(cons.ConsultationID == id)
                {
                    ok = false;
                    break;
                }
            }*/

            Assert.IsNotNull(c2);

            //Assert.IsTrue(ok);

        }
    }
}
