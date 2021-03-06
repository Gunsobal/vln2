﻿using CodeKingdom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeKingdom.Models.Entities;

namespace CodeKingdomTests
{
    /// <summary>
    /// Test Seed class to seed the Mock Database with data
    /// </summary>
    static class TestSeed
    {
        static public void All(MockDataContext context)
        {
            Folders(context);
            Projects(context);
            Files(context);
            CollaboratorRoles(context);
            Collaborators(context);
            Chats(context);
        }

        #region Collaborator seed
        static public void CollaboratorRoles(MockDataContext context)
        {
            context.CollaboratorRoles.Add(new CollaboratorRole
            {
                ID = 1,
                Name = "Owner"
            });
            context.CollaboratorRoles.Add(new CollaboratorRole
            {
                ID = 2,
                Name = "Member"
            });
            context.CollaboratorRoles.Add(new CollaboratorRole
            {
                ID = 3,
                Name = "Reader"
            });
        }

        static public void Collaborators(MockDataContext context)
        {
            context.Collaborators.Add(new Collaborator
            {
                ID = 1,
                ProjectID = 1,
                ApplicationUserID = "test1",
                CollaboratorRoleID = 1
            });

            context.Collaborators.Add(new Collaborator
            {
                ID = 4,
                ProjectID = 2,
                ApplicationUserID = "test1",
                CollaboratorRoleID = 1
            });

            context.Collaborators.Add(new Collaborator
            {
                ID = 5,
                ProjectID = 3,
                ApplicationUserID = "test1",
                CollaboratorRoleID = 1
            });

            context.Collaborators.Add(new Collaborator
            {
                ID = 2,
                ProjectID = 1,
                ApplicationUserID = "test2",
                CollaboratorRoleID = 2
            });

            context.Collaborators.Add(new Collaborator
            {
                ID = 3,
                ProjectID = 1,
                ApplicationUserID = "test3",
                CollaboratorRoleID = 3
            });
        }

        #endregion

        #region Folder seed
        static public void Folders(MockDataContext context)
        {
            // Root Folders
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
            context.Folders.Add(new Folder
            {
                ID = 6,
                Name = "root3"
            });
            // Normal Folders
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
        #endregion

        #region File seed
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
                ProjectID = 1
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
                ProjectID = 1
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
            context.Files.Add(new File
            {
                ID = 7,
                Name = "index",
                FolderID = 6,
                Type = "js",
                Content = "Default File",
                ApplicationUserID = "test1"
            });
        }
        #endregion

        #region Project seed
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
            context.Projects.Add(new Project
            {
                ID = 3,
                Name = "Thest",
                FolderID = 6,
                Frozen = true
            });
        }
        #endregion

        #region Chat seed
        static public void Chats(MockDataContext context)
        {
            context.Chats.Add(new Chat
            {
                ID = 1,
                ProjectID = 1,
                ApplicationUserID = "test1",
                Message = "Hi",
                DateTime = DateTime.Now
            });

            context.Chats.Add(new Chat
            {
                ID = 1,
                ProjectID = 1,
                ApplicationUserID = "test2",
                Message = "Test Message",
                DateTime = DateTime.Now
            });
        }
        #endregion
    }
}
