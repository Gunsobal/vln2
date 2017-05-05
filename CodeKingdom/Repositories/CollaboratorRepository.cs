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
            return db.Collaborators.Find(id);
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

            if (collaborator == null)
            {
                return false;
            }

            collaborator.CollaboratorRoleID = model.RoleID;
            db.SaveChanges();

            return true;
        }

        public bool Create(CollaboratorViewModel model)
        {
            ApplicationUser user = userRepository.GetByUserName(model.UserName);

            if (user == null)
            {
                return false;
            }

            if (IsInProject(user.Id, model.ProjectID))
            {
                return false;
            }

            db.Collaborators.Add(new Collaborator
            {
                ApplicationUserID = user.Id,
                ProjectID = model.ProjectID,
                CollaboratorRoleID = model.RoleID,
            });
            db.SaveChanges();
            return true;
        }

        public bool Delete(int id)
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
    }
}