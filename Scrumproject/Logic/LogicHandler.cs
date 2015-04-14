using System;
using System.Collections.Generic;
using System.IO;
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
      PDFRepository pdfRepository = new PDFRepository();

       public void CreatePdf(string text, string filnamn)
       {
           pdfRepository.createPdf(text, filnamn);
       }


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
           var notes = new Notes();
           if (File.Exists(sokvag))
           {
               return notesRepository.Ladda(sokvag);
           }
           notes.Note = "Det finns ingen sparad information";
           return notes;
       }

       public void SaveNotes(Notes report, string sokvag)
       {
           notesRepository.Spara(report, sokvag);
       }
       //Lägger till användare.
       public void registeruser(string Firstname, string Lastname, string email, string password, int bossid,
            string personnr)
       {
           
           var userlist = BossRepository.GetMatchingUsers(Firstname.Substring(0, 3) + Lastname.Substring(0, 3));
           var userName = Firstname.Substring(0, 3) + Lastname.Substring(0, 3);
           if (userlist.Count == 1)
           {
               userName = userName + 1;  

           }
           else if (userlist.Count > 1)
           {
               userName = userName + (userlist.Count);
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
                   Username = userName,
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
       //Ändra status på användare
       public void changeStatus(string username)
       {
           BossRepository.UpdateStatus(username);

       }
       //Uppdatera användare
       public void uppdateUser(int userID, string username, string firstname, string lastname, string password, string ssn, string email, int boss)
       {

           BossRepository.UpdateUser(userID, username, firstname, lastname, password, ssn, email, boss);

       }
   }
}
