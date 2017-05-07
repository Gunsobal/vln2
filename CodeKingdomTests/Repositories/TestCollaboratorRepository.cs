using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CodeKingdom.Repositories;
using System.Linq;
using CodeKingdom.Models.Entities;

namespace CodeKingdomTests.Repositories
{
    [TestClass]
    public class TestCollaboratorRepository
    {
        private CollaboratorRepository repo;

        #region Initialize
        [TestInitialize]
        public void Initialize()
        {
            MockDataContext mockDb = new MockDataContext();
            TestSeed.CollaboratorRoles(mockDb);
            TestSeed.Collaborators(mockDb);
            repo = new CollaboratorRepository(mockDb);
        }
        #endregion

        #region Test get methods
        [TestMethod]
        public void TestGetCollaboratorById()
        {
            // Arrange
            const int ID = 1;
            const int expectedProjectID = 1;
            const string expectedUserID = "test1";

            // Act
            var result = repo.GetById(ID);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedProjectID, result.ProjectID);
            Assert.AreEqual(expectedUserID, result.ApplicationUserID);
        }

        [TestMethod]
        public void TestGetCollaboratorByIdFail()
        {
            // Arrange
            const int ID = 90;

            // Act
            var result = repo.GetById(ID);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void TestGetCollaboratorsByProjectId()
        {
            // Arrange
            const int projectID = 1;
            const int expectedCount = 3;

            // Act
            var result = repo.GetByProjectId(projectID);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedCount, result.Count);
            foreach (Collaborator collab in result)
            {
                Assert.AreEqual(collab.ProjectID, projectID);
            }
        }

        [TestMethod]
        public void TestGetCollaboratorsByProjectIdFail()
        {
            // Arrange
            const int projectID = 90;

            // Act
            var result = repo.GetByProjectId(projectID);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void TestGetCollaboratorsByUserId()
        {
            // Arrange
            const string userID = "test1";
            const int expectedCount = 3;

            // Act
            var result = repo.GetByUserId(userID);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedCount, result.Count);
            foreach (Collaborator collab in result)
            {
                Assert.AreEqual(userID, collab.ApplicationUserID);
            }
        }

        [TestMethod]
        public void TestGetCollaboratorsByUserIdFail()
        {
            // Arrange
            const string userID = "fail";

            // Act
            var result = repo.GetByUserId(userID);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void TestGetCollaboratorByUserAndProjectId()
        {
            // Arrange
            const int projectID = 1;
            const string userID = "test1";
            const int expectedID = 1;

            // Act
            var result = repo.GetByUserIdAndProjectId(userID, projectID);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedID, result.ID);
        }

        [TestMethod]
        public void TestGetCollaboratorByUserAndProjectIdFail()
        {
            // Arrange
            const int existingProjectID = 1;
            const int notExistingProjectID = 90;
            const string existingUserID = "fail";
            const string notExistingUserID = "test1";

            // Act
            var result1 = repo.GetByUserIdAndProjectId(existingUserID, existingProjectID);
            var result2 = repo.GetByUserIdAndProjectId(notExistingUserID, notExistingProjectID);

            // Assert
            Assert.IsNull(result1);
            Assert.IsNull(result2);
        }
        #endregion

        // TODO test create

        #region Test update method
        #endregion
        #region Test delete method
        #endregion

        #region Test IsInProject method
        [TestMethod]
        public void TestIsCollaboratorInProject()
        {
            // Arrange
            const int projectID = 1;
            const string userID = "test1";

            // Act
            var result = repo.IsInProject(userID, projectID);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestIsCollaboratorInProjectFail()
        {
            // Arrange
            const int existingProjectID = 1;
            const int notExistingProjectID = 0;
            const string notExistingUserID = "fail";
            const string existingUserID = "test1";

            // Act
            var result1 = repo.IsInProject(notExistingUserID, existingProjectID);
            var result2 = repo.IsInProject(existingUserID, notExistingProjectID);

            // Assert
            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
        }
        #endregion
    }
}
