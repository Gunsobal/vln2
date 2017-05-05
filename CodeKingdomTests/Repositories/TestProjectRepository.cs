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
            const int successID = 1;
            const int failID = 5;
            const string expectedName = "SpaceX";

            // Act
            var success = repo.getById(successID);
            var fail = repo.getById(failID);

            // Assert
            Assert.IsNull(fail);
            Assert.IsNotNull(success);
            Assert.AreEqual(expectedName, success.Name);
        }

        [TestMethod]
        public void TestGetProjectsByUserId()
        {
            // Arrange
            const string successID = "test1";
            const string failID = "fail";
            const int expectedCount = 3;

            // Act
            var success = repo.getByUserId(successID);
            var fail = repo.getByUserId(failID);

            // Assert
            Assert.AreEqual(0, fail.Count);
            Assert.AreEqual(expectedCount, success.Count);
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
            var project = repo.getById(0);

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
            var existing = repo.getById(existingProjectID);
            var duplicate = repo.getById(0);

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
            var projects = repo.getByUserId(userID).Where(x => x.Name == duplicateName).ToList();
            
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
            const int successID = 1;
            const int failID = 0;

            // Act
            var success = repo.DeleteById(successID);
            var project = repo.getById(successID);
            var fail = repo.DeleteById(failID);

            // Assert
            Assert.IsTrue(success);
            Assert.IsNull(project);
            Assert.IsFalse(fail);
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
            var result = repo.getById(projectID);

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
            var result = repo.getById(projectID);

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
            var result1 = repo.getById(firstID);
            var result2 = repo.getById(secondID);

            // Assert
            Assert.IsTrue(success1);
            Assert.IsTrue(success2);
            Assert.AreNotEqual(newName, result1.Name);
            Assert.AreNotEqual(newName, result2.Name);
        }
        #endregion
    }
}
