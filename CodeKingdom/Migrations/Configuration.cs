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


            seedUser(context, "Gunso@mail.com");
            seedUser(context, "Gunso1@mail.com");
            seedUser(context, "Gunso2@mail.com");
            seedUser(context, "Gunso3@mail.com");
            seedUser(context, "unnsteinng@gmail.com");
            seedCollaboratorRoles(context);
            seedFolder(context, "Root1");
            seedFolder(context, "Root2");
            seedFolder(context, "Root3");
            seedFolder(context, "controllers", 1);
            seedFolder(context, "models", 1);
            seedFolder(context, "views", 1);
            seedFolder(context, "images", 2);
            seedFolder(context, "docs", 3);
            seedFolder(context, "bin", 3);
            seedFile(context, "index.html", 1, "unnsteinng@gmail.com");
            seedFile(context, "script.js", 1, "unnsteinng@gmail.com");
            seedFile(context, "style.css", 1, "unnsteinng@gmail.com");
            seedFile(context, "bubbiController.cs", 4, "unnsteinng@gmail.com");
            seedFile(context, "model.cs", 5, "unnsteinng@gmail.com");
            seedFile(context, "bubbi.html", 6, "unnsteinng@gmail.com");
            seedFile(context, "lame.html", 6, "unnsteinng@gmail.com");
            seedFile(context, "image.png", 7, "Gunso2@mail.com");
            seedFile(context, "image2.png", 7, "Gunso2@mail.com");
            seedFile(context, "image3.png", 7, "Gunso2@mail.com");
            seedFile(context, "image4.png", 7, "Gunso2@mail.com");
            seedFile(context, "doc.txt", 8, "Gunso3@mail.com");
            seedFile(context, "doc2.txt", 8, "Gunso3@mail.com");
            seedFile(context, "doc3.txt", 8, "Gunso3@mail.com");
            seedFile(context, "bin.obj", 9, "Gunso3@mail.com");
            seedProject(context, "projectNron", 1);
            seedProject(context, "projectNrond", 2);
            seedProject(context, "projectNron3", 3);
            seedCollaborators(context, 1, "unnsteinng@gmail.com", 1);
            seedCollaborators(context, 1, "Gunso@mail.com", 2);
            seedCollaborators(context, 1, "Gunso2@mail.com", 2);
            seedCollaborators(context, 2, "Gunso@mail.com", 1);
            seedCollaborators(context, 3, "Gunso@mail.com", 1);
        }

        private void seedUser(ApplicationDbContext context, string email)
        {
            if (!context.Roles.Any(r => r.Name == "Normal"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Normal" };

                manager.Create(role);
            }
            if (!context.Users.Any(u => u.UserName == email))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser { UserName = email, Email = email };

                manager.Create(user, "P@ssword123");
                manager.AddToRole(user.Id, "Normal");
            }
        }
        private void seedCollaboratorRoles(ApplicationDbContext context)
        {
            context.CollaboratorRoles.AddOrUpdate(r => r.Name,
                new CollaboratorRole { Name = "Owner" },
                new CollaboratorRole { Name = "Reader" },
                new CollaboratorRole { Name = "Member" });
            context.SaveChanges();
        }
        private void seedFolder(ApplicationDbContext context, string name, int? parent_id = null)
        {
            context.Folders.AddOrUpdate(f => f.Name,
                new Folder { Name = name, Parent = parent_id.HasValue ? context.Folders.Find(parent_id) : null });
            context.SaveChanges();
        }
        private void seedFile(ApplicationDbContext context, string name, int folder_id, string username)
        {
            Folder folder = context.Folders.Find(folder_id);

            var store = new UserStore<ApplicationUser>(context);
            var manager = new UserManager<ApplicationUser>(store);
            var user = manager.FindByName(username);

            if (folder != null && user != null)
            {
                
                context.Files.AddOrUpdate(file => file.Name, new File
                {
                    Name = name,
                    Content = "Lorem Ipsum",
                    Type = name.Substring(name.IndexOf('.') + 1),
                    Folder = folder,
                    Owner = context.Users.Find(user.Id)
                });
                context.SaveChanges();
            }
        }
        private void seedProject(ApplicationDbContext context, string name, int root_id)
        {
            Folder folder = context.Folders.Find(root_id);

            if (folder != null)
            {
                context.Projects.AddOrUpdate(p => p.Name,
                    new Project { Name = name, Root = folder, Frozen = false });
                context.SaveChanges();
            }
        }
        private void seedCollaborators(ApplicationDbContext context, int project_id, string username, int role_id)
        {
            var store = new UserStore<ApplicationUser>(context);
            var manager = new UserManager<ApplicationUser>(store);
            var user = manager.FindByName(username);
            var project = context.Projects.Find(project_id);
            var role = context.CollaboratorRoles.Find(role_id);
            if (user != null && project != null && role != null)
            {
                context.Collaborators.AddOrUpdate(c => new { c.Project.ID, c.User.Id },
                    new Collaborator { Project = project, Role = role, User = user });
            }
            context.SaveChanges();
        }
    }
}
