using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CodeKingdom.Repositories;
using CodeKingdom.Models.Entities;

namespace CodeKingdomTests.Repositories
{
    [TestClass]
    public class TestProjectRepository
    {
        private ProjectRepository repo;

        [TestInitialize]
        public void Initialize()
        {
            var mockDb = new MockDataContext();
            repo = new ProjectRepository(mockDb);
        }

        [TestMethod]
        public void TestGetAll()
        {
            //Arrange
            string id = "7";
            //Act
            var res = repo.getByUserId(id);

            //Assert
            Assert.AreEqual(5, res);
        }
    }
}
