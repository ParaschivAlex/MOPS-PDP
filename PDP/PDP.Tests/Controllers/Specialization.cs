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
	public class Specialization
	{
		[TestMethod]
		public void Index()
		{
			// Arrange
			SpecializationsController controller = new SpecializationsController();

			// Act
			ViewResult result = controller.Index() as ViewResult;

			// Assert
			Assert.IsNotNull(result);
		}
	}
}
