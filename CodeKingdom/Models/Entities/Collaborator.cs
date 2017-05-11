using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CodeKingdom.Models.Entities
{
    public class Collaborator
    {


        public int ID { get; set; }
        [Required]
        public string ApplicationUserID { get; set; }
        [Required]
        public int ProjectID { get; set; }
        [Required]
        public int CollaboratorRoleID { get; set; }

        virtual public ApplicationUser User { get; set; }
        virtual public Project Project{ get; set; }
        virtual public CollaboratorRole Role { get; set; }
    }
}