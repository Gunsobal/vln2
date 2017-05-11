using CodeKingdom.Models;
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

        public Folder GetParent(int id)
        {
            Folder folder = GetById(id);
            if (folder == null || !folder.FolderID.HasValue)
            {
                return null;
            }
            return GetById(folder.FolderID.Value);
        }

        public Folder GetRoot(int id)
        {
            Folder parent = GetParent(id);
            if (parent == null)
            {
                return GetById(id);
            }
            else
            {
                return GetRoot(parent.ID);
            }
        }

        public Folder Create(Folder folder)
        {
            List<Folder> foldersInParent = GetChildrenById(folder.FolderID.Value);
            foreach (Folder f in foldersInParent)
            {
                if (f.Name == folder.Name)
                {
                    folder.Name += "Copy";
                    Create(folder);
                }
            }
            db.Folders.Add(folder);
            db.SaveChanges();
            return folder;
        }

        public bool DeleteById(int id)
        {
            List<Folder> children = GetCascadingChildrenById(id);
            if (children.Count == 0)
            {
                return false;
            }
            foreach (var child in children)
            {
                List<File> files = db.Files.Where(x => x.FolderID == child.ID).ToList();
                foreach (var file in files)
                {
                    db.Files.Remove(file);
                }
                db.Folders.Remove(child);
            }
            db.SaveChanges();
            return true;
        }

        public bool Update(Folder folder)
        {
            Folder existing = GetById(folder.ID);
            if (existing == null || existing.Name == folder.Name)
            {
                return false;
            }
            List<Folder> folders = GetChildrenById(existing.ID);
            foreach (var f in folders)
            {
                if (f.Name == folder.Name)
                {
                    folder.Name += "Copy";
                    return Update(folder);
                }
            }
            existing.Name = folder.Name;
            db.SaveChanges();
            return true;
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