using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using CodeKingdom.Business;

namespace CodeKingdom.API
{
    public class FileHub : Hub
    {
        private ProjectStructure business = new ProjectStructure();
        public void Get(string id)
        {
            var correctId = 0;
            int.TryParse(id, out correctId);
            var file = business.GetFileByID(correctId);
            // TODO: Change file types in database so that they match with ace editor
            Clients.Caller.ReturnFile(file.Content, file.Type);
        }
    }
}