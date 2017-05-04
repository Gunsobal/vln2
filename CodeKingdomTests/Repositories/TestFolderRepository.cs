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
        public void TestGetById()
        {
            // Arrange
            const int id = 1;

            // Act
            var result = repo.GetChildrenById(id);

            // Assert
            Assert.AreEqual(2, result.Count);
        }
    }
}
