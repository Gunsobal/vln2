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
        public void TestGetFileById()
        {
            // Arrange
            const int successID = 1;
            const int failID = 90;
            const string expectedName = "";

            // Act


            // Assert
        }
    }
}
