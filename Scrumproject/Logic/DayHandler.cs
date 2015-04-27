using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scrum.Data;
using Scrum.Data.Data;
using Scrumproject.Logic.Entities;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;


namespace Scrumproject.Logic
{
    public class DayHandler
    {
        public string date { set; get; }
        public string lastdate { set; get; }
        public string country { set; get; }
        //public bool vacation { set; get; }
        public double subsistence { set; get; }
        public string accountName = "scrumprojekt";
        public string accessKey = "J0b3PkuN5FHd5QNjnjS7080NRdWAILm/uSJV32rhEjB8Sxw3tuKyGyyqsi9JxM0LrfvVx7U1qzJN4uNJdn88cw==";

        public int StoreReport(List<DayHandler> list, decimal distance, string description, decimal totalAmount, string user, List<RecieptHandler> listan)
        {
            Report rp = new Report();
            TravelInfo ti = new TravelInfo();
            UserRepository ur = new UserRepository();
            var userid = ur.GetUserId(user);
 
            rp.UID = userid;
            rp.Status = "Pågående";
            rp.Description = description;
            rp.Kilometers = distance;
            rp.TotalAmount = totalAmount;
            rp.ReportDate = DateTime.Now;

            int i = ur.SaveReport(rp);
            StoreReciept(i, listan);
            StoreTravelInfo(i, list);
            return i;
        }

        public void StoreReciept(int i, List<RecieptHandler> listan)
        {
           

            try
            {
                StorageCredentials creds = new StorageCredentials(accountName, accessKey);
                CloudStorageAccount account = new CloudStorageAccount(creds, useHttps: true);

                CloudBlobClient client = account.CreateCloudBlobClient();
                //Den Storage container jag skapat för att lagra kvitton
                //Här kan vi göra valet att lägga alla kvitton i samma container (mapp) och autogenerera kvittonamnen så att de blir unika
                //Alternativt kan man skapa en container för varje användare. Vet faktiskt inte vad som är smartast. 
                CloudBlobContainer kvittoContainer = client.GetContainerReference("kvitton");
                kvittoContainer.CreateIfNotExists();
                Random random = new Random();
                
                foreach (var item in listan)
                { 

                 string sokvag = random.Next(1000,1000000000).ToString() + ".jpg";
                CloudBlockBlob blob = kvittoContainer.GetBlockBlobReference(sokvag); //Här sätter vi namnet på filen när den laddas upp. Viktigt att få med .jpg liknande
                using (Stream file = System.IO.File.OpenRead(item.TravelReciept)) //Använder sökvägen ifrån "openfiledialog" och skapar en stream
               

                  
                
                {
                    blob.UploadFromStream(file); //Själva uppladdningen
                    Reciept ri = new Reciept();
                    ri.RID = i;
                    ri.TravelReciept = "http://scrumprojekt.blob.core.windows.net/kvitton/" + sokvag;
                    ri.RecieptAmount = item.RecieptAmount;
                    UserRepository.SaveReciept(ri);
                }
                
                }
                


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
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
            var lastloop = 0;
         
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
                    if (lastloop != 1)
                    {
                        Country c = new Country();
                        c = UserRepository.GetCountryID(country);
                        TravelInfo ti = new TravelInfo();
                        
                        
                        DateTime startdate = DateTime.Parse(lastdate.First());
                        DateTime enddate = DateTime.Parse(lastdate.Last());
                        ti.RID = RID;
                        ti.CID = c.CID;
                        ti.StartDate = startdate;
                        ti.EndDate = enddate;
                        ti.VacationDays = VACdays;
                        UserRepository.SaveTravelInfo(ti);
                    }
                    
                    lastdate.Clear();
                    lastdate.Add(item.date);
                    count = 0;
                    VACdays = 0;
                 

                }
                if(item.country.Equals("hej"))
                {
                    lastloop = 1;
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
