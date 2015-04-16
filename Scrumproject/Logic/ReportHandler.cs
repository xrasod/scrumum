using System.Collections.Generic;
using System.Linq;
using Scrum.Data;
using Scrum.Data.Data;
using Scrumproject.Data;

namespace Scrumproject.Logic
{
    public class ReportHandler
    {

        ReportRepository<Report> reportRepository = new ReportRepository<Report>();



        public List<Report> GetSortedReportList()
        {
           
            var reportList = reportRepository.GetAllReports();

           // var filteredReportList = reportList.OrderBy(x => x.)
            return null;
        }

        public void BossRejectPost(string reportname)
        {
            

            var reportList = reportRepository.GetAllReports();

            var rejectspecificpost = reportList.Select(x => x.Description.Contains(reportname)).FirstOrDefault();

        List<Report> updatedPost = 
        }
    }
}