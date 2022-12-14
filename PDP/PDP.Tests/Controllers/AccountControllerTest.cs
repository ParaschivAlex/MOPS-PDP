using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDP.Controllers;
using System.Web.Mvc;

namespace PDP.Tests.Controllers
{
    [TestClass]
    public class AccountControllerTest
    {
        [TestMethod]
        public void Login()
        {
            //Arrange
            AccountController sut = new AccountController();

            //Act
            ViewResult result = sut.Login("") as ViewResult;

            //Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ForgotPasswordConfirmation()
        {
            //Arrange
            AccountController sut = new AccountController();

            //Act
            ViewResult result = sut.ForgotPasswordConfirmation() as ViewResult;

            //Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ForgotPassword()
        {
            //Arrange
            AccountController sut = new AccountController();

            //Act
            ViewResult result = sut.ForgotPassword() as ViewResult;

            //Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ExternalLoginFailure()
        {
            //Arrange
            AccountController sut = new AccountController();

            //Act
            ViewResult result = sut.ExternalLoginFailure() as ViewResult;

            //Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ResetPassword()
        {
            //Arrange
            AccountController sut = new AccountController();

            //Act
            ViewResult result = sut.ResetPassword("") as ViewResult;

            //Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ResetPasswordConfirmation()
        {
            //Arrange
            AccountController sut = new AccountController();

            //Act
            ViewResult result = sut.ResetPasswordConfirmation() as ViewResult;

            //Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Register()
        {
            //Arrange
            AccountController sut = new AccountController();

            //Act
            ViewResult result = sut.Register() as ViewResult;

            //Assert
            Assert.IsNotNull(result);
        }
    }
}
