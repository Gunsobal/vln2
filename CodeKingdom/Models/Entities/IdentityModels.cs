using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using CodeKingdom.Models.Entities;

namespace CodeKingdom.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public interface IAppDataContext
    {
        IDbSet<Chat> Chats { get; set; }
        IDbSet<Collaborator> Collaborators { get; set; }
        IDbSet<CollaboratorRole> CollaboratorRoles { get; set; }
        IDbSet<File> Files { get; set; }
        IDbSet<Folder> Folders { get; set; }
        IDbSet<Project> Projects { get; set; }
        IDbSet<UserConfiguration> UserConfigurations { get; set; }
        int SaveChanges();
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IAppDataContext
    {
        public IDbSet<Chat> Chats { get; set; }
        public IDbSet<Collaborator> Collaborators { get; set; }
        public IDbSet<CollaboratorRole>  CollaboratorRoles { get; set; }
        public IDbSet<File> Files { get; set; }
        public IDbSet<Folder> Folders { get; set; }
        public IDbSet<Project> Projects { get; set; }
        public IDbSet<UserConfiguration> UserConfigurations { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        //public System.Data.Entity.DbSet<CodeKingdom.Models.ApplicationUser> ApplicationUsers { get; set; }
    }
}