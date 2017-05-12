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

        /// <summary>
        /// Returns null or single instance of folder
        /// </summary>
        /// <param name="id">Folder ID</param>
        public Folder GetById(int id)
        {
            return db.Folders.Where(x => x.ID == id).FirstOrDefault();
        }

        /// <summary>
        /// Returns list of folders within a folder. Returns empty list if no subfolders are found
        /// </summary>
        /// <param name="id">Folder ID</param>
        public List<Folder> GetChildrenById(int id)
        {
            return db.Folders.Where(x => x.FolderID == id).ToList();
        }

        /// <summary>
        /// Returns a list of a folder with a given id and every subfolder within that folder and every subsequent subfolder within those subfolders until no subfolders are found. Returns empty list if folder with given id isn't found. 
        /// </summary>
        /// <param name="id">Folder ID</param>
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

        /// <summary>
        /// Returns null or single instance of folder by folder parent id.
        /// </summary>
        /// <param name="id">Folder ID</param>
        public Folder GetParent(int id)
        {
            Folder folder = GetById(id);
            if (folder == null || !folder.FolderID.HasValue)
            {
                return null;
            }
            return GetById(folder.FolderID.Value);
        }

        /// <summary>
        /// Returns null or a single instance of a folder with no parent folder (or a root folder)
        /// </summary>
        /// <param name="id">Folder ID</param>
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

        /// <summary>
        /// Stores a single instance of a folder in database and ensures a unique name within folder directory, returns created folder
        /// </summary>
        /// <param name="folder">Folder ID, Name</param>
        public Folder Create(Folder folder)
        {
            List<Folder> foldersInParent = GetChildrenById(folder.FolderID.Value);

            // Check for duplicate names
            foreach (Folder f in foldersInParent)
            {
                if (f.Name == folder.Name)
                {
                    folder.Name += "Copy";
                    return Create(folder);
                }
            }
            db.Folders.Add(folder);
            db.SaveChanges();
            return folder;
        }

        /// <summary>
        /// Deletes a folder from database and every subfolder/file within that folder and every subfolder/file within those subfolders until no subfolders are found.
        /// </summary>
        /// <param name="id">Folder ID</param>
        public bool DeleteById(int id)
        {
            List<Folder> children = GetCascadingChildrenById(id);
            if (children.Count == 0)
            {
                return false;
            }

            // Remove all subfolders and files
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

        /// <summary>
        /// Updates folder name, returns false if no folder is found, true otherwise
        /// </summary>
        /// <param name="folder">Folder ID, Name</param>
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

        /// <summary>
        /// Recursively build a list of all subfolders from a given starting id
        /// </summary>
        /// <param name="id">Folder ID</param>
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