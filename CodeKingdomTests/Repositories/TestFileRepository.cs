﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CodeKingdom.Repositories;
using CodeKingdom.Models.Entities;

namespace CodeKingdomTests.Repositories
{
    [TestClass]
    public class TestFileRepository
    {
        private FileRepository repo;

        #region Initialize
        [TestInitialize]
        public void Initialize()
        {
            var mockDb = new MockDataContext();
            TestSeed.Files(mockDb);
            repo = new FileRepository(mockDb);
        }
        #endregion 

        #region Test get methods
        [TestMethod]
        public void TestGetFileById()
        {
            // Arrange
            const int ID = 1;
            const string expectedName = "index";

            // Act
            var result = repo.GetById(ID);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedName, result.Name);
        }

        [TestMethod]
        public void TestGetFileByIdFail()
        {
            // Arrange
            const int ID = 10;

            // Act
            var result = repo.GetById(ID);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void TestGetFilesByFolderId()
        {
            // Arrange
            const int folderID = 2;
            const int expectedCount = 3;

            // Act
            var result = repo.GetByFolderId(folderID);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedCount, result.Count);
            foreach (File file in result)
            {
                Assert.AreEqual(folderID, file.FolderID);
            }
        }

        [TestMethod]
        public void TestGetFilesByFolderIdFail()
        {
            // Arrange
            const int folderID = 90;

            // Act
            var result = repo.GetByFolderId(folderID);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }
        #endregion

        #region Test delete method
        [TestMethod]
        public void TestDeleteFileById()
        {
            // Arrange
            const int ID = 1;

            // Act
            var result = repo.DeleteById(ID);
            var file = repo.GetById(ID);

            // Assert
            Assert.IsTrue(result);
            Assert.IsNull(file);
        }

        [TestMethod]
        public void TestDeleteFileByIdFail()
        {
            // Arrange
            const int ID = 0;

            // Act
            var result = repo.DeleteById(ID);

            // Assert
            Assert.IsFalse(result);
        }
        #endregion

        // TODO: Test Create
        // TODO: Test Update
    }
}
