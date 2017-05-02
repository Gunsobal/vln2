using CodeKingdom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeKingdom.Repositories
{
    public class UserRepository
    {
        private readonly IAppDataContext db;

        public UserRepository(IAppDataContext context = null)
        {
            db = context ?? new ApplicationDbContext();
        }
    }
}