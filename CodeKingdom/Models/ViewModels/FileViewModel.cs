using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodeKingdom.Models.ViewModels
{
    public class FileViewModel
    {
        public int ID { get; set; }
        public int ProjectID { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "File name cannot be longer than 50 characters.")]
        public string Name { get; set; }
        public int FolderID { get; set; }
        public string Content { get; set; }
        [Required]
        [StringLength(20, ErrorMessage = "file type cannot be longer than 50 characters.")]
        public string Type { get; set; }
        public string ApplicationUserID { get; set; }

        public IEnumerable<SelectListItem> Folders { get; set; }

    }
}