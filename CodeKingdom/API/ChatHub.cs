using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using CodeKingdom.Repositories;
using CodeKingdom.Models.ViewModels;

namespace CodeKingdom.API
{
    public class ChatHub : Hub
    {
        ChatRepository chatRepository = new ChatRepository();

        public void JoinChat(int projectID)
        {
            Groups.Add(Context.ConnectionId, Convert.ToString(projectID));
        }

        public void Send(int projectID, string message)
        {
            string username = Context.User.Identity.Name;

            ChatViewModel viewModel = new ChatViewModel
            {
                Message = message,
                Username = username,
                DateTime = DateTime.Now,
                ProjectID = projectID
            };
            viewModel.DateAndTime = viewModel.DateTime.ToString("dd MMM HH:mm");

            Clients.Group(Convert.ToString(projectID)).addNewMessageToPage(viewModel);
            chatRepository.Save(viewModel);
        }
    }
}