using CodeKingdom.Models;

namespace CodeKingdom.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models.Entities;
    using System;
    using System.Collections.Generic;
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

            //CodeKingdomTest User Seeds
            //seedUser(context, "test_user_one@test.com");
            //seedUser(context, "test_user_two@test.com");
            
            //CodeKingdom Seeds
            seedUser(context, "Gunso@mail.com");
            seedUser(context, "Gunso1@mail.com");
            seedUser(context, "Gunso2@mail.com");
            seedUser(context, "Gunso3@mail.com");
            seedUser(context, "unnsteinng@gmail.com");
            seedCollaboratorRoles(context);
            seedFolder(context, "Root1");
            seedFolder(context, "Root2");
            seedFolder(context, "Root3");
            seedFolder(context, "controllers", context.Folders.Where(x => x.Name == "Root1").FirstOrDefault());
            seedFolder(context, "models", context.Folders.Where(x => x.Name == "Root1").FirstOrDefault());
            seedFolder(context, "views", context.Folders.Where(x => x.Name == "Root1").FirstOrDefault());
            seedFolder(context, "images", context.Folders.Where(x => x.Name == "Root1").FirstOrDefault());
            seedFolder(context, "docs");
            seedFolder(context, "bin", context.Folders.Where(x => x.Name == "models").FirstOrDefault());

            int project1 = seedProject(context, "projectNron", context.Folders.Where(x => x.Name == "Root1").Single().ID);
            int project2 = seedProject(context, "projectNrond", context.Folders.Where(x => x.Name == "Root2").Single().ID);
            int project3 = seedProject(context, "projectNron3", context.Folders.Where(x => x.Name == "Root3").Single().ID);

            seedFile(context, "index1.html", context.Folders.Where(x => x.Name == "Root1").Single().ID, "unnsteinng@gmail.com", project1);
            seedFile(context, "script.js", context.Folders.Where(x => x.Name == "Root1").Single().ID, "unnsteinng@gmail.com", project1);
            seedFile(context, "style.css", context.Folders.Where(x => x.Name == "Root1").Single().ID, "unnsteinng@gmail.com", project1);
            seedFile(context, "index2.js", context.Folders.Where(x => x.Name == "Root2").Single().ID, "Gunso2@mail.com", project2);
            seedFile(context, "index3.js", context.Folders.Where(x => x.Name == "Root3").Single().ID, "Gunso3@mail.com", project3);
            seedFile(context, "bubbiController.cs", context.Folders.Where(x => x.Name == "controllers").Single().ID, "unnsteinng@gmail.com", project1);
            seedFile(context, "model.cs", context.Folders.Where(x => x.Name == "models").Single().ID, "unnsteinng@gmail.com", project1);
            seedFile(context, "bubbi.html", context.Folders.Where(x => x.Name == "views").Single().ID, "unnsteinng@gmail.com", project1);
            seedFile(context, "lame.html", context.Folders.Where(x => x.Name == "views").Single().ID, "unnsteinng@gmail.com", project1);
            seedFile(context, "image.png", context.Folders.Where(x => x.Name == "images").Single().ID, "Gunso2@mail.com", project1);
            seedFile(context, "image2.png", context.Folders.Where(x => x.Name == "images").Single().ID, "Gunso2@mail.com", project1);
            seedFile(context, "image3.png", context.Folders.Where(x => x.Name == "images").Single().ID, "Gunso2@mail.com", project1);
            seedFile(context, "image4.png", context.Folders.Where(x => x.Name == "images").Single().ID, "Gunso2@mail.com", project1);
            seedFile(context, "doc.txt", context.Folders.Where(x => x.Name == "docs").Single().ID, "Gunso3@mail.com", project1);
            seedFile(context, "doc2.txt", context.Folders.Where(x => x.Name == "docs").Single().ID, "Gunso3@mail.com", project1);
            seedFile(context, "doc3.txt", context.Folders.Where(x => x.Name == "docs").Single().ID, "Gunso3@mail.com", project1);
            seedFile(context, "bin.obj", context.Folders.Where(x => x.Name == "bin").Single().ID, "Gunso3@mail.com", project1);
            seedFile(context, "index4.js", context.Folders.Where(x => x.Name == "Root2").Single().ID, "Gunso2@mail.com", project1);
            seedFile(context, "index5.js", context.Folders.Where(x => x.Name == "Root3").Single().ID, "Gunso3@mail.com", project1);
            
            seedCollaborators(context, context.Projects.Where(x => x.Name == "projectNron").Single().ID, "unnsteinng@gmail.com", 1);
            seedCollaborators(context, context.Projects.Where(x => x.Name == "projectNron").Single().ID, "Gunso@mail.com", 1);
            seedCollaborators(context, context.Projects.Where(x => x.Name == "projectNron").Single().ID, "Gunso2@mail.com", 1);
            seedCollaborators(context, context.Projects.Where(x => x.Name == "projectNrond").Single().ID, "Gunso@mail.com", 2);
            seedCollaborators(context, context.Projects.Where(x => x.Name == "projectNron3").Single().ID, "Gunso2@mail.com", 2);
            seedChats(context, "Hi", context.Projects.Where(x => x.Name == "projectNron").Single().ID, "unnsteinng@gmail.com");
            seedChats(context, "Hi 2", context.Projects.Where(x => x.Name == "projectNron").Single().ID, "Gunso@mail.com");
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
        private void seedFolder(ApplicationDbContext context, string name, Folder parent = null)
        {
            Folder folder = new Models.Entities.Folder();
            if (parent != null)
            {
                folder.FolderID = parent.ID;
            }
            folder.Name = name;
            context.Folders.AddOrUpdate(f => f.Name, folder);
            context.SaveChanges();
        }
        private void seedFile(ApplicationDbContext context, string name, int folder_id, string username, int projectId)
        {
            Folder folder = context.Folders.Where(x => x.ID == folder_id).FirstOrDefault();

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
                    FolderID = folder.ID,
                    ApplicationUserID = user.Id,
                    ProjectID = projectId
                });
                context.SaveChanges();
            }
        }
        private int seedProject(ApplicationDbContext context, string name, int root_id)
        {
            Folder folder = context.Folders.Find(root_id);
            Project project = new Project { Name = name, FolderID = folder.ID, Frozen = false };

            if (folder != null)
            {
                context.Projects.AddOrUpdate(p => p.Name, project);
                context.SaveChanges();
            }

            return project.ID;
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
                IEnumerable<Collaborator> collabs = context.Collaborators.Where(c => c.User.UserName == username && c.Project.ID == project_id);
                if (collabs.Count() == 0)
                {
                    context.Collaborators.Add(new Collaborator { Project = project, User = user, Role = role });
                }
            }
            context.SaveChanges();
        }
        private void seedChats(ApplicationDbContext context, string message, int project_id, string username)
        {
            var store = new UserStore<ApplicationUser>(context);
            var manager = new UserManager<ApplicationUser>(store);
            var user = manager.FindByName(username);
            var project = context.Projects.Find(project_id);

            if (user != null && project != null)
            {
                IEnumerable<Chat> chats = context.Chats.Where(c => c.User.UserName == username && c.Project.ID == project_id);
                if (chats.Count() == 0)
                {
                    context.Chats.Add(new Chat { Message = message, ProjectID = project.ID, ApplicationUserID = user.Id, DateTime = DateTime.Now });
                }
                context.SaveChanges();
            }
        }
    }
}
