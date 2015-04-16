using System.Collections.Generic;
using System.Linq;
using Scrum.Data;
using Scrum.Data.Data;

namespace Scrumproject.Logic
{
    public class FileHandler
    {
        public List<Report> GetNewestReports()
        {
            ReportRepository<Report> reportrepository = new ReportRepository<Report>();
            var reportList = reportrepository.GetAllReports();
            var newestreports = reportList.OrderByDescending(x => x.)
            return null;
        }
    }
}