using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scrum.Data;
using Scrum.Data.Data;
using Scrumproject.Logic.Entities;
using iTextSharp.text.pdf;

namespace Scrumproject.Logic
{
   public class LogicHandler
    { 
      
      ReportRepository<Report> reportRepository = new ReportRepository<Report>();
      ReportRepository<Notes> notesRepository = new ReportRepository<Notes>();


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


   }
}
