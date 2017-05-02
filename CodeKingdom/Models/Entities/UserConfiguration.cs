using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeKingdom.Models.Entities
{
    public class UserConfiguration
    {
        public int ID { get; set; }
        public string KeyBinding { get; set; }
        public string ColorScheme { get; set; }

        virtual public ApplicationUser User { get; set; }
    }
}