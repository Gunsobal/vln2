using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeKingdom.Models.Entities
{
    public class Folder
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int? FolderID { get; set; }

        virtual public Folder Parent { get; set; }
        virtual public List<Folder> Folders { get; set; }
        virtual public List<File> Files{ get; set; }
    }
}