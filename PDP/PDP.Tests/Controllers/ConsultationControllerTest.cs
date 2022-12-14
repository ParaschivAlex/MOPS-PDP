using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDP.Controllers;
using PDP.Models;

namespace PDP.Tests.Controllers
{
    [TestClass]
    public class ConsultationControllerTest
    {
        // Arange Data - First - In order to create doctors we need Specializations first
        private List<Specialization> specializationsToAdd = new List<Specialization>
        {
            new Specialization { SpecializationID = 9998, Name = "Reumatologie", Price = 150 },
        };
        private SpecializationsController specializationsController = new SpecializationsController();

        // Arange Data - Second - Prepare Doctors to create
        // Note: there specializationId is the order of their specialization in specializationsToAdd, in order to to update them easier after inserting specializations
        private List<Doctor> doctorsToAdd = new List<Doctor>
        {
            new Doctor { DoctorId = 1001, FirstName = "Gherasim", SecondName = "Rares", SpecializationID = 0, IsAvailable = true, PriceRate = 1.1, PhoneNumber = "0746018999", Email = "rrares33@yahoo.com", Photo = "----", Rating = 0},
            new Doctor { DoctorId = 1002, FirstName = "Pavel", SecondName = "Dan", SpecializationID = 0, IsAvailable = false, PriceRate = 0.9, PhoneNumber = "0744353689", Email = "rrares32@yahoo.com", Photo = "----", Rating = 0},
        };
        private DoctorsController doctorsController = new DoctorsController();

        // We need a user in order to test consultations
        // In order to run these tests you need a user with mail "test0@gmail.com" and id "06003fb0-2e29-4130-ad14-06eedf7805da"
        private string userId = "06003fb0-2e29-4130-ad14-06eedf7805da";

        // consultation preaprea Area
        ConsultationsController consultationsController = new ConsultationsController();
        private Consultation consultation = new Consultation { ConsultationID = 9999, price = 100, DoctorId = 1001, UserId ="EMPTY", slot_hour = 10, date_day = System.DateTime.Parse("12/20/2022")};


        [TestMethod]
        public void AllConsultationMethodsTests()
        {
            consultation.UserId = userId;

            // ============================= CREATE SPECIALIZATIONS ==================================
            foreach (Specialization specToAdd in specializationsToAdd)
            {
                ViewResult result = specializationsController.Create(specToAdd) as ViewResult;
            }
            ViewResult indexResultSpecializations = specializationsController.Index() as ViewResult;
            List<Specialization> indexDataSpecializations = indexResultSpecializations.ViewData.Model as List<Specialization>;
            foreach (Specialization viewSpecialization in indexDataSpecializations)
            {
                for (int i = 0; i < specializationsToAdd.Count(); i++)
                {
                    if (viewSpecialization.Name.Equals(specializationsToAdd[i].Name) && viewSpecialization.Price.Equals(specializationsToAdd[i].Price))
                    {
                        specializationsToAdd[i].SpecializationID = viewSpecialization.SpecializationID;
                        break;
                    }
                }
            }
            // ============================= CREATE DOCTORS ==========================================================
            doctorsToAdd[0].SpecializationID = specializationsToAdd[doctorsToAdd[0].SpecializationID].SpecializationID;
            doctorsToAdd[1].SpecializationID = specializationsToAdd[doctorsToAdd[1].SpecializationID].SpecializationID;
            foreach (Doctor doctor in doctorsToAdd)
            {
                ViewResult result = doctorsController.Create(doctor) as ViewResult;
            }
            ViewResult indexResultAfterCreate = doctorsController.Index() as ViewResult;
            List<Doctor> indexDataAfterCreate = indexResultAfterCreate.ViewData["doctors"] as List<Doctor>;
            foreach (Doctor viewDoctor in indexDataAfterCreate)
            {
                for (int i = 0; i < doctorsToAdd.Count(); i++)
                {
                    if (viewDoctor.FirstName.Equals(doctorsToAdd[i].FirstName) && viewDoctor.SpecializationID.Equals(doctorsToAdd[i].SpecializationID) && viewDoctor.SecondName.Equals(doctorsToAdd[i].SecondName))
                    {
                        doctorsToAdd[i].DoctorId = viewDoctor.DoctorId;
                        break;
                    }
                }
            }

            consultation.DoctorId = doctorsToAdd[0].DoctorId;


            // ==================================  INDEX TEST  =======================================================
            ViewResult indexConsultationsEmpty = consultationsController.Index(userIdDefault: userId) as ViewResult;
            List<Consultation> indexDataConsultationsEmpty = indexConsultationsEmpty.ViewData.Model as List<Consultation>;
            Assert.IsNotNull(indexDataConsultationsEmpty);
            Assert.AreEqual(0, indexDataConsultationsEmpty.Count());

            // ================================== CREATE TEST ========================================================
            ViewResult indexCreateAndReturnConsultation = consultationsController.Create(doctorsToAdd[0].DoctorId, userIdDefault:userId) as ViewResult;
            Consultation indexDataCreateAndReturnConsultation = indexCreateAndReturnConsultation.ViewData.Model as Consultation;
            Assert.IsNotNull(indexDataCreateAndReturnConsultation);
            Assert.AreEqual(doctorsToAdd[0].DoctorId, indexDataCreateAndReturnConsultation.DoctorId);
            Assert.AreEqual(userId, indexDataCreateAndReturnConsultation.UserId);

            ViewResult indexCreatedConsultation = consultationsController.Create(consultation) as ViewResult;
            ViewResult indexCreatedConsultation2 = consultationsController.Create(consultation) as ViewResult;
            ViewResult indexConsultationsAfterCreation = consultationsController.Index(userIdDefault: userId) as ViewResult;
            List<Consultation> indexDataCreatedConsultation = indexConsultationsAfterCreation.ViewData.Model as List<Consultation>;
            Assert.IsNotNull(indexDataCreatedConsultation);
            Assert.AreEqual(1, indexDataCreatedConsultation.Count());
            consultation.ConsultationID = indexDataCreatedConsultation[0].ConsultationID;

            // ============================= EDIT TESTS ==============================================================
            int? aux = null;
            ViewResult indexEditWithNull = consultationsController.Edit(aux) as ViewResult;
            ViewResult indexEditWithNotExists = consultationsController.Edit(9999999) as ViewResult;
            ViewResult indexEditWithConsultationID = consultationsController.Edit(consultation.ConsultationID) as ViewResult;
            Consultation indexDataEditWithConsultationID = indexEditWithConsultationID.ViewData.Model as Consultation;
            Assert.AreEqual(indexDataEditWithConsultationID, consultation);

            ViewResult indexEditWithConsultationError = consultationsController.Edit(consultation) as ViewResult;
            consultation.slot_hour -= 1;
            ViewResult indexEditWithConsultation = consultationsController.Edit(consultation) as ViewResult;
            ViewResult indexConsultationsAfterEdit = consultationsController.Index(userIdDefault: userId) as ViewResult;
            List<Consultation> indexDataConsultationAfterEdit = indexConsultationsAfterEdit.ViewData.Model as List<Consultation>;
            Assert.AreEqual(consultation, indexDataConsultationAfterEdit[0]);

            // ============================= DELETE CONSULTATION =====================================================
            ViewResult indexConsultationAfterDelete = consultationsController.Delete(consultation.ConsultationID) as ViewResult;
            ViewResult indexConsultationsAfterDelete = consultationsController.Index(userIdDefault: userId) as ViewResult;
            List<Consultation> indexDataConsultationAfterDelete = indexConsultationsAfterDelete.ViewData.Model as List<Consultation>;

            // ============================= DELETE CREATED DOCTORS ==================================================
            // Now we should check if we can delete the added data
            foreach (Doctor docToAdd in doctorsToAdd)
            {
                ViewResult result = doctorsController.Delete(docToAdd.DoctorId) as ViewResult;
            }

            // ============================= DELETE CREATED SPECIALIZATIONS =========================================
            // Now we should check if we can delete the added data
            foreach (Specialization specToAdd in specializationsToAdd)
            {
                System.Diagnostics.Debug.WriteLine(specToAdd.SpecializationID);
                ViewResult result = specializationsController.Delete(specToAdd.SpecializationID) as ViewResult;
            }

            // ============================= DISPOSE USED CONTROLLERS ===============================================
            doctorsController.Dispose();
            specializationsController.Dispose();
            consultationsController.Dispose();
        }
    }
}
