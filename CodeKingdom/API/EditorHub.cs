using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using CodeKingdom.Repositories;
using CodeKingdom.Models.ViewModels;
using System.Threading.Tasks;
using CodeKingdom.Models;

namespace CodeKingdom.API
{
    /// <summary>
    /// Signal R hub that is responsible for receiving and sending out changes that happens in the 
    /// online editor. 
    /// </summary>
    public class EditorHub : Hub
    {
        private FileRepository fileRepository = new FileRepository();
        private static List<EditorUser> users = new List<EditorUser>();

        /// <summary>
        /// When user joins files he will receive changes and notifications about that file by fileID.
        /// </summary>
        /// <param name="fileID"></param>
        public void JoinFile(int fileID)
        {
            string id = Context.ConnectionId;
            string username = Context.User.Identity.Name;

            EditorUser user = users.Where(u => u.Username == username).FirstOrDefault();
            if (user == null)
            {
                user = new EditorUser(id, username);
                users.Add(user);
            }

            user.Groups.Add(Convert.ToString(fileID));
            Groups.Add(Context.ConnectionId, Convert.ToString(fileID));
            Clients.Group(Convert.ToString(fileID)).UserList(users);
        }

        /// <summary>
        /// Sombody changed a file. Let's brodcast that to the rest of the group.
        /// </summary>
        /// <param name="data">Content of the file being changed</param>
        /// <param name="fileID">ID of file being changed</param>
        public void OnChange(object data, int fileID)
        {
            Clients.Group(Convert.ToString(fileID), Context.ConnectionId).OnChange(data);
        }

        /// <summary>
        /// When user moves his position in the file everyone in that file should know about it. 
        /// </summary>
        /// <param name="data">Content of the file being changed</param>
        /// <param name="fileID">ID of file being changed</param>
        public void UpdateCursor(object data, int fileID)
        {
            Clients.Group(Convert.ToString(fileID), Context.ConnectionId).UpdateCursor(data);
        }

        /// <summary>
        /// Sombody want's a updated list of the connected users. Let's give him that.
        /// </summary>
        /// <param name="fileID">ID of open file</param>
        public void GetUsers(int fileID)
        {
            List<EditorUser> groupUsers = users.Where(u => u.Groups.Contains(Convert.ToString(fileID))).ToList();
            Clients.Group(Convert.ToString(fileID)).userList(groupUsers);
        }

        /// <summary>
        /// File has changed and needs to be saved
        /// </summary>
        /// <param name="content">Content of file being changed</param>
        /// <param name="fileID">ID of file being changed</param>
        /// <param name="projectID">ID of open project</param>
        public void Save(string content, int fileID, int projectID)
        {
            FileViewModel viewModel = new FileViewModel
            {
                ID = fileID,
                ProjectID = projectID,
                Content = content
            };

            fileRepository.UpdateContent(viewModel);
        }

        /// <summary>
        /// Sombody disconnected. He needs to be removed and rest of the group should know about it so 
        /// they can remove his cursor and remove his username from the list of connected users.
        /// </summary>
        /// <param name="stopCalled"></param>
        /// <returns></returns>
        public override Task OnDisconnected(bool stopCalled)
        {
            string id = Context.ConnectionId;

            EditorUser user = users.Where(u => u.ID == id).FirstOrDefault();

            if (user != null)
            {
                if (users.Remove(user))
                {
                    foreach (var group in user.Groups)
                    {
                        Clients.Group(group).RemoveCursor(Context.ConnectionId);
                        Clients.Group(group).userList(users);
                    }
                }

                EditorUser.ClearColor(user.Color);
            }

            return base.OnDisconnected(stopCalled);
        }

        /// <summary>
        /// User are leaving some file with id = fileID. He needs to be unsubscribed from changes in that file.
        /// </summary>
        /// <param name="fileID">File ID</param>
        public void LeaveFile(int fileID)
        {
            string id = Context.ConnectionId;
            string groupName = Convert.ToString(fileID);
            EditorUser user = users.Where(u => u.ID == id).FirstOrDefault();
            user.Groups.Remove(groupName);
            Clients.Group(groupName).RemoveCursor(Context.ConnectionId);
            GetUsers(fileID);
        }        
    }
} 