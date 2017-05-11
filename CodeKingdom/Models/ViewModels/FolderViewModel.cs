using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodeKingdom.Models.ViewModels
{
    public class FolderViewModel
    {
        public int ID { get; set; }
        [Required]
        [Display(Name = "Folder")]
        public int FolderID { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Folder name cannot be longer than 50 characters.")]
        public string Name { get; set; }
        public int ProjectID { get; set; }
        public string ApplicationUserID { get; set; }
        public IEnumerable<SelectListItem> Folders { get; set; }

    }
}