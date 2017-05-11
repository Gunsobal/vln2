using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CodeKingdom.Models.Entities
{
    public class Project
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Project name cannot be longer than 50 characters.")]
        public string  Name { get; set; }
        public bool Frozen { get; set; }

        [Required]
        public int FolderID { get; set; }
        
        virtual public Folder Root { get; set; }
        virtual public List<Collaborator> Collaborators { get; set; }
    }
}