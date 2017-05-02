using CodeKingdom.Models;
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

    }
}