using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Scrumproject.Logic
{
    internal class EmailHandler
    {



        public static void SendEmail(string reciever)
        {

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
            
        

    

