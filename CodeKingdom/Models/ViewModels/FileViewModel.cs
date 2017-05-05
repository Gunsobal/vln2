using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeKingdom.Models.ViewModels
{
    public class FileViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int FolderID { get; set; }
        public string Content { get; set; }
        public string Type { get; set; }
        public string ApplicationUserID { get; set; }
        public int MyProperty { get; set; }
    }
}