using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeKingdom.Models.ViewModels
{
    public class ChatViewModel
    {

        public string Message { get; set; }
        public int ProjectID { get; set; }
        public string Username { get; set; }
        public DateTime DateTime { get; set; }
    }
}