using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using CodeKingdom.Repositories;
using CodeKingdom.Models.ViewModels;
using System.Threading.Tasks;

namespace CodeKingdom.API
{
    public class EditorHub : Hub
    {
        private FileRepository fileRepository = new FileRepository();

        public void JoinFile(int fileID)
        {
            Groups.Add(Context.ConnectionId, Convert.ToString(fileID));
        }

        public void OnChange(object data, int fileID)
        {
            Clients.Group(Convert.ToString(fileID), Context.ConnectionId).OnChange(data);
        }

        public void UpdateCursor(object data, int fileID)
        {
            Clients.Group(Convert.ToString(fileID), Context.ConnectionId).UpdateCursor(data);
        }

        public void Save(string content, int fileID)
        {
            FileViewModel viewModel = new FileViewModel
            {
                ID = fileID,
                Content = content
            };

            fileRepository.UpdateContent(viewModel);
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            // Since every connectionId is unique this should be a save way to remove user cursor
            Clients.All.RemoveCursor(Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }


        public void LeaveFile(int fileID)
        {
            Clients.Group(Convert.ToString(fileID), Context.ConnectionId).RemoveCursor(Context.ConnectionId);
            Groups.Remove(Context.ConnectionId, Convert.ToString(fileID));
        }
    }
}