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
        private FolderRepository folderRepo = new FolderRepository();

        public void Get(string fileId, int projectId)
        {
            var correctId = 0;
            int.TryParse(fileId, out correctId);
            var file = business.GetFileByID(correctId, projectId);

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

        public void DeleteFolder(int projectID, int folderID)
        {
            folderRepo.DeleteById(folderID);
            Clients.Group(Convert.ToString(projectID)).DeleteFolder(folderID);
        }

        public void RenameFolder(int projectID, int folderID, string newName)
        {
            FolderViewModel model = new FolderViewModel
            {
                ID = folderID,
                Name = newName,
                ProjectID = projectID
            };
            folderRepo.Rename(model);
        }
    }
}