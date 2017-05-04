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

        //Create

        //Delete

        //Update
    }
}