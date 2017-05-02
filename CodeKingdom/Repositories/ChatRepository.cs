using CodeKingdom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeKingdom.Repositories
{
    public class ChatRepository
    {
        private readonly IAppDataContext db;

        public ChatRepository(IAppDataContext context = null)
        {
            db = context ?? new ApplicationDbContext();
        }
    }
}