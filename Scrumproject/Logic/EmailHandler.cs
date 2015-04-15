using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Scrum.Data;
using Scrumproject.Data;

namespace Scrumproject.Logic
{
    internal class EmailHandler
    {



      

        public string GetBossEmailForAUser(string username)
        {
          
            var userRepository = new UserRepository();

                var getAllUsers = userRepository.GetAllUsers();
                var getAllBosses = userRepository.GetAllBosses();

            var bossEmail = getAllUsers.Join(getAllBosses, u => u.BID, b => b.BID,
                (u, b) => new {User = u, Boss = b})

                .Where(uAndb => uAndb.User.Username == username)
                .Where(uAndb => uAndb.User.BID == uAndb.Boss.BID)
                .Select(x => x.Boss.Email);
                

                return bossEmail.ToString();
            }
        public void SendEmailToBoss(string username)
        {
            string reciever = GetBossEmailForAUser(username);
            MailMessage mail = new MailMessage();
            SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");

            mail.From = new MailAddress("coderdanger@gmail.com");
            mail.To.Add(reciever);
            mail.Subject = "New changes in your travel system";
            mail.Body = "A change has been made, please go fuck urself.";

            smtpServer.Port = 587;
            // coderdanger = The email-bot
            smtpServer.Credentials = new NetworkCredential("coderdanger@gmail.com", "mamma758");
            smtpServer.EnableSsl = true;

            smtpServer.Send(mail);
        }
    }
}
            
        

    

