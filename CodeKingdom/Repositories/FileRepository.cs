using CodeKingdom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeKingdom.Repositories
{
    public class FileRepository
    {
        private readonly IAppDataContext db;

        public FileRepository(IAppDataContext context = null)
        {
            db = context ?? new ApplicationDbContext();
        }
    }
}