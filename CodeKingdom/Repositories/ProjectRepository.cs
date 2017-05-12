using CodeKingdom.Models;
using CodeKingdom.Models.Entities;
using CodeKingdom.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeKingdom.Repositories
{
	public class ProjectRepository
	{
        private readonly IAppDataContext db;

        public ProjectRepository(IAppDataContext context = null)
        {
            db = context ?? new ApplicationDbContext();
        }

        /// <summary>
        /// Returns null or single instance of project
        /// </summary>
        /// <param name="ID">Project ID</param>
        public Project getById(int ID)
        {
            var result = (from project in db.Projects
                          where project.ID == ID
                          select project).FirstOrDefault();

            return result;
        }

        /// <summary>
        /// Returns null or single instance of project by folder root ID
        /// </summary>
        /// <param name="ID">Folder ID</param>
        public Project GetByRootId(int ID)
        {
            return db.Projects.Where(x => x.FolderID == ID).FirstOrDefault();
        }

        /// <summary>
        /// Returns list of projects by user, returns empty list if no projects are found
        /// </summary>
        /// <param name="userID">User ID</param>
        public List<Project> getByUserId(string userID)
        {

            var ret = from project in db.Projects
                      join collaborator in db.Collaborators
                      on project.ID equals collaborator.ProjectID
                      where collaborator.ApplicationUserID == userID
                      select project;

            return ret.ToList();
        }

        /// <summary>
        /// Creates a project in database and ensures unique name, creates user as collaborator with role as owner, a root folder for project and default index file
        /// </summary>
        /// <param name="model">User ID, Name</param>
        public bool Create(ProjectViewModel model)
        {
            // Check for duplicate names
            if (getByUserId(model.ApplicationUserID).Where(x => x.Name == model.Name).ToList().Count != 0)
            {
                model.Name += "Copy";
                return Create(model);
            }

            CollaboratorRole role = db.CollaboratorRoles.Where(cr => cr.Name == "Owner").FirstOrDefault();

            Folder root = new Folder
            {
                Name = model.Name + "root"
            };

            File file = new File
            {
                Name = "index.js",
                Content = "",
                Type = "Javascript",
                Folder = root,
                ApplicationUserID = model.ApplicationUserID
            };

            Project project = new Project
            {
                Name = model.Name,
                Frozen = false,
                Root = root
            };

            Collaborator collaborator = new Collaborator
            {
                ApplicationUserID = model.ApplicationUserID,
                Project = project,
                Role = role
            };

            db.Folders.Add(root);
            db.Files.Add(file);
            db.Projects.Add(project);
            db.Collaborators.Add(collaborator);
            db.SaveChanges();
            file.ProjectID = project.ID;
            db.SaveChanges();
            return true;
        }

        /// <summary>
        /// Removes a single project from database and all files associated with it --NOT FOLDERS!
        /// </summary>
        /// <param name="id">Project ID</param>
        public bool DeleteById(int id)
        {
            Project project = getById(id);

            if (project == null)
            {
                return false;
            }

            List<File> files = db.Files.Where(x => x.ProjectID == id).ToList();
            foreach (var file in files)
            {
                db.Files.Remove(file);
            }

            db.Projects.Remove(project);
            db.SaveChanges();

            return true;
        }

        /// <summary>
        /// Updates project name and ensures it's unique for user. Returns false if project doesn't exist, true otherwise.
        /// </summary>
        /// <param name="model">User ID, Name</param>
        public bool Update(ProjectViewModel model)
        {
            Project project = getById(model.ID);
            
            if (project == null)
            {
                return false;
            }

            // Check for duplicate name
            if (getByUserId(model.ApplicationUserID).Where(x => x.Name == model.Name).ToList().Count != 0)
            {
                model.Name += "Copy";
                return Update(model);
            }

            project.Name = model.Name;
            db.SaveChanges();

            return true;
        }
    }
}