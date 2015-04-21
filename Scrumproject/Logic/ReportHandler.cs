using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using Scrum.Data;
using Scrum.Data.Data;
using Scrumproject.Data;

namespace Scrumproject.Logic
{
    public class ReportHandler
    {

        ReportTestClass ReportTestClass = new ReportTestClass();
        UserRepository UserRepository = new UserRepository();



        public List<string> GetReportList()
        {
           
            var reportList = ReportTestClass.GetAllReports();
            var userList = UserRepository.GetAllUsers();
            var filteredReportList = reportList.Join(userList, r => r.UID, u => u.UID,
                (r, u) => new {Report = r, User = u})
                .OrderByDescending(ur => ur.Report.ReportDate)
                
                .Select(ur => ur.User.Username +  "    -  " + ur.Report.RID + "   -  " +ur.Report.Status ).ToList();
            
            
             
           
            return filteredReportList;
        }

        

        public int GetReportId(string reportname)
        {


            var reportList = ReportTestClass.GetAllReports();

            int reportId = reportList.Where(y => y.Description == reportname).Select(x => x.RID).FirstOrDefault();



            return reportId;
        }

        public void Rejectpost(string reportname)
        {
            int reportId = GetReportId(reportname);


            ReportTestClass.SaveUpdatedReportStatus(reportId, false);
        }

        public void Acceptpost(string reportname)
        {
            int reportId = GetReportId(reportname);
            ReportTestClass.SaveUpdatedReportStatus(reportId, true);
        }
    }
}