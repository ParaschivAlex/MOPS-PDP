using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDP;
using PDP.Controllers;

namespace PDP.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Contact()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Contact() as ViewResult;
            System.Diagnostics.Debug.WriteLine("=====BEFORE=====");
            System.Diagnostics.Debug.WriteLine(result);
            System.Diagnostics.Debug.WriteLine("=====AFETR=====");
            // Assert
            Assert.IsNotNull(result);
        }
    }
}
