using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeKingdom.Models.Entities
{
    public class Project
    {
        public int ID { get; set; }
        public string  Name { get; set; }
        
        public bool Frozen { get; set; }
        virtual public Folder Root { get; set; }
        virtual public List<Collaborator> Collaborators { get; set; }
    }
}