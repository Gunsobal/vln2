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
        private FileRepository fileRepo;

        #region Initialize
        [TestInitialize]
        public void Initialize()
        {
            var mockDb = new MockDataContext();
            TestSeed.Folders(mockDb);
            repo = new FolderRepository(mockDb);
            fileRepo = new FileRepository(mockDb);
        }
        #endregion

        #region Test get methods
        [TestMethod]
        public void TestGetFolderById()
        {
            // Arrange
            const int ID = 1;
            const string expectedName = "root1";

            // Act
            var result = repo.GetById(ID);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedName, result.Name);
        }

        [TestMethod]
        public void TestGetFolderByIdFail()
        {
            // Arrange
            const int ID = 30;

            // Act
            var result = repo.GetById(ID);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void TestGetFolderChildrenById()
        {
            // Arrange
            const int ID = 1;
            const int expectedCount = 2;

            // Act
            var result = repo.GetChildrenById(ID);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedCount, result.Count);
            foreach (Folder folder in result)
            {
                Assert.AreEqual(ID, folder.FolderID);
            }
        }

        [TestMethod]
        public void TestGetFolderChildrenByIdFail()
        {
            // Arrange
            const int ID = 90;

            // Act
            var result = repo.GetChildrenById(ID);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void TestGetCascadingFolderChildrenByIdFromRoot()
        {
            // Arrange
            const int ID = 1;
            const int expectedCount = 4;

            // Act
            var result = repo.GetCascadingChildrenById(ID);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedCount, result.Count);
        }

        [TestMethod]
        public void TestGetCascadingFolderChildrenByIdFromChild()
        {
            // Arrange
            const int ID = 4;
            const int expectedCount = 2;

            // Act
            var result = repo.GetCascadingChildrenById(ID);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedCount, result.Count);
        }

        [TestMethod]
        public void TestGetCascadingFolderChildrenByIdFail()
        {
            // Arrange
            const int ID = 8;

            // Act
            var result = repo.GetCascadingChildrenById(ID);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void TestGetCascadingFolderChildrenByIdNoChildren()
        {
            // Arrange
            const int ID = 2;

            // Act
            var result = repo.GetCascadingChildrenById(ID);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void TestGetFolderParent()
        {
            // Arrange
            const int ID = 4;
            const int expectedID = 1;

            // Act
            var result = repo.GetParent(ID);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedID, result.ID);
        }

        [TestMethod]
        public void TestGetFolderParentNoParent()
        {
            // Arrange
            const int ID = 1;

            // Act
            var result = repo.GetParent(ID);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void TestGetFolderParentFail()
        {
            // Arrange
            const int ID = 0;

            // Act
            var result = repo.GetParent(ID);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void TestGetFolderRoot()
        {
            // Arrange
            const int ID = 4;
            const int expectedID = 1;

            // Act
            var result = repo.GetRoot(ID);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedID, result.ID);
        }

        [TestMethod]
        public void TestGetFolderRootNested()
        {
            // Arrange
            const int ID = 5;
            const int expectedID = 1;

            // Act
            var result = repo.GetRoot(ID);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedID, result.ID);
        }

        [TestMethod]
        public void TestGetFolderRootFromRoot()
        {
            // Arrange
            const int ID = 1;

            // Act
            var result = repo.GetRoot(ID);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(ID, result.ID);
        }

        [TestMethod]
        public void TestGetFolderRootFail()
        {
            // Arrange
            const int ID = 0;

            // Act
            var result = repo.GetRoot(ID);

            // Assert
            Assert.IsNull(result);
        }
        #endregion

        #region Test create method
        [TestMethod]
        public void TestCreateNewFolder()
        {
            // Arrange
            const int parent = 1;
            const string newName = "FolderTest";

            // Act
            var result = repo.Create(new Folder { Name = newName, FolderID = parent });

            // Assert
            Assert.AreEqual(newName, result.Name);
        }

        [TestMethod]
        public void TestCreateDuplicateFolder()
        {
            // Arrange
            const int parent = 1;
            const string newName = "css";

            // Act
            var result = repo.Create(new Folder { Name = newName, FolderID = parent });

            // Assert
            Assert.AreNotEqual(newName, result.Name);
        }

        [TestMethod]
        public void TestCreateMultipleDuplicateFolders()
        {
            // Arrange
            const int parent = 1;
            const string newName = "css";

            // Act
            var result1 = repo.Create(new Folder { Name = newName, FolderID = parent });
            var result2 = repo.Create(new Folder { Name = newName, FolderID = parent });

            // Assert
            Assert.AreNotEqual(newName, result1.Name);
            Assert.AreNotEqual(newName, result2.Name);
            Assert.AreNotEqual(result1.Name, result2.Name);
        }
        #endregion

        #region Test delete method
        [TestMethod]
        public void TestDeleteFolder()
        {
            // Arrange
            const int ID = 5;

            // Act
            var success = repo.DeleteById(ID);
            var result = repo.GetById(ID);

            // Assert
            Assert.IsTrue(success);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void TestDeleteFolderWithFiles()
        {
            // Arrange
            const int ID = 5;
            const int FileID = 3;

            // Act
            var success = repo.DeleteById(ID);
            var nullFile = fileRepo.GetById(FileID);

            // Assert
            Assert.IsTrue(success);
            Assert.IsNull(nullFile);
        }

        [TestMethod]
        public void TestDeleteFolderWithFolders()
        {
            // Arrange 
            const int ID = 2;

            // Act
            var success = repo.DeleteById(ID);
            var folders = repo.GetChildrenById(ID);

            // Assert
            Assert.IsTrue(success);
            Assert.AreEqual(0, folders.Count);
        }
        #endregion
    }
}
