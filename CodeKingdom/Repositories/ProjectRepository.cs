using CodeKingdom.Models;
using CodeKingdom.Models.Entities;
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

        public List<Project> getAll(string userID)
        {

            var ret = from project in db.Projects
                      join collaborator in db.Collaborators
                      on project.ID equals collaborator.Project.ID
                      where collaborator.User.Id == userID
                      select project;

            return ret.ToList();
        }

    }
}