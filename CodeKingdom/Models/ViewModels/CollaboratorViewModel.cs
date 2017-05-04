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
        public string UserName { get; set; }
        
        public string RoleName { get; set; }

        [Required]
        public int RoleID { get; set; }

        public int ProjectID { get; set; }

        public IEnumerable<SelectListItem> Roles { get; set; }
    }
}