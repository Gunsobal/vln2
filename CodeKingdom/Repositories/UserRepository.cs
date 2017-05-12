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

        /// <summary>
        /// Returns null or single user by user ID
        /// </summary>
        /// <param name="id">User ID</param>
        public ApplicationUser GetById(string id)
        {
            return db.Users.Where(u => u.Id == id).FirstOrDefault();
        }

        /// <summary>
        /// Returns null or single user by user email
        /// </summary>
        /// <param name="email">Email</param>
        public ApplicationUser GetByEmail(string email)
        {
            return db.Users.Where(u => u.Email == email).FirstOrDefault();
        }

        /// <summary>
        /// Returns null or single user by user name
        /// </summary>
        /// <param name="userName">Username</param>
        public ApplicationUser GetByUserName(string userName)
        {
            return db.Users.Where(u => u.UserName == userName).FirstOrDefault();
        }

        /// <summary>
        /// Returns current theme for editor by user ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Returns current key binding for editor by user ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Sets configuration for user by IndexViewModel. 
        /// If the user does not have specified configuration, default configurations are set. 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Sets default configurations for users ny user email. 
        /// Returns new UserConfigurations object.
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
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