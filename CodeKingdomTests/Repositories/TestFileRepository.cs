using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CodeKingdom.Repositories;

namespace CodeKingdomTests.Repositories
{
    [TestClass]
    public class TestFileRepository
    {
        private FileRepository repo;

        [TestInitialize]
        public void Initialize()
        {
            var mockDb = new MockDataContext();
            TestSeed.Files(mockDb);
            repo = new FileRepository(mockDb);
        }

        [TestMethod]
        public void TestGetById()
        {
            // Arrange

            const int id = 1;

            // Act

            var result = repo.GetById(id);

            // Assert
            Assert.IsNotNull(id);
        }
    }
}
