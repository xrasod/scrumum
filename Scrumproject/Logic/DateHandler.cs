using System;
using System.Collections.Generic;
using System.Linq;

namespace Scrumproject.Logic
{
    public class DateHandler
    {
        public List<int> GetTimeDiffrence(string dpStartDate, string dpEndDate, int daysOff)
        {

            var startdate = DateTime.Parse(dpStartDate).Date;
            var enddate = DateTime.Parse(dpEndDate).Date;

            var dateDiffrence = enddate - startdate;

            var finalDateDiffrence = dateDiffrence.TotalDays;

            var totalWorkDays = finalDateDiffrence - daysOff;

            List<int> numbers = new List<int>();
            for (int i = 0; i < totalWorkDays + 1; i++)
            {
                numbers.Add(i);
            }
            return numbers;
        }
    }
}
        