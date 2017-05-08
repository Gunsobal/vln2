using CodeKingdom.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeKingdom.Models.ViewModels
{
    public class ProjectViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string ApplicationUserID { get; set; }

        public List<CollaboratorViewModel> Collaborators { get; set; }
    }
}