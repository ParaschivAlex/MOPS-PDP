using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDP.Controllers;
using System.Web.Mvc;

namespace PDP.Tests.Controllers
{
    [TestClass]
    public class ManageControllerTest
    {
        [TestMethod]
        public void ChangePassword()
        {
            //Arrange
            ManageController sut = new ManageController();

            //Act
            ViewResult result = sut.ChangePassword() as ViewResult;

            //Assert
            Assert.IsNotNull(result);
            sut.Dispose();
        }

        [TestMethod]
        public void SetPassword()
        {
            //Arrange
            ManageController sut = new ManageController();

            //Act
            ViewResult result = sut.SetPassword() as ViewResult;

            //Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void AddPhoneNumber()
        {
            //Arrange
            ManageController sut = new ManageController();

            //Act
            ViewResult result = sut.AddPhoneNumber() as ViewResult;

            //Assert
            Assert.IsNotNull(result);
        }
    }
}

