using CodeKingdom.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CodeKingdom.Models.ViewModels
{
    public class ProjectViewModel
    {
        public int ID { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Project name cannot be longer than 50 characters.")]
        public string Name { get; set; }
        public string Role { get; set; }
        public string ApplicationUserID { get; set; }

        public List<CollaboratorViewModel> Collaborators { get; set; }
    }
}