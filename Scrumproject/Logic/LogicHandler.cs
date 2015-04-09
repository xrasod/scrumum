using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scrum.Data;
using Scrum.Data.Data;

namespace Scrumproject.Logic
{
   public class LogicHandler
    { 
      ReportRepository<Report> reportRepository = new ReportRepository<Report>();


      public void LoadDraft(string sokvag)
      {
          reportRepository.Ladda(sokvag);
      }
       
       public void SaveDraft(Report report, string sokvag)
       {
           reportRepository.Spara(report, sokvag);
       }

    }
}
