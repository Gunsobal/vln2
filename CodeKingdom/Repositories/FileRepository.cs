using CodeKingdom.Models;
using CodeKingdom.Models.Entities;
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
            return db.Files.Find(id);
        }

        public List<File> GetByFolderId(int id)
        {
            return db.Files.Where(x => x.Folder.ID == id).ToList();
        }

        public bool DeleteById(int id)
        {
            File file = db.Files.Find(id);

            if (file == null)
            {
                return false;
            }

            db.Files.Remove(file);
            db.SaveChanges();

            return true;
        }
    }
}