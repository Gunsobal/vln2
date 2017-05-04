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
                Name = "jquery",
                FolderID = 4
            });
        }

        static public void Files(MockDataContext context)
        {

        }

    }
}
