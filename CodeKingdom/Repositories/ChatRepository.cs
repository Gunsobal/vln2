using CodeKingdom.Models;
using CodeKingdom.Models.Entities;
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

        public List<Chat> GetByProjectId(int id)
        {
            return db.Chats.Where(x => x.Project.ID == id).ToList();
        } 
    }
}