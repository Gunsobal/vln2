using CodeKingdom.Models;
using CodeKingdom.Models.Entities;
using CodeKingdom.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeKingdom.Repositories
{
    public class ChatRepository
    {
        private readonly IAppDataContext db;
        private UserRepository userRepository = new UserRepository();

        public ChatRepository(IAppDataContext context = null)
        {
            db = context ?? new ApplicationDbContext();
        }

        public List<Chat> GetByProjectId(int id)
        {
            return db.Chats.Where(x => x.ProjectID == id).ToList();
        }

        public void Save(ChatViewModel viewModel)
        {
            ApplicationUser user = userRepository.GetByEmail(viewModel.Username);
            Chat chat = new Chat
            {
                Message = viewModel.Message,
                DateTime = viewModel.DateTime,
                ProjectID = viewModel.ProjectID,
                ApplicationUserID = user.Id
            };
            db.Chats.Add(chat);
            db.SaveChanges();
        }

        public void ClearChat(int id)
        {
            var chats = db.Chats.Where(x => x.ProjectID == id);
            foreach(var chat in chats)
            {
                db.Chats.Remove(chat);
            }
            db.SaveChanges();
        }
    }
}