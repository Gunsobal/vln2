using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CodeKingdom.Controllers;
using System.Web.Mvc;

namespace CodeKingdomTests.Controller
{
    [TestClass]
    public class TestProjectController
    {
        [TestMethod]
        public void TestIndex()
        {
            // Arrange
            ProjectController testController = new ProjectController();

            // Act
            ViewResult result = testController.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
