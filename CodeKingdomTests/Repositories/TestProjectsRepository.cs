using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CodeKingdom.Repositories;
using CodeKingdom.Models.Entities;

namespace CodeKingdomTests.Repositories
{
    [TestClass]
    public class TestProjectsRepository
    {
        private ProjectsRepository _repo;

        [TestInitialize]
        public void Initialize()
        {
            var mockDb = new MockDataContext();
        }

        [TestMethod]
        public void TestMethod1()
        {
        }
    }
}
