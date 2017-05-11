using CodeKingdom.Models;
using CodeKingdom.Models.Entities;
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

        public string GetColorScheme(string id)
        {
            UserConfiguration userConfig = db.UserConfigurations.Where(u => u.AppicationUserID == id).FirstOrDefault();
            ApplicationUser appUser = db.Users.Where(u => u.Id == id).FirstOrDefault();
            if (userConfig == null)
            {
                userConfig = SetInitialUserConfig(appUser.Email);

                db.UserConfigurations.Add(userConfig);
               
                db.SaveChanges();
            }

            return userConfig.ColorScheme;

        }
        public string GetKeyBinding(string id)
        {
            UserConfiguration userConfig = db.UserConfigurations.Where(u => u.AppicationUserID == id).FirstOrDefault();
            ApplicationUser appUser = db.Users.Where(u => u.Id == id).FirstOrDefault();
            if (userConfig == null)
            {
                userConfig = SetInitialUserConfig(appUser.Email);

                db.UserConfigurations.Add(userConfig);
               
                db.SaveChanges();
            }
            return userConfig.KeyBinding;
        }

        public bool SetUserConfiguration(IndexViewModel model)
        {
            UserConfiguration userConfig = db.UserConfigurations.Where(u => u.User.Email == model.UsersEmailAddress).FirstOrDefault();

            if (userConfig == null)
            {
                userConfig = SetInitialUserConfig(model.UsersEmailAddress);

                db.UserConfigurations.Add(userConfig);
            }
            
            userConfig.KeyBinding = model.Keybinding;
            userConfig.ColorScheme = model.Colorscheme;
            db.SaveChanges();
            
            return true;
        }

        private UserConfiguration SetInitialUserConfig(string emailAddress)
        {
            ApplicationUser appUser = db.Users.Where(u => u.Email == emailAddress).FirstOrDefault();
            string userID = appUser.Id;

            return new UserConfiguration()
            {
                ColorScheme = "monokai",
                KeyBinding = "ace",
                AppicationUserID = userID,
                User = appUser,
            };
        }
    }
}