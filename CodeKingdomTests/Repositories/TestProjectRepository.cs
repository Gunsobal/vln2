using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CodeKingdom.Repositories;
using CodeKingdom.Models.Entities;
using CodeKingdom.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace CodeKingdomTests.Repositories
{
    [TestClass]
    public class TestProjectRepository
    {
        private ProjectRepository repo;

        #region Test Initialize
        [TestInitialize]
        public void Initialize()
        {
            var mockDb = new MockDataContext();
            TestSeed.All(mockDb);
            repo = new ProjectRepository(mockDb);
        }
        #endregion

        #region Test project repo get methods
        [TestMethod]
        public void TestGetProjectById()
        {
            // Arrange
            const int ID = 1;
            const string expectedName = "SpaceX";

            // Act
            var success = repo.GetById(ID);

            // Assert
            Assert.IsNotNull(success);
            Assert.AreEqual(expectedName, success.Name);
        }

        [TestMethod]
        public void TestGetProjectByIdFail()
        {
            // Arrange
            const int ID = 5;

            // Act
            var result = repo.GetById(ID);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void TestGetProjectsByUserId()
        {
            // Arrange
            const string userID = "test1";
            const int expectedCount = 3;

            // Act
            var result = repo.GetByUserId(userID);

            // Assert
            Assert.AreEqual(expectedCount, result.Count);
        }

        [TestMethod]
        public void TestGetProjectsByUserIdFail()
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
        public void TestGetProjectsByRootId()
        {
            // Arrange
            const int rootID = 1;
            const int expectedID = 1;

            // Act
            var result = repo.GetByRootId(rootID);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedID, result.ID);
        }

        [TestMethod]
        public void TestGetProjectsByRootIdFail()
        {
            // Arrange
            const int rootID = 0;

            // Act
            var result = repo.GetByRootId(rootID);

            // Assert
            Assert.IsNull(result);
        }
        #endregion

        #region Test project repo create method
        [TestMethod]
        public void TestCreateNewProject()
        {
            // Arrange
            const string projectName = "NewProject";
            ProjectViewModel newProject = new ProjectViewModel
            {
                ID = 999,
                Name = projectName,
                Collaborators = null,
                ApplicationUserID = "test1"
            };

            // Act
            var result = repo.Create(newProject);
            var project = repo.GetById(0);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(projectName, project.Name);
        }

        [TestMethod]
        public void TestCreateDuplicateProject()
        {
            // Arrange
            const int existingProjectID = 1;
            ProjectViewModel duplicateProject = new ProjectViewModel
            {
                ID = 999,
                Name = "SpaceX",
                Collaborators = null,
                ApplicationUserID = "test1"
            };

            // Act
            var result = repo.Create(duplicateProject);
            var existing = repo.GetById(existingProjectID);
            var duplicate = repo.GetById(0);

            // Assert
            Assert.IsTrue(result);
            Assert.AreNotEqual(duplicate.Name, existing.Name);
        }

        [TestMethod]
        public void TestCreateMultipleDuplicateProjects()
        {
            // Arrange
            const string duplicateName = "SpaceX";
            const string userID = "test1";
            ProjectViewModel duplicateProject = new ProjectViewModel
            {
                ID = 999,
                Name = duplicateName,
                Collaborators = null,
                ApplicationUserID = userID
            };

            // Act
            var result1 = repo.Create(duplicateProject);
            var result2 = repo.Create(duplicateProject);
            var projects = repo.GetByUserId(userID).Where(x => x.Name == duplicateName).ToList();
            
            // Assert
            Assert.IsTrue(result1);
            Assert.IsTrue(result1);
            Assert.AreEqual(1, projects.Count);
        }
        #endregion

        #region Test project repo delete method
        [TestMethod]
        public void TestDeleteProjectById()
        {
            // Arrange
            const int ID = 1;

            // Act
            var success = repo.DeleteById(ID);
            var project = repo.GetById(ID);

            // Assert
            Assert.IsTrue(success);
            Assert.IsNull(project);
        }

        [TestMethod]
        public void TestDeleteProjectByIdFail()
        {
            // Arrange
            const int ID = 0;

            // Act
            var result = repo.DeleteById(ID);

            // Assert
            Assert.IsFalse(result);
        }
        #endregion

        #region Test project repo update method
        [TestMethod]
        public void TestUpdateProjectWithNewName()
        {
            // Arrange
            const string newName = "TestUpdate";
            const int projectID = 1;
            ProjectViewModel targetProject = new ProjectViewModel
            {
                Name = newName,
                ID = projectID,
                Collaborators = null
            };

            // Act
            var success = repo.Update(targetProject);
            var result = repo.GetById(projectID);

            // Assert
            Assert.IsTrue(success);
            Assert.AreEqual(newName, result.Name);
        }

        [TestMethod]
        public void TestUpdateProjectWithDuplicateName()
        {
            // Arrange
            const string newName = "The new Enron";
            const int projectID = 1;
            ProjectViewModel targetProject = new ProjectViewModel
            {
                Name = newName,
                ID = projectID,
                Collaborators = null,
                ApplicationUserID = "test1"
            };

            // Act
            var success = repo.Update(targetProject);
            var result = repo.GetById(projectID);

            // Assert
            Assert.IsTrue(success);
            Assert.AreNotEqual(newName, result.Name);
        }

        [TestMethod]
        public void TestUpdateProjectWithMultipleDuplicateNames()
        {
            // Arrange
            const string newName = "The new Enron";
            const int firstID = 1;
            const int secondID = 3;
            ProjectViewModel duplicate1 = new ProjectViewModel
            {
                ID = firstID,
                Collaborators = null,
                Name = newName,
                ApplicationUserID = "test1"
            };
            ProjectViewModel duplicate2 = new ProjectViewModel
            {
                ID = secondID,
                Collaborators = null,
                Name = newName,
                ApplicationUserID = "test1"
            };

            // Act
            var success1 = repo.Update(duplicate1);
            var success2 = repo.Update(duplicate2);
            var result1 = repo.GetById(firstID);
            var result2 = repo.GetById(secondID);

            // Assert
            Assert.IsTrue(success1);
            Assert.IsTrue(success2);
            Assert.AreNotEqual(newName, result1.Name);
            Assert.AreNotEqual(newName, result2.Name);
        }
        #endregion
    }
}
