using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrumproject.Logic
{
    public class Report
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int Status { get; set; }
        public string Description { get; set; }
        public int NumberOfKilometersDriven { get; set; }
        public int TotalSumOfSpending { get; set; }
    }
}
