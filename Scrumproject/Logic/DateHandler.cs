using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Globalization;

namespace Scrumproject.Logic
{
    public class DateHandler
    {
        public BindingList<string> GetTimeDiffrence(string dpStartDate, string dpEndDate)
        {

            var startdate = DateTime.Parse(dpStartDate).Date;
            var enddate = DateTime.Parse(dpEndDate).Date;

            var dateDiffrence = enddate - startdate;

            var finalDateDiffrence = dateDiffrence.TotalDays;

            var totalWorkDays = finalDateDiffrence;

            BindingList<string> numbers = new BindingList<string>();
            for (int i = 1; i < totalWorkDays + 2; i++)
            {
                numbers.Add(i.ToString());
            }
            return numbers;
        }

        public List<DateTime> GetDays(DateTime startDate, DateTime endDate)
        {
            List<DateTime> allDates = new List<DateTime>();

            int starting = startDate.Day;
            int ending = endDate.Day;

            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {            
                allDates.Add(date);
            }       

            return allDates;
        }
    }
}
        