using CodeKingdom.Models;

namespace CodeKingdom.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models.Entities;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<CodeKingdom.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            

            Folder folder = new Folder
            {
                Name = "RootFolder"
            };

            context.Folders.AddOrUpdate(
                f => f.Name,
                folder
            );

            context.SaveChanges();

            Project pro = new Project
            {
                Name = "Project",
                Root = folder,
                Frozen = false
            };

            context.Projects.AddOrUpdate(
                p => p.Name,
                pro
            );

            context.SaveChanges();

            Collaborator borabora = new Collaborator
            {
                User = context.Users.First(),

                Project = context.Projects.First()
            };

            context.Collaborators.AddOrUpdate(
                c => c.ID,
                borabora
                );
            
            context.SaveChanges();
        }

        private void seedUser(ApplicationDbContext context, string username, string email)
        {
            if (!context.Roles.Any(r => r.Name == "Normal"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Normal" };

                manager.Create(role);
            }
            if (!context.Users.Any(u => u.UserName == username))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser { UserName = username, Email = email };

                manager.Create(user, "P@ssword123");
                manager.AddToRole(user.Id, "Normal");
            }
        }
    }
}
