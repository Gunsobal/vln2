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


    }
}