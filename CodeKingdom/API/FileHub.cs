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

        /// <summary>
        /// Returns file for user
        /// </summary>
        /// <param name="fileId">File ID</param>
        /// <param name="projectId">Project ID</param>
        public void Get(string fileId, int projectId)
        {
            var correctId = 0;
            int.TryParse(fileId, out correctId);
            var file = business.GetFileByID(correctId, projectId);

            var type = "";
            if (file != null)
            {
                type = file.Type;
            
                Clients.Caller.ReturnFile(file.ID, file.Content, file.Type);
            }
        }

        /// <summary>
        /// Puts user in a group for a specific project
        /// </summary>
        /// <param name="id">Project ID</param>
        public void JoinProject(int id)
        {
            Groups.Add(Context.ConnectionId, Convert.ToString(id));
        }

        /// <summary>
        /// Removes a user from a specific project
        /// </summary>
        /// <param name="id">Project ID</param>
        public void LeaveProject(int id)
        {
            Groups.Remove(Context.ConnectionId, Convert.ToString(id));
        }

        /// <summary>
        /// Deletes a file from database and sends notifications to all users in project group
        /// </summary>
        /// <param name="id">Project ID</param>
        /// <param name="fileID">File ID</param>
        public void DeleteFile(int id, int fileID)
        {
            repo.DeleteById(fileID);
            Clients.Group(Convert.ToString(id)).RemoveFile(fileID);
        }

        /// <summary>
        /// Renames a file in database and sends notifications to all users in project group
        /// </summary>
        /// <param name="id">Project ID</param>
        /// <param name="fileID">File ID</param>
        /// <param name="newName">Name</param>
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

        /// <summary>
        /// Deletes a folder in database and sends notifications to all users in project group
        /// </summary>
        /// <param name="projectID">Project ID</param>
        /// <param name="folderID">Folder ID</param>
        public void DeleteFolder(int projectID, int folderID)
        {
            folderRepo.DeleteById(folderID);
            Clients.Group(Convert.ToString(projectID)).DeleteFolder(folderID);
        }

        /// <summary>
        /// Renames a folder in database and sends notifications to all users in project group
        /// </summary>
        /// <param name="projectID">Project ID</param>
        /// <param name="folderID">Folder ID</param>
        /// <param name="newName">Name</param>
        public void RenameFolder(int projectID, int folderID, string newName)
        {
            folderRepo.Update(new Folder { Name = newName, ID = folderID});
            Clients.Group(Convert.ToString(projectID)).UpdateFolder(folderID, newName);
        }
    }
}