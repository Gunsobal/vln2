using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeKingdom.Models.Entities
{
    public class Collaborator
    {


        public int ID { get; set; }
        public string ApplicationUserID { get; set; }
        public int ProjectID { get; set; }
        public int CollaboratorRoleID { get; set; }

        virtual public ApplicationUser User { get; set; }
        virtual public Project Project{ get; set; }
        virtual public CollaboratorRole Role { get; set; }
    }
}