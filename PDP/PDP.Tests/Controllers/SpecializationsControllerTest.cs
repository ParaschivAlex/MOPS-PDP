using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDP;
using PDP.Controllers;
using PDP.Models;

namespace PDP.Tests.Controllers
{
	[TestClass]
	public class SpecializationsControllerTest
    {
        // Arange Data
        private List<Specialization> specializationsToAdd = new List<Specialization>
        {
            new Specialization { SpecializationID = 9997, Name = "Urologie", Price = 200 },
            new Specialization { SpecializationID = 9998, Name = "Dermatologie", Price = 150 },
            new Specialization { SpecializationID = 9999, Name = "Urologie", Price = 250 }
        };
        private SpecializationsController specializationsController = new SpecializationsController();

        [TestMethod]
		public void MethodsTest()
        {
            // Get specializations received by Index when loading
            ViewResult indexResultBeforeCreate = specializationsController.Index() as ViewResult;
            List<Specialization> indexDataBeforeCreate = indexResultBeforeCreate.ViewData.Model as List<Specialization>;

            // ============================= CREATE & INDEX TESTS ==================================
            // Creates the declared specializations and tests if they are received in the view
            // Create specializations
            foreach (Specialization specToAdd in specializationsToAdd)
            {
                ViewResult result = specializationsController.Create(specToAdd) as ViewResult;
            }

            // Get specializations received by Index when loading
            ViewResult indexResult = specializationsController.Index() as ViewResult;
            List<Specialization> indexData = indexResult.ViewData.Model as List<Specialization>;

            // Assert for Index method after creating
            Assert.IsNotNull(indexResult);
            Assert.AreEqual(specializationsToAdd.Count() + indexDataBeforeCreate.Count(), indexData.Count());
            foreach (Specialization specToAdd in specializationsToAdd)
            {
                Assert.IsTrue(indexData.Contains(specToAdd));
            }

            // Because the ID(s) change when they are added to the database, we need to use another variable to store them again
            // You may think that we can just save {indexData} into {specializationsAdded}, but if we do so we will also save {database} data,
            //     which was not produced during this test. So we better match {name} and {price} at least. 
            foreach (Specialization viewSpecialization in indexData)
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

            // ============================= EDIT TESTS ==================================
            // First test EDIT with id which only returns an specialization from DB
            ViewResult editModelResult = specializationsController.Edit(specializationsToAdd[0].SpecializationID) as ViewResult;
            Assert.IsInstanceOfType(editModelResult.Model, typeof(Specialization));
            Assert.AreEqual((editModelResult.Model as Specialization).SpecializationID, specializationsToAdd[0].SpecializationID);
            Assert.AreEqual((editModelResult.Model as Specialization).Name, specializationsToAdd[0].Name);
            Assert.AreEqual((editModelResult.Model as Specialization).Price, specializationsToAdd[0].Price);

            // Then test the actual edit
            specializationsToAdd[1].Name = "Urorologie";
            ViewResult editResult = specializationsController.Edit(specializationsToAdd[1]) as ViewResult;
            bool foundChangedSpecializationEdittedName = false;
            // Get all specializations
            ViewResult indexResultAfterEdit = specializationsController.Index() as ViewResult;
            List<Specialization> indexDataAfterEdit = indexResult.ViewData.Model as List<Specialization>;
            foreach (Specialization viewSpecialization in indexData)
            {
                if (specializationsToAdd[1].Name.Equals(viewSpecialization.Name) 
                    && specializationsToAdd[1].Price.Equals(viewSpecialization.Price) 
                    && specializationsToAdd[1].SpecializationID.Equals(viewSpecialization.SpecializationID))
                {
                    foundChangedSpecializationEdittedName = true;
                    break;
                }
            }
            // If this assert is true the edit was successfull
            Assert.IsTrue(foundChangedSpecializationEdittedName);            

            // ============================= DELETE TESTS ==================================
            // Now we should check if we can delete the added data
            foreach (Specialization specToAdd in specializationsToAdd)
            {
                ViewResult result = specializationsController.Delete(specToAdd.SpecializationID) as ViewResult;
            }

            // Get specializations received by Index when loading
            ViewResult indexResultAfterDelete = specializationsController.Index() as ViewResult;
            List<Specialization> indexDataAfterDelete = indexResult.ViewData.Model as List<Specialization>;

            // Assert for Index method after deleting
            Assert.IsNotNull(indexResultAfterDelete);
            foreach (Specialization specToAdd in specializationsToAdd)
            {
                Assert.IsFalse(indexDataAfterDelete.Contains(specToAdd));
            }
        }
    }
}
