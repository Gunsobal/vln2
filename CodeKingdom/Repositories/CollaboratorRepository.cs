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

        public Collaborator GetById(int id)
        {
            return db.Collaborators.Where(x => x.ID == id).FirstOrDefault();
        }

        public List<Collaborator> GetByProjectId(int id)
        {
            return db.Collaborators.Where(x => x.ProjectID == id).ToList();
        }

        public List<Collaborator> GetByUserId(string id)
        {
            return db.Collaborators.Where(x => x.ApplicationUserID == id).ToList();
        }

        public Collaborator GetByUserIdAndProjectId(string userID, int projectID)
        {

            return db.Collaborators.Where(collaborator => 
                collaborator.ApplicationUserID == userID && collaborator.ProjectID == projectID
            ).FirstOrDefault();
        }

        public bool IsInProject(string userID, int projectID)
        {
            return (GetByUserId(userID).Where(collaborator => collaborator.ProjectID == projectID).FirstOrDefault() != null);
        }

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

        public List<CollaboratorRole> GetAllRoles()
        {
            return db.CollaboratorRoles.ToList();
        }

        public CollaboratorRole GetRoleById(int id)
        {
            return db.CollaboratorRoles.Where(x => x.ID == id).FirstOrDefault();
        }
    }
}