using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrumproject.Logic.Entities
{
    public class TravelInfoEntity
    {
        public int Reportid { get; set; }
        public int Countryid { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int VacationDays { get; set; }
        public Countries VisitedCountry { get; set; }
    }
}
