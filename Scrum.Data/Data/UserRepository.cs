using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrum.Data.Data
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

        public string GetBossForUser(string username)
        {
            using (var context = new scrumEntities())
            {
                var loggedInUser = context.Users.FirstOrDefault(x => x.Username == username);
                var chef = context.Bosses.FirstOrDefault(x => x.BID == loggedInUser.BID);
                return chef.FirstName + " " + chef.LastName;
            }
        }

        public string GetFullNameFromUsername(string username)
        {
            using (var context = new scrumEntities())
            {
                var loggedInUser = context.Users.FirstOrDefault(x => x.Username == username);
                return loggedInUser.FirstName + " " + loggedInUser.LastName;
            }
        }

        public int GetUserId(string username)
        {
            using (var context = new scrumEntities())
            {
                var user = context.Users.SingleOrDefault(x => x.Username == username);
                return user.UID;
            }
        }

        public User GetUserID(string user)
        {
            using (var context = new scrumEntities())
            {
                return context.Users.FirstOrDefault(x => x.Username.Equals(user));
            }
        }


        public static void SaveTravelInfo(TravelInfo ti)
        {
            using (var context = new scrumEntities())
            {
                context.TravelInfoes.Add(ti);
            }
        }

        public static User GetUserID(string user)
        {
            using (var context = new scrumEntities())
            {
                return context.Users.FirstOrDefault(x => x.Username.Equals(user));
            }
        }

        public int SaveReport(Report r)
        {
            using (var context = new scrumEntities())
            {
                context.Reports.Add(r);
                context.SaveChanges();

                int id = r.RID;
                return id;
            }
        }

        public static void SaveReciept(Reciept ri)
        {
            using (var context = new scrumEntities())
            {
                context.Reciepts.Add(ri);
                context.SaveChanges();
            }
        }

        public static Country GetCountryID(string name)
        {
            using (var context = new scrumEntities())
            {
                return context.Countries.FirstOrDefault(x => x.Name.Equals(name));
            }
        }
    }
}
