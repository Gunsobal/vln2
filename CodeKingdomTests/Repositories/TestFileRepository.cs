using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CodeKingdom.Repositories;
using CodeKingdom.Models.Entities;
using CodeKingdom.Models.ViewModels;

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

        #region Test create method
        [TestMethod]
        public void TestCreateFile()
        {
            // Arrange
            FileViewModel newFile = new FileViewModel
            {
                Name = "testFile",
                Type = "js",
                FolderID = 4,
                ApplicationUserID = "test1",
                Content = "some text"
            };

            // Act
            var result = repo.Create(newFile);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(newFile.Name, result.Name);
        }

        [TestMethod]
        public void TestCreateFileDuplicate()
        {
            // Arrange
            const string fileName = "myscript";
            FileViewModel newFile = new FileViewModel
            {
                Name = fileName,
                FolderID = 5,
                Type = "js",
                Content = "script-stuff",
                ApplicationUserID = "dummy",
            };

            // Act
            var result = repo.Create(newFile);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreNotEqual(fileName, result.Name);
        }

        [TestMethod]
        public void TestCreateMultipleFileDuplicates()
        {
            // Arrange
            const string fileName = "myscript";
            FileViewModel newFile = new FileViewModel
            {
                Name = fileName,
                FolderID = 5,
                Type = "js",
                Content = "script-stuff",
                ApplicationUserID = "dummy"
            };

            // Act
            var result1 = repo.Create(newFile);
            var result2 = repo.Create(newFile);

            // Assert
            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.AreNotEqual(fileName, result1.Name);
            Assert.AreNotEqual(fileName, result2.Name);
            Assert.AreNotEqual(result1.Name, result2.Name);
        }
        #endregion

        #region Test update methods
        [TestMethod]
        public void TestUpdateFileContent()
        {
            // Arrange
            const string newContent = "newContent";
            FileViewModel file = new FileViewModel
            {
                ID = 1,
                Content = newContent
            };

            // Act
            var result = repo.UpdateContent(file);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(newContent, result.Content);
        }

        [TestMethod]
        public void TestUpdateFileContentNonExistingFile()
        {
            // Arrange
            FileViewModel file = new FileViewModel { ID = 0 };

            // Act
            var result = repo.UpdateContent(file);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void TestRenameFile()
        {
            // Arrange
            const string newName = "newname";
            FileViewModel file = new FileViewModel
            {
                ID = 1,
                Name = newName,
                FolderID = 1
            };

            // Act
            var result = repo.Rename(file);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(newName, result.Name);
        }

        [TestMethod]
        public void TestRenameFileDuplicate()
        {
            // Arrange
            const string newName = "better-birds";
            FileViewModel file = new FileViewModel
            {
                ID = 4,
                Name = newName,
                FolderID = 2
            };

            // Act
            var result = repo.Rename(file);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreNotEqual(newName, result.Name);
        }

        [TestMethod]
        public void TestRenameFileSameName()
        {
            // Arrange
            const string newName = "birds";
            FileViewModel file = new FileViewModel
            {
                ID = 4,
                Name = newName,
                FolderID = 2
            };

            // Act
            var result = repo.Rename(file);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(newName, result.Name);
        }

        [TestMethod]
        public void TestRenameNonExistingFile()
        {
            // Arrange
            const string newName = "d";
            FileViewModel file = new FileViewModel { ID = 0, Name = newName };

            // Act
            var result = repo.Rename(file);

            // Assert
            Assert.IsNull(result);
        }
        #endregion
    }
}
