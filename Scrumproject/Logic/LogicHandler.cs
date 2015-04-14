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
      
      ReportRepository<ReportDraft> reportRepository = new ReportRepository<ReportDraft>();
      ReportRepository<Notes> notesRepository = new ReportRepository<Notes>();
      BossRepository bossRepository = new BossRepository();
      PDFRepository pdfRepository = new PDFRepository();

       public void CreatePdf(string text, string filnamn)
       {
           pdfRepository.createPdf(text, filnamn);
       }


      public ReportDraft LoadDraft(string sokvag)
      {
          
          
              return reportRepository.Ladda(sokvag);
          
          
      }
       
       public void SaveDraft(ReportDraft reportDraft, string sokvag)
       {
           reportRepository.Spara(reportDraft, sokvag);
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
