﻿using System;
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
            TestSeed.All(mockDb);
            repo = new ProjectRepository(mockDb);
        }

        [TestMethod]
        public void TestGetAll()
        {
            //Arrange
            string id = "7";
            //Act
            var result = repo.getAll(id);

            //Assert
            Assert.AreEqual(5, result);
        }
    }
}
