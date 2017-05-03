using CodeKingdom.Models;
using CodeKingdom.Models.Entities;
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

        public List<Collaborator> GetByProjectId(int id)
        {
            return db.Collaborators.Where(x => x.Project.ID == id).ToList();
        }

        public List<Collaborator> GetByUserId(string id)
        {
            return db.Collaborators.Where(x => x.User.Id == id).ToList();
        }
    }
}