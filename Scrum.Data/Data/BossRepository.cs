using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scrum.Data;

namespace Scrumproject.Data
{
    public class BossRepository
    {
        public void authorizeReport()
        {
            //Metod som kallas på om chef godkänner rapport
        }

        public void denyReport()
        {
            //Metod som kallas på om chef nekar rapport
        }
        //Lägg till användare
        public void adduser(User user)
        {

            using (var context = new scrumEntities())
            {
                try
                {
                    context.Users.Add(user);

                    context.SaveChanges();
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName,
                                validationError.ErrorMessage);
                        }
                    }
                }

            }
        }
        //Skapa list med användarnamn
        public static List<User> GetMatchingUsers(string name)
        {
            using (var context = new scrumEntities())
            {
                return context.Users.Where(x => x.Username.Contains(name)).ToList();
            }

        } 
        public static List<User> GetAll()
        {


            using (var context = new scrumEntities())
            {
                return context.Users.OrderBy(x => x.FirstName).ToList();
            }
        }


        //Uppdatera status på användare
        public static void UpdateStatus(string username)
        {
            using (var context = new scrumEntities())
            {

                try
                {
                    var u = context.Users
                        .FirstOrDefault(x => x.Username == username);
                    u.Status = false;

                    context.SaveChanges();
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName,
                                validationError.ErrorMessage);
                        }
                    }
                }
            }
        }

        //Uppdatera användare
        public static void UpdateUser(int userid, string newusername, string newfirstname, string newlastname,
            string newpassword, string newssn, string newemail, int newboss)
        {
            using (var context = new scrumEntities())
            {

                try
                {
                    var u = context.Users
                        .FirstOrDefault(x => x.UID == userid);
                    u.PW = newpassword;
                    u.Username = newusername;
                    u.FirstName = newfirstname;
                    u.LastName = newlastname;
                    u.Email = newemail;
                    u.SSN = newssn;
                    u.BID = newboss;


                    context.SaveChanges();
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName,
                                validationError.ErrorMessage);
                        }
                    }
                }
            }





        }
    }
}
