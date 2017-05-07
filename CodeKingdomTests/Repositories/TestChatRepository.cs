using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using CodeKingdom.Repositories;
using CodeKingdom.Models.Entities;

namespace CodeKingdomTests.Repositories
{
    [TestClass]
    public class TestChatRepository
    {
        private ChatRepository repo;

        #region Initialize
        [TestInitialize]
        public void Initialize()
        {
            MockDataContext mockDb = new MockDataContext();
            TestSeed.Chats(mockDb);
            repo = new ChatRepository(mockDb);
        }
        #endregion

        #region Test get methods
        [TestMethod]
        public void TestGetChatsByProjectId()
        {
            // Arrange
            const int projectID = 1;
            const int expectedCount = 2;

            // Act
            var result = repo.GetByProjectId(projectID);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedCount, result.Count);
            foreach (Chat chat in result)
            {
                Assert.AreEqual(projectID, chat.ProjectID);
            }
        }

        [TestMethod]
        public void TestGetChatsByProjectIdFail()
        {
            // Arrange
            const int projectID = 0;

            // Act
            var result = repo.GetByProjectId(projectID);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }
        #endregion
    }
}
