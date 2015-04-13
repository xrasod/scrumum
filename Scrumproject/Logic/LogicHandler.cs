using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Scrum.Data;
using Scrum.Data.Data;
using Scrumproject.Logic.Entities;
using iTextSharp.text.pdf;
using Scrumproject.Data;

namespace Scrumproject.Logic
{
   public class LogicHandler
    { 
      
      ReportRepository<Report> reportRepository = new ReportRepository<Report>();
      ReportRepository<Notes> notesRepository = new ReportRepository<Notes>();
      BossRepository bossRepository = new BossRepository();
      


      public Report LoadDraft(string sokvag)
      {
          return reportRepository.Ladda(sokvag);
      }
       
       public void SaveDraft(Report report, string sokvag)
       {
           reportRepository.Spara(report, sokvag);
       }

       public Notes LoadNotes(string sokvag)
       {
           return notesRepository.Ladda(sokvag);
       }

       public void SaveNotes(Notes report, string sokvag)
       {
           notesRepository.Spara(report, sokvag);
       }

       public void registeruser(string Firstname, string Lastname, string email, string password, int bossid,
            string personnr)
       {
           var users = BossRepository.GetAll();

           foreach (var user in users)
           {
               if (Firstname.Substring(0, 3) + Lastname.Substring(0, 3) == user.Username)
               {
                   MessageBox.Show("Username already exists.");

               }

           }

           try
           {
               var user = new Scrum.Data.User
               {
                   PW = password,
                   BID = bossid,
                   FirstName = Firstname,
                   LastName = Lastname,
                   SSN = personnr,
                   Username = Firstname.Substring(0, 3) + Lastname.Substring(0, 3),
                   Email = email,
                   Status = true
               };

               bossRepository.adduser(user);

           }
           catch (Exception ex)
           {
               Console.WriteLine(ex + "#### Det har blivit fel, ProfileController, Register");
           }
       }

       public void changeStatus(string username)
       {
           BossRepository.UpdateUser(username);

       }

       public void uppdateUser(string username)
       {
           var users = BossRepository.GetAll();

           foreach (var user in users)
           {
               if (username == user.Username)
               {


               }

           }



       }
   }
}
