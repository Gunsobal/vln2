﻿using CodeKingdom.Models;
using CodeKingdom.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeKingdom.Repositories
{
    public class FolderRepository
    {
        private readonly IAppDataContext db;

        public FolderRepository(IAppDataContext context = null)
        {
            db = context ?? new ApplicationDbContext();
        }

        public Folder GetById(int id)
        {
            return db.Folders.Where(x => x.ID == id).FirstOrDefault();
        }

        public List<Folder> GetChildrenById(int id)
        {
            return db.Folders.Where(x => x.FolderID == id).ToList();
        }

        public List<Folder> GetCascadingChildrenById(int id)
        {
            List<Folder> folders = getCascadingChildrenRecursive(id);
            Folder folder = GetById(id);
            if (folder != null)
            {
                folders.Insert(0, folder);
            }
            return folders;
        }

        public Folder Create(Folder folder)
        {
            return new Folder();
        }

        public bool DeleteById(int id)
        {
            return false;
        }

        public Folder Update(Folder folder)
        {
            return new Folder();
        }

        private List<Folder> getCascadingChildrenRecursive(int id)
        {
            List<Folder> children = GetChildrenById(id);
            List<Folder> folders = new List<Folder>();
            folders.AddRange(children);
            foreach (Folder child in children)
            {
                folders.AddRange(getCascadingChildrenRecursive(child.ID));
            }
            return folders;
        }
    }
}