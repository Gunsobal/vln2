using CodeKingdom.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeKingdom.Repositories
{
    public class UserRepository
    {
        private readonly ApplicationDbContext db;

        public UserRepository()
        {
            db = new ApplicationDbContext();
        }

        public ApplicationUser GetById(string id)
        {
            return db.Users.Where(u => u.Id == id).FirstOrDefault();
        }

        public ApplicationUser GetByEmail(string email)
        {
            return db.Users.Where(u => u.Email == email).FirstOrDefault();
        }

        public ApplicationUser GetByUserName(string userName)
        {
            return db.Users.Where(u => u.UserName == userName).FirstOrDefault();
        }
        public ApplicationUser Getcolorscheme(string id)
        {
            return db.UserConfigurations.Where(u => u.AppicationUserID == id) .FirstOrDefault();
            //return UserConfiguration. "ghg";
        }
    }
}