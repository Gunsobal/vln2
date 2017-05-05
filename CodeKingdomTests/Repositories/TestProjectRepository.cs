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

        [TestInitialize]
        public void Initialize()
        {
            var mockDb = new MockDataContext();
            TestSeed.All(mockDb);
            repo = new ProjectRepository(mockDb);
        }

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
            const int expectedCount = 1;

            // Act
            var success = repo.getByUserId(successID);
            var fail = repo.getByUserId(failID);

            // Assert
            Assert.AreEqual(0, fail.Count);
            Assert.AreEqual(expectedCount, success.Count);
        }

        [TestMethod]
        public void TestCreateNewProject()
        {
            // Arrange
            const string projectName = "NewProject";
            const string userID = "test1";
            ProjectViewModel newProject = new ProjectViewModel
            {
                ID = 999,
                Name = projectName,
                Collaborators = null
            };

            // Act
            var result = repo.Create(newProject, userID);
            var project = repo.getById(0);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(projectName, project.Name);
        }

        [TestMethod]
        public void TestCreateDuplicateProject()
        {
            // Arrange
            const string userID = "test1";
            const int existingProjectID = 1;
            ProjectViewModel duplicateProject = new ProjectViewModel
            {
                ID = 999,
                Name = "SpaceX",
                Collaborators = null
            };

            // Act
            var result = repo.Create(duplicateProject, userID);
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
            const string userID = "test1";
            const string duplicateName = "SpaceX";
            ProjectViewModel duplicateProject = new ProjectViewModel
            {
                ID = 999,
                Name = duplicateName,
                Collaborators = null
            };

            // Act
            var result1 = repo.Create(duplicateProject, userID);
            var result2 = repo.Create(duplicateProject, userID);
            var projects = repo.getByUserId(userID).Where(x => x.Name == duplicateName).ToList();
            
            // Assert
            Assert.IsTrue(result1);
            Assert.IsTrue(result1);
            Assert.AreEqual(1, projects.Count);
        }

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
                Collaborators = null
            };

            // Act
            var success = repo.Update(targetProject);
            var result = repo.getById(projectID);

            // Assert
            Assert.IsTrue(success);
            Assert.AreNotEqual(newName, result.Name);
        }
    }
}
