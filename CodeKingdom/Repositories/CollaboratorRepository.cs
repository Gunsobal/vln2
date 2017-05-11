using CodeKingdom.Models;
using CodeKingdom.Models.Entities;
using CodeKingdom.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace CodeKingdom.Repositories
{
    public class CollaboratorRepository
    {
        private readonly IAppDataContext db;
        private readonly UserRepository userRepository;

        public CollaboratorRepository(IAppDataContext context = null)
        {
            db = context ?? new ApplicationDbContext();
            userRepository = new UserRepository();
        }

        /// <summary>
        /// Returns null or single instance of a collaborator
        /// </summary>
        /// <param name="id">Collaborator ID</param>
        public Collaborator GetById(int id)
        {
            return db.Collaborators.Where(x => x.ID == id).FirstOrDefault();
        }

        /// <summary>
        /// Returns list of collaborators by project ID, empty list if no collaborators are found
        /// </summary>
        /// <param name="id">Project ID</param>
        public List<Collaborator> GetByProjectId(int id)
        {
            return db.Collaborators.Where(x => x.ProjectID == id).ToList();
        }

        /// <summary>
        /// Returns list of collaborators by user ID, empty list if no collaborators are found
        /// </summary>
        /// <param name="id">User ID</param>
        public List<Collaborator> GetByUserId(string id)
        {
            return db.Collaborators.Where(x => x.ApplicationUserID == id).ToList();
        }

        /// <summary>
        /// Returns null or single instance of a collaborator by user and project ID
        /// </summary>
        /// <param name="userID">User ID</param>
        /// <param name="projectID">Project ID</param>
        /// <returns></returns>
        public Collaborator GetByUserIdAndProjectId(string userID, int projectID)
        {

            return db.Collaborators.Where(collaborator => 
                collaborator.ApplicationUserID == userID && collaborator.ProjectID == projectID
            ).FirstOrDefault();
        }

        /// <summary>
        /// Returns true if user exists as collaborator for a specific project, false otherwise
        /// </summary>
        /// <param name="userID">User ID</param>
        /// <param name="projectID">Project ID</param>
        public bool IsInProject(string userID, int projectID)
        {
            return (GetByUserId(userID).Where(collaborator => collaborator.ProjectID == projectID).FirstOrDefault() != null);
        }

        /// <summary>
        /// Updates collaborator role for a specific collaborator. Returns false if collaborator or role is not found, true otherwise
        /// </summary>
        /// <param name="model">Collaborator ID, Role ID</param>
        public bool Update(CollaboratorViewModel model)
        {
            Collaborator collaborator = this.GetById(model.ID);
            CollaboratorRole role = this.GetRoleById(model.RoleID);

            if (collaborator == null || role == null)
            {
                return false;
            }

            collaborator.CollaboratorRoleID = role.ID;
            db.SaveChanges();

            return true;
        }

        /// <summary>
        /// Stores a single instance of a collaborator in database. Returns false if user already exists as collaborator, true otherwise
        /// </summary>
        /// <param name="model">User ID, Project ID</param>
        public bool Create(Collaborator model)
        {
            if (IsInProject(model.ApplicationUserID, model.ProjectID))
            {
                return false;
            }

            db.Collaborators.Add(model);
            db.SaveChanges();
            return true;
        }

        /// <summary>
        /// Deletes a single instance of a collaborator from database by collaborator id. Returns false if collaborator doesn't exist, true otherwise
        /// </summary>
        /// <param name="id">Collaborator ID</param>
        public bool DeleteById(int id)
        {
            Collaborator collaborator = GetById(id);
            if (collaborator == null)
            {
                return false;
            }

            db.Collaborators.Remove(collaborator);
            db.SaveChanges();
            return true;
        }

        /// <summary>
        /// Returns all collaborator roles as list
        /// </summary>
        public List<CollaboratorRole> GetAllRoles()
        {
            return db.CollaboratorRoles.ToList();
        }

        /// <summary>
        /// Returns null or single instance of a collaborator role by id
        /// </summary>
        /// <param name="id">Collaborator Role ID</param>
        public CollaboratorRole GetRoleById(int id)
        {
            return db.CollaboratorRoles.Where(x => x.ID == id).FirstOrDefault();
        }
    }
}