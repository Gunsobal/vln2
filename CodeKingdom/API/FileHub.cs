using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using CodeKingdom.Business;
using CodeKingdom.Models.Entities;
using CodeKingdom.Repositories;
using CodeKingdom.Models.ViewModels;

namespace CodeKingdom.API
{
    public class FileHub : Hub
    {
        private ProjectStructure business = new ProjectStructure();
        private FileRepository repo = new FileRepository();

        // Is this still being used somewere?
        public void Get(string id)
        {
            var correctId = 0;
            int.TryParse(id, out correctId);
            // var file = business.GetFileByID(correctId);
            File file = null;
            // TODO: Change file types in database so that they match with ace editor
            var type = "";
            if (file != null)
                {
                if (file.Type == "js")
                {
                    type = "javascript";
                }
                else
                {
                    type = file.Type;
                }
                Clients.Caller.ReturnFile(file.ID, file.Content, file.Type);
            }
        }

        public void JoinProject(int id)
        {
            Groups.Add(Context.ConnectionId, Convert.ToString(id));
            Clients.Group(Convert.ToString(id)).Testing(id);
        }

        public void LeaveProject(int id)
        {
            Groups.Remove(Context.ConnectionId, Convert.ToString(id));
        }

        public void DeleteFile(int id, int fileID)
        {
            repo.DeleteById(fileID);
            Clients.Group(Convert.ToString(id)).RemoveFile(fileID);
        }

        public void RenameFile(int id, int fileID, string newName)
        {
            FileViewModel model = new FileViewModel
            {
                ID = fileID,
                Name = newName,
                ProjectID = id
            };
            repo.Rename(model);
            Clients.Group(Convert.ToString(id)).RenameFile(fileID, newName);
        }
    }
}