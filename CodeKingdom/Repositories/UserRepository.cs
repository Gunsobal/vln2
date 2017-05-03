﻿using CodeKingdom.Models;
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
        private readonly IAppDataContext db;
        private UserManager<ApplicationUser> manager;

        public UserRepository(IAppDataContext context = null)
        {
            if (context == null)
            {
                db = new ApplicationDbContext();
                var store = new UserStore<ApplicationUser>(db as ApplicationDbContext);
                manager = new UserManager<ApplicationUser>(store);
            }
            else
            {
                db = context;
            }
        }

        public ApplicationUser GetById(string id)
        {
            var user = manager.Users.Where(u => u.Id == id).FirstOrDefault();
            return user;
        }
    }
}