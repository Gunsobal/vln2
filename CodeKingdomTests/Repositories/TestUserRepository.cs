using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CodeKingdom.Repositories;
using CodeKingdom.Models.Entities;
using CodeKingdom.Models;

namespace CodeKingdomTests.Repositories
{
    [TestClass]
    public class TestUserRepository
    {

        private UserRepository repo;

        [TestInitialize]
        public void Initialize()
        {
            var mockDb = new MockDataContext();
            TestSeed.All(mockDb);
            repo = new UserRepository(mockDb);
        }

        [TestMethod]
        public void TestGetById()
        {
            // Arrange
            const string id = "5";

            // Act
            var result = repo.GetById(id);

            // Assert
            Assert.IsNotNull(result);

        }
    }
}
