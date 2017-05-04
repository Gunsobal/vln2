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

        public CollaboratorRepository(IAppDataContext context = null)
        {
            db = context ?? new ApplicationDbContext();
        }

        public Collaborator GetById(int id)
        {
            return db.Collaborators.Find(id);
        }

        public List<Collaborator> GetByProjectId(int id)
        {
            return db.Collaborators.Where(x => x.Project.ID == id).ToList();
        }

        public List<Collaborator> GetByUserId(string id)
        {
            return db.Collaborators.Where(x => x.User.Id == id).ToList();
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

        public List<CollaboratorRole> GetAllRoles()
        {
            return db.CollaboratorRoles.ToList();
        }
    }
}