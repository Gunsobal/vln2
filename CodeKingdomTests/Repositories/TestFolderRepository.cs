using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CodeKingdom.Repositories;
using CodeKingdom.Models.Entities;

namespace CodeKingdomTests.Repositories
{
    [TestClass]
    public class TestFolderRepository
    {
        private FolderRepository repo;
        
        [TestInitialize]
        public void Initialize()
        {
            var mockDb = new MockDataContext();
            TestSeed.Folders(mockDb);
            repo = new FolderRepository(mockDb);
        }

        [TestMethod]
        public void TestGetFolderById()
        {
            // Arrange
            const int successID = 1;
            const int failID = 30;
            const string expectedName = "root1";

            // Act
            var success = repo.GetById(successID);
            var fail = repo.GetById(failID);

            // Assert
            Assert.IsNull(fail);
            Assert.IsNotNull(success);
            Assert.AreEqual(expectedName, success.Name);
        }
    }
}
