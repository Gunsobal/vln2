using CodeKingdom.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeKingdom.Models.ViewModels
{
    public class EditorViewModel
    {
        public string Name { get; set; }
        public int ProjectID { get; set; }
        public int FileID { get; set; }
        public string Type { get; set; }
        public string Content { get; set; }
        public List<Collaborator> Collaborators { get; set; }
        public List<Folder> Folders { get; set; }
        public List<File> Files { get; set; }
        public List<ChatViewModel> Chats { get; set; }
        public Folder Root { get; set; }
       
    }   



}