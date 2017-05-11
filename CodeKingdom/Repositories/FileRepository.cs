﻿using CodeKingdom.Models;
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

        public File GetById(int id)
        {
            return db.Files.Where(x => x.ID == id).FirstOrDefault();
        }

        public File GetByFileInProject(int fileId, int projectId)
        {
            return db.Files.Where(x => x.ID == fileId && x.ProjectID == projectId).FirstOrDefault();
        }

        public List<File> GetByFolderId(int id)
        {
            return db.Files.Where(x => x.FolderID == id).ToList();
        }

        public File Create(FileViewModel model)
        {
            List<File> files = GetByFolderId(model.FolderID);

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

        public bool deleteByProjectId(int projectID)
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

        public File Rename(FileViewModel model)
        {
            File file = GetById(model.ID);

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