using CodeKingdom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeKingdom.Repositories
{
    public class FolderRepository
    {
        private readonly IAppDataContext db;

        public FolderRepository(IAppDataContext context = null)
        {
            db = context ?? new ApplicationDbContext();
        }
    }
}