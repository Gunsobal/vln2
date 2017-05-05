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

        public Project getById(int ID)
        {
            var result = (from project in db.Projects
                          where project.ID == ID
                          select project).FirstOrDefault();

            return result;
        }

        public List<Project> getByUserId(string userID)
        {

            var ret = from project in db.Projects
                      join collaborator in db.Collaborators
                      on project.ID equals collaborator.ProjectID
                      where collaborator.ApplicationUserID == userID
                      select project;

            return ret.ToList();
        }

        public bool Create(ProjectViewModel model)
        {
            // Check for duplicate names
            if (getByUserId(model.ApplicationUserID).Where(x => x.Name == model.Name).ToList().Count != 0)
            {
                model.Name += "Copy";
                Create(model);
                return true;
            }

            CollaboratorRole role = db.CollaboratorRoles.Where(cr => cr.Name == "Owner").FirstOrDefault();

            Folder root = new Folder
            {
                Name = model.Name + "root"
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
            db.Projects.Add(project);
            db.Collaborators.Add(collaborator);
            db.SaveChanges();

            return true;
        }

        public bool DeleteById(int id)
        {
            Project project = getById(id);

            if (project == null)
            {
                return false;
            }

            db.Projects.Remove(project);
            db.SaveChanges();

            return true;
        }

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
                Update(model);
                return true;
            }

            project.Name = model.Name;
            db.SaveChanges();

            return true;
        }
    }
}