using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CodeKingdom.Repositories;
using System.Linq;
using CodeKingdom.Models.Entities;
using CodeKingdom.Models.ViewModels;

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

        [TestMethod]
        public void TestGetCollabRoleById()
        {
            // Arrange
            const int ID = 1;
            const string expectedName = "Owner";

            // Act
            var result = repo.GetRoleById(ID);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedName, result.Name);
        }

        [TestMethod]
        public void TestGetCollabRoleByIdFail()
        {
            // Arrange
            const int ID = 0;

            // Act
            var result = repo.GetRoleById(ID);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void TestGetCollabRoles()
        {
            // Arrange
            int expectedCount = 3;

            // Act
            var result = repo.GetAllRoles();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedCount, result.Count);
        }
        #endregion

        // TODO test create

        #region Test update method
        [TestMethod]
        public void TestUpdateCollaborator()
        {
            // Arrange
            const int ID = 1;
            const int newRoleID = 2;
            CollaboratorViewModel coll = new CollaboratorViewModel { ID = ID, RoleID = newRoleID };

            // Act
            var result = repo.Update(coll);
            var updatedColl = repo.GetById(ID);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(newRoleID, updatedColl.CollaboratorRoleID);
        }

        [TestMethod]
        public void TestUpdateCollaboratorFail()
        {
            // Arrange
            const int ID = 0;
            const int newRoleID = 2;
            CollaboratorViewModel coll = new CollaboratorViewModel { ID = ID, RoleID = newRoleID };

            // Act
            var result = repo.Update(coll);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestUpdateCollaboratorWithUnknownRole()
        {
            // Arrange
            const int ID = 1;
            const int newRoleID = 99;
            CollaboratorViewModel coll = new CollaboratorViewModel { ID = ID, RoleID = newRoleID };

            // Act
            var result = repo.Update(coll);
            var updatedColl = repo.GetById(ID);

            // Assert
            Assert.IsFalse(result);
            Assert.AreNotEqual(newRoleID, updatedColl.CollaboratorRoleID);
        }
        #endregion

        #region Test delete method
        [TestMethod]
        public void TestDeleteCollaboratorById()
        {
            // Arrange
            const int ID = 1;

            // Act
            var result = repo.DeleteById(ID);
            var collab = repo.GetById(ID);

            // Assert
            Assert.IsTrue(result);
            Assert.IsNull(collab);
        }

        [TestMethod]
        public void TestDeleteCollaboratorByIdFail()
        {
            // Arrange
            const int ID = 0;

            // Act
            var result = repo.DeleteById(ID);

            // Assert
            Assert.IsFalse(result);
        }
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
