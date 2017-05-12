using CodeKingdom.Models;
using CodeKingdom.Models.Entities;
using CodeKingdom.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeKingdom.Repositories
{
    public class FileRepository
    {
        private readonly IAppDataContext db;

        public FileRepository(IAppDataContext context = null)
        {
            db = context ?? new ApplicationDbContext();
        }

        /// <summary>
        /// Returns null or single instance of a file
        /// </summary>
        /// <param name="id">File ID</param>
        public File GetById(int id)
        {
            return db.Files.Where(x => x.ID == id).FirstOrDefault();
        }

        /// <summary>
        /// Returns null or single instance of a file by id and projectID. Returns null if file doesn't exist or if project and file ID don't match.
        /// </summary>
        /// <param name="fileId">File ID</param>
        /// <param name="projectId">Project ID</param>
        public File GetByFileInProject(int fileId, int projectId)
        {
            return db.Files.Where(x => x.ID == fileId && x.ProjectID == projectId).FirstOrDefault();
        }

        /// <summary>
        /// Returns list of files by specific folder id. Returns empty list if no file exists with given folder id.
        /// </summary>
        /// <param name="id">Folder ID</param>
        public List<File> GetByFolderId(int id)
        {
            return db.Files.Where(x => x.FolderID == id).ToList();
        }

        /// <summary>
        /// Stores a single empty file in database and ensures unique name within folder directory and returns created file.
        /// </summary>
        /// <param name="model">Name, Type, User ID, Project ID</param>
        public File Create(FileViewModel model)
        {
            List<File> files = GetByFolderId(model.FolderID);

            // Check if filename is unique, else add something to it and retry creating it
            foreach (File f in files)
            {
                if (f.Name == model.Name)
                {
                    model.Name += "Copy";
                    return Create(model);
                }
            }

            File file = new File
            {
                Name = model.Name,
                Type = model.Type,
                FolderID = model.FolderID,
                ApplicationUserID = model.ApplicationUserID,
                Content = "",
                ProjectID = model.ProjectID,
            };

            db.Files.Add(file);
            db.SaveChanges();
            return file;
        }

        /// <summary>
        /// Deletes a single file from database by id
        /// </summary>
        /// <param name="id">File ID</param>
        public bool DeleteById(int id)
        {
            File file = GetById(id);

            if (file == null)
            {
                return false;
            }

            db.Files.Remove(file);
            db.SaveChanges();

            return true;
        }

        /// <summary>
        /// Deletes every file from database with a given projectID, returns false if no files are deleted, true otherwise.
        /// </summary>
        /// <param name="projectID">Project ID</param>
        public bool DeleteByProjectId(int projectID)
        {
            List<File> files = db.Files.Where(x => x.ProjectID == projectID).ToList();
            if (files.Count == 0)
            {
                return false;
            }
            foreach (var file in files)
            {
                db.Files.Remove(file);
            }
            db.SaveChanges();
            return true;
        }

        /// <summary>
        /// Updates content of a given file. Returns null if file isn't found, else returns updated file
        /// </summary>
        /// <param name="model">File ID, Project ID</param>
        /// <returns></returns>
        public File UpdateContent(FileViewModel model)
        {
            
            File file = GetByFileInProject(model.ID, model.ProjectID);

            if (file == null)
            {
                return null;
            }

            file.Content = model.Content;

            db.SaveChanges();
            
            return file;
        }


        /// <summary>
        /// Renames a file and ensures a unique name if desired file name exists in file's folder directory. Returns null if file isn't found, else returns renamed file.
        /// </summary>
        /// <param name="model">File ID, Project ID, Name(optional), Type(optional)</param>
        public File Rename(FileViewModel model)
        {
            File file = GetByFileInProject(model.ID, model.ProjectID);

            if (file == null)
            {
                return null;
            }

            if (file.Name != model.Name)
            {
                List<File> files = GetByFolderId(model.FolderID);
                foreach (File f in files)
                {
                    if (f.Name == model.Name)
                    {
                        model.Name += "Copy";
                        return Rename(model);
                    }
                }
                file.Name = model.Name;
                db.SaveChanges();
            }

            if (model.Type != null)
            {
                file.Type = model.Type;
                db.SaveChanges();
            }

            return file;
        }
        
    }
}