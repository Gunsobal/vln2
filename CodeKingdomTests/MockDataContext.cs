using System.Data.Entity;
using CodeKingdom.Models;
using CodeKingdom.Models.Entities;

namespace CodeKingdomTests
{
    class MockDataContext : IAppDataContext
    {
        /// <summary>
        /// Sets up the fake database.
        /// </summary>
        public MockDataContext()
        {
            // We're setting our DbSets to be InMemoryDbSets rather than using SQL Server.
            this.Chats = new InMemoryDbSet<Chat>();
            this.Collaborators = new InMemoryDbSet<Collaborator>();
            this.CollaboratorRoles = new InMemoryDbSet<CollaboratorRole>();
            this.Files = new InMemoryDbSet<File>();
            this.Folders = new InMemoryDbSet<Folder>();
            this.Projects = new InMemoryDbSet<Project>();
            this.UserConfigurations = new InMemoryDbSet<UserConfiguration>();
            this.Users = new InMemoryDbSet<ApplicationUser>();
        }

        public IDbSet<Chat> Chats { get; set; }
        public IDbSet<Collaborator> Collaborators { get; set; }
        public IDbSet<CollaboratorRole> CollaboratorRoles { get; set; }
        public IDbSet<File> Files { get; set; }
        public IDbSet<Folder> Folders { get; set; }
        public IDbSet<Project> Projects { get; set; }
        public IDbSet<UserConfiguration> UserConfigurations { get; set; }
        public IDbSet<ApplicationUser> Users { get; set; }

        public int SaveChanges()
        {
            // Pretend that each entity gets a database id when we hit save.
            int changes = 0;

            return changes;
        }

        public void Dispose()
        {
            // Do nothing!
        }
    }
}
