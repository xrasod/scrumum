using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Scrumproject.Logic
{
    public class DateHandler
    {
        public BindingList<string> GetTimeDiffrence(string dpStartDate, string dpEndDate, int daysOff)
        {

            var startdate = DateTime.Parse(dpStartDate).Date;
            var enddate = DateTime.Parse(dpEndDate).Date;

            var dateDiffrence = enddate - startdate;

            var finalDateDiffrence = dateDiffrence.TotalDays;

            var totalWorkDays = finalDateDiffrence - daysOff;

            BindingList<string> numbers = new BindingList<string>();
            for (int i = 0; i < totalWorkDays + 1; i++)
            {
                numbers.Add(i.ToString());
            }
            return numbers;
        }
    }
}
        