using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDP.Controllers;
using PDP.Models;

namespace PDP.Tests.Controllers
{
    [TestClass]
    public class DoctorsControllerTest
    {    
        // Arange Data - First - In order to create doctors we need Specializations first
        private List<Specialization> specializationsToAdd = new List<Specialization>
            {
                new Specialization { SpecializationID = 9997, Name = "Oncologie", Price = 200 },
                new Specialization { SpecializationID = 9998, Name = "Reumatologie", Price = 150 },
            };
        private SpecializationsController specializationsController = new SpecializationsController();
        // Arange Data - Second - Prepare Doctors to create
        // Note: there specializationId is the order of their specialization in specializationsToAdd, in order to to update them easier after inserting specializations
        private List<Doctor> doctorsToAdd = new List<Doctor>
            {
                new Doctor { DoctorId = 1001, FirstName = "Gherasim", SecondName = "Rares", SpecializationID = 0, IsAvailable = true, PriceRate = 1.1, PhoneNumber = "0746018999", Email = "rrares33@yahoo.com", Photo = "----", Rating = 0},
                new Doctor { DoctorId = 1002, FirstName = "Pavel", SecondName = "Dan", SpecializationID = 1, IsAvailable = false, PriceRate = 0.9, PhoneNumber = "0744353689", Email = "rrares32@yahoo.com", Photo = "----", Rating = 0},
            };
        private DoctorsController doctorsController = new DoctorsController();

        [TestMethod]
        public void AllDoctorsMethodsTest()
        {
            // Get doctors received by Index when loading
            ViewResult indexResultBeforeCreate = doctorsController.Index() as ViewResult;
            List<Doctor> indexDataBeforeCreate = indexResultBeforeCreate.ViewData["doctors"] as List<Doctor>;
            Assert.IsNotNull(indexResultBeforeCreate);

            // ============================= CREATE SPECIALIZATIONS ==================================
            // Arrange specializations in the background
            foreach (Specialization specToAdd in specializationsToAdd)
            {
                ViewResult result = specializationsController.Create(specToAdd) as ViewResult;
            }
            // Get specializations received by Index when loading and update the IDs in specializationsToAdd in order to be able to create Doctors
            ViewResult indexResultSpecializations = specializationsController.Index() as ViewResult;
            List<Specialization> indexDataSpecializations = indexResultSpecializations.ViewData.Model as List<Specialization>;
            // Because the ID(s) change when they are added to the database, we need to use another variable to store them again
            // You may think that we can just save {indexData} into {specializationsAdded}, but if we do so we will also save {database} data,
            //     which was not produced during this test. So we better match {name} and {price} at least. 
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
            // Generates a create form, should not be null
            ViewResult resultCreateForm = doctorsController.Create() as ViewResult;
            Assert.IsNotNull(resultCreateForm);

            foreach (Doctor doctor in doctorsToAdd)
            {
                ViewResult result = doctorsController.Create(doctor) as ViewResult;
            }
            ViewResult indexResultAfterCreate = doctorsController.Index() as ViewResult;
            List<Doctor> indexDataAfterCreate = indexResultAfterCreate.ViewData["doctors"] as List<Doctor>;
            string searchString = indexResultAfterCreate.ViewData["searchString"] as string;
            string sortOption = indexResultAfterCreate.ViewData["sortOption"] as string;
            int? selectedOption = indexResultAfterCreate.ViewData["selectedOption"] as int?;
            Assert.AreEqual(indexDataAfterCreate.Count(), doctorsToAdd.Count() + indexDataBeforeCreate.Count());
            Assert.IsNotNull(selectedOption);
            Assert.AreEqual(searchString, "");
            Assert.AreEqual(sortOption, "order-by-names");

            // Actualize doctors ids in order to be able to update and delete them later
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

            // ======================== Test searchString on Index() for "gherasim" ================================
            ViewResult indexResultSearchString = doctorsController.Index(searchString: "gherasim") as ViewResult;
            List<Doctor> indexDataSearchString = indexResultSearchString.ViewData["doctors"] as List<Doctor>;
            Assert.AreEqual(1, indexDataSearchString.Count());
            Assert.AreEqual(indexDataSearchString[0], doctorsToAdd[0]);

            // ======================= Test orderBy is received as exceptedn ========================================
            ViewResult indexResultSortOption = doctorsController.Index() as ViewResult;
            List<Doctor> indexDataSortOption = indexResultSortOption.ViewData["doctors"] as List<Doctor>;
            ViewResult indexResultSortOption1 = doctorsController.Index(sortOption: "order-by-names-reverse") as ViewResult;
            List<Doctor> indexDataSortOption1 = indexResultSortOption1.ViewData["doctors"] as List<Doctor>;
            indexDataSortOption1.Reverse();
            // We can check if the sort is done right by checking if the first list of doctors is the same as the second one, but reversed
            for (int i = 0; i< indexDataSortOption.Count(); i++)
            {
                Assert.AreEqual(indexDataSortOption[i], indexDataSortOption1[i]);
            }

            ViewResult indexResultSortOption2 = doctorsController.Index(sortOption: "order-by-price") as ViewResult;
            List<Doctor> indexDataSortOption2 = indexResultSortOption2.ViewData["doctors"] as List<Doctor>;
            ViewResult indexResultSortOption3 = doctorsController.Index(sortOption: "order-by-price-reverse") as ViewResult;
            List<Doctor> indexDataSortOption3 = indexResultSortOption3.ViewData["doctors"] as List<Doctor>;
            indexDataSortOption3.Reverse();
            // We can check if the sort is done right by checking if the first list of doctors is the same as the second one, but reversed
            for (int i = 0; i < indexDataSortOption2.Count(); i++)
            {
                Assert.AreEqual(indexDataSortOption2[i], indexDataSortOption3[i]);
            }

            // ======================= Test selectedOption (filter by specialization) is received as excepted ========
            ViewResult indexResultFiltered = doctorsController.Index(selectedOption: specializationsToAdd[0].SpecializationID) as ViewResult;
            List<Doctor> indexDataFiltered = indexResultFiltered.ViewData["doctors"] as List<Doctor>;
            Assert.IsTrue(indexDataFiltered.Contains(doctorsToAdd[0]));
            Assert.IsFalse(indexDataFiltered.Contains(doctorsToAdd[1]));

            // ============================= DETAILS A DOCTOR ========================================================
            ViewResult indexResultForDetails = doctorsController.Details(doctorsToAdd[0].DoctorId) as ViewResult;
            Doctor doctorsDetails = indexResultForDetails.ViewData.Model as Doctor;
            Assert.AreEqual(doctorsDetails, doctorsToAdd[0]);


            // ============================= EDIT A DOCTOR ===========================================================
            // Change a doctor name
            doctorsToAdd[0].FirstName = "Matei";
            ViewResult editResult = doctorsController.Edit(doctorsToAdd[0]) as ViewResult;

            // Get details for the editted specialization
            ViewResult indexResultForDetailsAfterEdit = doctorsController.Details(doctorsToAdd[0].DoctorId) as ViewResult;
            Doctor doctorsDetailsAfterEdit = indexResultForDetailsAfterEdit.ViewData.Model as Doctor;

            // If this assert is true the edit was successfull
            Assert.AreEqual(doctorsToAdd[0].FirstName, doctorsDetailsAfterEdit.FirstName);
            Assert.AreNotEqual("Gherasim", doctorsDetailsAfterEdit.FirstName);


            // ============================= DELETE CREATED DOCTORS ==================================================
            // Now we should check if we can delete the added data
            foreach (Doctor docToAdd in doctorsToAdd)
            {
                ViewResult result = doctorsController.Delete(docToAdd.DoctorId) as ViewResult;
            }

            // Get specializations received by Index when loading
            ViewResult indexResultAfterDeleteDoctors = doctorsController.Index() as ViewResult;
            List<Doctor> doctorsAfterDelete = indexResultAfterDeleteDoctors.ViewData["doctors"] as List<Doctor>;

            // Assert for Index method after deleting
            Assert.IsNotNull(indexResultAfterDeleteDoctors);
            foreach (Doctor docToAdd in doctorsToAdd)
            {
                Assert.IsFalse(doctorsAfterDelete.Contains(docToAdd));
            }


            // ============================= DELETE CREATED SPECIALIZATIONS ==================================
            // Now we should check if we can delete the added data
            foreach (Specialization specToAdd in specializationsToAdd)
            {
                System.Diagnostics.Debug.WriteLine(specToAdd.SpecializationID);
                ViewResult result = specializationsController.Delete(specToAdd.SpecializationID) as ViewResult;
            }

            // Get specializations received by Index when loading
            ViewResult indexResultAfterDeleteSpecializations = specializationsController.Index() as ViewResult;
            List<Specialization> indexDataAfterDeleteSpecializations = indexResultAfterDeleteSpecializations.ViewData.Model as List<Specialization>;

            // Assert for Index method after deleting
            Assert.IsNotNull(indexResultAfterDeleteSpecializations);
            foreach (Specialization specToAdd in specializationsToAdd)
            {
                Assert.IsFalse(indexDataAfterDeleteSpecializations.Contains(specToAdd));
            }
        }
    }
}
