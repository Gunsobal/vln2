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
        public int FolderID { get; set; }
        [Required]
        public string Name { get; set; }
        public int ProjectID { get; set; }
        public string ApplicationUserID { get; set; }
        public IEnumerable<SelectListItem> Folders { get; set; }

    }
}