using System;
using System.Collections.Generic;

namespace Scrumproject.Logic.Entities
{
    public class RealReportEntity
    {
        public int Rid { get; set; }
        public int Uid { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public decimal? KilometersDriven { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime ReportDate { get; set; }
        public List<ReceiptsEntity> ListOfReceipts { get; set; }
        public TravelInfoEntity TravelInfo { get; set; }


    }
}