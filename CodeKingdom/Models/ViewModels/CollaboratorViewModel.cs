using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodeKingdom.Models.ViewModels
{
    public class CollaboratorViewModel
    {
        
        public int ID { get; set; }
        [Required]
        [Display(Name = "Email")]
        [StringLength(50, ErrorMessage = "Project name cannot be longer than 50 characters.")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }

        [Required]
        [Display(Name = "Role Name")]
        public int RoleID { get; set; }

        public int ProjectID { get; set; }

        public IEnumerable<SelectListItem> Roles { get; set; }
    }
}