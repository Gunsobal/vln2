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
            repo = new UserRepository();
        }

        [TestMethod]
        public void TestGetById()
        {

        }
    }
}
