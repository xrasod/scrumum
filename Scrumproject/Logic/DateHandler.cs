using System;

namespace Scrumproject.Logic
{
    public class DateHandler
    {
        public double GetTimeDiffrence(string dpStartDate, string dpEndDate, int daysOff)
        {

            var startdate = DateTime.Parse(dpStartDate).Date;
            var enddate = DateTime.Parse(dpEndDate).Date;

            var dateDiffrence = enddate - startdate;

          var finalDateDiffrence  = dateDiffrence.TotalDays;
        
            var totalWorkDays = finalDateDiffrence - daysOff;
            return totalWorkDays;
        }
    }
}