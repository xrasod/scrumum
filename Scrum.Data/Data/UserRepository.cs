﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

    }
}
