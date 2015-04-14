using System;

namespace Scrumproject.Logic.Entities
{
    public class DateHandler
    {
        public int GetTimeDiffrence(DateTime startdate, DateTime endDate, int daysOff)
        {
            var timeDiffrence = startdate - endDate;
          
            int totalWorkDays = timeDiffrence.Days - daysOff;
            return totalWorkDays;
        }
    }
}
    