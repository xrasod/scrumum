using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scrum.Data;

namespace Scrumproject.Data
{
    public class UserRepository
    {
        public User LoginUser(string username, string password)
        {
            using (var context = new scrumEntities())
            {
                return context.Users.FirstOrDefault(x =>
                    x.Username.Equals(username) &&
                    x.PW.Equals(password));
            }
        }

        public Boss LoginBoss(string username, string password)
        {
            using (var context = new scrumEntities())
            {
                return context.Bosses.FirstOrDefault(x =>
                    x.Username.Equals(username) &&
                    x.PW.Equals(password));
            }
        }

        public List<User> GetAllUsers()
        {
            
            using (var context = new scrumEntities())
            {
                return context.Users.ToList();
            }
        }

        public List<Boss> GetAllBosses()
        {
            using (var context = new scrumEntities())
            {
                return context.Bosses.ToList();
            }
        }

    }
}
