using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeKingdom.Models.Entities
{
    public class File
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public string Type { get; set; }
        public int FolderID { get; set; }
        public string ApplicationUserID { get; set; }

        virtual public Folder Folder { get; set; }
        virtual public ApplicationUser Owner { get; set; }
    }
}