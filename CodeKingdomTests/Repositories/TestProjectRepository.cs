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
            TestSeed.Folders(mockDb);
            TestSeed.Projects(mockDb);
            repo = new ProjectRepository(mockDb);
        }

        [TestMethod]
        public void TestGetProjectById()
        {
            // Arrange
            const int successID = 1;
            const int failID = 5;
            const string expectedName = "SpaceX";

            // Act
            var success = repo.getById(successID);
            var fail = repo.getById(failID);

            // Assert
            Assert.IsNull(fail);
            Assert.IsNotNull(success);
            Assert.AreEqual(expectedName, success.Name);
        }
    }
}
