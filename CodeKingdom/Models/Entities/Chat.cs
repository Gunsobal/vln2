using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeKingdom.Models.Entities
{
    public class Chat
    {
        public int ID { get; set; }        
        public string Message { get; set; }

        virtual public ApplicationUser User { get; set; }
        virtual public Project Project { get; set; }
        public DateTime DateTime { get; set; }
    }
}