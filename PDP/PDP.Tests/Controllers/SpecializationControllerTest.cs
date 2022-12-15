using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDP.Controllers;
using PDP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PDP.Tests.Controllers
{
    [TestClass]
    public class SpecializationControllerTest
    {
        private Specialization spec = new Specialization { Name = "Neurologie", Price = 500 };
        private SpecializationsController specController = new SpecializationsController();

        [TestMethod]
        public void CreateGET()
        {
            //Arrange
            SpecializationsController sut = new SpecializationsController();

            //Act
            ViewResult result = sut.Create() as ViewResult;

            //Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CreatePOST()
        {
            ViewResult result = specController.Create(spec) as ViewResult;

            ViewResult spec_from_index = specController.Index() as ViewResult;

            List<Specialization> indexData = spec_from_index.ViewData.Model as List<Specialization>;

            Assert.IsTrue(indexData.Contains(spec));
        }
    }
}
