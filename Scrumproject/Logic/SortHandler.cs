using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scrum.Data;
using Scrum.Data.Data;

namespace Scrumproject.Logic
{
   public class SortHandler
    {

        ReportTestClass ReportTestClass = new ReportTestClass();
        UserRepository UserRepository = new UserRepository();

       public List<String> GetCbSortList()
       {
          ReportTestClass reporttestClass = new ReportTestClass();
           var listofreports = reporttestClass.GetAllReports();

           var getcategories = listofreports.Select(x => x.Status).Distinct().ToList();

           return getcategories;
       }

       public List<String> GetSortByStatusResult(string status)
       {
           var reportList = ReportTestClass.GetAllReports();
           var userList = UserRepository.GetAllUsers();
           var filteredReportList = reportList.Join(userList, r => r.UID, u => u.UID,
               (r, u) => new { Report = r, User = u }).Where(r => r.Report.Status == status)
               .OrderBy(ur => ur.Report.RID)

               .Select(ur => " Användare : " + " " + ur.User.Username + " " + " ID : " + ur.Report.RID + " " + "Status : " + ur.Report.Status).ToList();




           return filteredReportList;
       }

       public List<String> GetReportsForSpecificUser(int id)
       {
           var reportList = ReportTestClass.GetAllReports();
           var userList = UserRepository.GetAllUsers();
           var filteredReportList = reportList.Join(userList, r => r.UID, u => u.UID,
               (r, u) => new { Report = r, User = u }).Where(r => r.Report.UID == id)
               .OrderBy(ur => ur.Report.RID)

               .Select(ur => " Användare : " + " " + ur.User.Username + " " + " ID : " + ur.Report.RID + " " + "Status : " + ur.Report.Status).ToList();




           return filteredReportList;
       }

    }
}
