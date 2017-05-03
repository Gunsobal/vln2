using CodeKingdom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeKingdomTests
{
    static class TestSeed
    {
        static public void All(MockDataContext context)
        {
            Users(context);
        }

        static public void Users(MockDataContext context)
        {
            context.Users.Add(new ApplicationUser {
                Id = "1",
                UserName = "gunzo@gunzo.is",
                Email = "gunzo@gunzo.is"
            });

            context.Users.Add(new ApplicationUser
            {
                Id = "2",
                UserName = "disa@klukka.is",
                Email = "disa@klukka.is"
            });

            context.Users.Add(new ApplicationUser
            {
                Id = "3",
                UserName = "saesi@hotmale.is",
                Email = "saesi@hotmale.is"
            });

            context.Users.Add(new ApplicationUser
            {
                Id = "4",
                UserName = "stuni@stud.is",
                Email = "stuni@stud.is"
            });
        }


    }
  
  
}
