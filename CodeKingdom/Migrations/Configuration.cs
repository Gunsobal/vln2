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


            seedUser(context, "Gunso", "Gunso@mail.com");
            seedUser(context, "Gunso1", "Gunso1@mail.com");
            seedUser(context, "Gunso2", "Gunso2@mail.com");
            seedUser(context, "Gunso3", "Gunso3@mail.com");
            seedCollaboratorRoles(context);
            seedFolder(context, "controllers", 4);
            seedFolder(context, "models", 4);
            seedFolder(context, "views", 4);
            seedFolder(context, "images", 5);
            seedFolder(context, "docs", 6);
            seedFolder(context, "bin", 6);

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
        private void seedCollaboratorRoles(ApplicationDbContext context)
        {
            CollaboratorRole role1 = new CollaboratorRole { Name = "Owner" };
            CollaboratorRole role2 = new CollaboratorRole { Name = "Reader" };
            CollaboratorRole role3 = new CollaboratorRole { Name = "Member" };
            context.CollaboratorRoles.AddOrUpdate(role1);
            context.CollaboratorRoles.AddOrUpdate(role2);
            context.CollaboratorRoles.AddOrUpdate(role3);
            context.SaveChanges();
        }
        private void seedFolder(ApplicationDbContext context, string name, int? parent_id = null)
        {
            Folder folder = new Folder { Name = name };
            if (parent_id.HasValue)
            {
                folder.Parent = context.Folders.Find(parent_id);
            }
            context.Folders.Add(folder);
            context.SaveChanges();
        }
        private void seedFile(ApplicationDbContext context, string name, string content, int folder_id, int user_id)
        {
            Folder folder = context.Folders.Find(folder_id);
            ApplicationUser user = context.Users.Find(user_id);

            if (folder != null && user != null)
            {
                File file = new File
                {
                    Name = name,
                    Content = content,
                    Folder = context.Folders.Find(folder_id),
                    Owner = context.Users.Find(user_id)
                };
                context.Files.Add(file);
                context.SaveChanges();
            }
        }
    }
}
