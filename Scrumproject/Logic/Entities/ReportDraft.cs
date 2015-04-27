using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scrum.Data;

namespace Scrumproject.Logic
{
    public class ReportDraft
    {
        
        public string Description { get; set; }
        public int NumberOfKilometersDrivenInTotal { get; set; }
        public int KilometersDriven { get; set; }
        public int TotalSumOfSpending { get; set; }
        public List<string> imagePathsList { get; set; }
        public List<string> daysSpentInCountry { get; set; } 
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string TotalReceiptAmount { get; set; }

    }
}
