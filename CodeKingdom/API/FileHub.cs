using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using CodeKingdom.Business;
using CodeKingdom.Models.Entities;

namespace CodeKingdom.API
{
    public class FileHub : Hub
    {
        private ProjectStructure business = new ProjectStructure();

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
    }
}