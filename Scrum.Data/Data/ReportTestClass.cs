using System.Collections.Generic;
using System.Linq;

namespace Scrum.Data.Data
{
    public class ReportTestClass
    {
        public List<Report> GetAllReports()
        {
            using (var context = new scrumEntities())
            {
                return context.Reports.ToList();
            }
        }

        public void SaveUpdatedReportStatus(int id, string status)
        {

            using (var context = new scrumEntities())
            {
                var updatereportquery = context.Reports.First(reportId => reportId.RID == id);
                updatereportquery.Status = status;
                {
                    context.SaveChanges();
                }
            }
        }

        public Report GetSingleReport(int id)
        {
            using (var context = new scrumEntities())
            {
                return context.Reports.FirstOrDefault(x => x.RID == id);
            }
        } 


    }
}