using CodeKingdom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeKingdom.Models.Entities;

namespace CodeKingdomTests
{
    static class TestSeed
    {
        static public void All(MockDataContext context)
        {

        }
        
        static public void Folders(MockDataContext context)
        {
            /// Root Folders
            context.Folders.Add(new Folder
            {
                ID = 1,
                Name = "root1"
            });
            context.Folders.Add(new Folder
            {
                ID = 2,
                Name = "root2"
            });
            /// Folders
            context.Folders.Add(new Folder
            {
                ID = 3,
                Name = "css",
                FolderID = 1
            });
            context.Folders.Add(new Folder
            {
                ID = 4,
                Name = "scripts",
                FolderID = 1
            });
            context.Folders.Add(new Folder
            {
                ID = 5,
                Name = "javascript",
                FolderID = 4
            });
        }

        static public void Files(MockDataContext context)
        {
            context.Files.Add(new File
            {
                ID = 1,
                Name = "index",
                FolderID = 1,
                Type = "html",
                Content = "stuff",
                ApplicationUserID = "dummy",
            });
            context.Files.Add(new File
            {
                ID = 2,
                Name = "style",
                FolderID = 3,
                Type = "css",
                Content = "css-stuff",
                ApplicationUserID = "dummy",
            });
            context.Files.Add(new File
            {
                ID = 3,
                Name = "myscript",
                FolderID = 5,
                Type = "js",
                Content = "script-stuff",
                ApplicationUserID = "dummy",
            });
            context.Files.Add(new File
            {
                ID = 4,
                Name = "birds",
                FolderID = 2,
                Type = "jpg",
                Content = "pic-of-birds",
                ApplicationUserID = "dummy",
            });
            context.Files.Add(new File
            {
                ID = 5,
                Name = "better-birds",
                FolderID = 2,
                Type = "png",
                Content = "pic-of-better-birds",
                ApplicationUserID = "dummy",
            });
            context.Files.Add(new File
            {
                ID = 6,
                Name = "logo",
                FolderID = 2,
                Type = "ai",
                Content = "illustrator-file",
                ApplicationUserID = "dummy",
            });
        }

        static public void Projects(MockDataContext context)
        {
            context.Projects.Add(new Project
            {
                ID = 1,
                Name = "SpaceX",
                FolderID = 1,
                Frozen = false
            });

            context.Projects.Add(new Project
            {
                ID = 2,
                Name = "The new Enron",
                FolderID = 2,
                Frozen = false
            });

        }

    }
}
