using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scrum.Data;
using Scrum.Data.Data;

namespace Scrumproject.Logic
{
    public class DayHandler
    {
        public string date { set; get; }
        public string lastdate { set; get; }
        public string country { set; get; }
        //public bool vacation { set; get; }
        public double subsistence { set; get; }
        public void StoreReport(List<DayHandler> list, decimal distance, string description, decimal totalAmount, string user, List<string> reciept)
        {
            Report rp = new Report();
            TravelInfo ti = new TravelInfo();
            UserRepository ur = new UserRepository();
            var userid = ur.GetUserId(user);
 
            rp.UID = userid;
            rp.Status = null;
            rp.Description = description;
            rp.Kilometers = distance;
            rp.TotalAmount = totalAmount;
            rp.ReportDate = DateTime.Now;

            int i = ur.SaveReport(rp);
            StoreReciept(i, reciept);
            StoreTravelInfo(i, list);
        }

        public void StoreReciept(int i, List<string> reciept)
        {
            foreach (var r in reciept)
            {
                Reciept ri = new Reciept();
                ri.RID = i;
                ri.TravelReciept = r;
                UserRepository.SaveReciept(ri);
            }
        }

        public static void StoreTravelInfo(int RID, List<DayHandler> list)
        {
            DayHandler d = new DayHandler();
            d.country = "hej";
            d.subsistence = 1;
            list.Add(d);
            var count = 0;
            var country = "";
            var VACdays = 0;
            List<DayHandler> countryList = new List<DayHandler>();
            List<string> lastdate = new List<string>();


            foreach (var item in list)
            {

                var date = item.date;
                var subsistence = item.subsistence;

                if (country == item.country || count == 0)
                {
                    lastdate.Add(date);
                }
                if (country != item.country && count != 0)
                {
                    if (item.date != null)
                    {
                        Country c = new Country();
                        c = UserRepository.GetCountryID(item.country);
                        TravelInfo ti = new TravelInfo();
                        //DateTime startdate = DateTime.ParseExact(date, "yyyy-MM-dd HH:mm", System.Globalization.CultureInfo.InvariantCulture);
                        //DateTime enddate = DateTime.ParseExact(lastdate.Last(), "yyyy-MM-dd HH:mm", System.Globalization.CultureInfo.InvariantCulture);
                        DateTime startdate = DateTime.Parse(lastdate.First());
                        DateTime enddate = DateTime.Parse(lastdate.Last());
                        ti.RID = RID;
                        ti.CID = c.CID;
                        ti.StartDate = startdate;
                        ti.EndDate = enddate;
                        ti.VacationDays = VACdays;
                        UserRepository.SaveTravelInfo(ti);
                    }
                    Console.WriteLine(lastdate.First());
                    Console.WriteLine(lastdate.Last());
                    Console.WriteLine(VACdays);

                    //d.date = date;
                    ////d.lastdate = 
                    //d.subsistence = subsistence;
                    //countryList.Add(d);
                    //foreach (var c in countryList)
                    //{
                    //    Console.WriteLine(c.country);

                    //Console.WriteLine(d.date);
                    //Console.WriteLine(d.lastdate);

                    //    Console.WriteLine(c.date.LastOrDefault());
                    //}
                    lastdate.Clear();
                    lastdate.Add(item.date);
                    count = 0;
                    VACdays = 0;
                    //if (item.subsistence == 0)
                    //{
                    //    //checkVAC = 1;
                    //    VACdays++;
                    //}

                }
                if (item.subsistence == 0)
                {
                    //checkVAC = 0;
                    VACdays++;
                }
                country = item.country;
                count++;
            }



        }

        public decimal CalculateTotalAmount(List<DayHandler> list, double km, double recieptAmount)
        {
            double kmAmount = 0;
            double totalSubsistance = 0;
            if (km != 0)
            {
                kmAmount = km * 18.5;
            }
            foreach (var item in list)
            {
                totalSubsistance += item.subsistence;
            }
            var total = recieptAmount + totalSubsistance + kmAmount;
            decimal totalDec = Convert.ToDecimal(total);
            return totalDec;

        }
    }
}
