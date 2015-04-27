
using System;
using System.Collections.Generic;
using System.Linq;
using Scrum.Data;
using Scrum.Data.Data;
using Scrumproject.Data;
 
namespace Scrumproject.Logic
{
    public class StatisticsHandler
    {
        
        private ReportTestClass reportRepositoryMethodAccessor = new ReportTestClass();
        private UserRepository userRepositoryMethodAccessor = new UserRepository();
        private CountriesRepository countriesRepositoryMethodAccesor = new CountriesRepository();
        private TravelRepository travelRepository = new TravelRepository();
        private ReportHandler reportHandler = new ReportHandler();
       

        public List<string> GetStatisticsOverCountriesWhereUsersBeen(string country)
        {
            
            var getAllReports = reportRepositoryMethodAccessor.GetAllReports();
            var getAllUsers = userRepositoryMethodAccessor.GetAllUsers();
            var getAllCountries = countriesRepositoryMethodAccesor.GetAllCountries();
            var getAllTravels =  travelRepository.GetAllTravels();

            var filterUserDependingOnCountry = getAllUsers
                .Join(getAllReports, u => u.UID, r => r.UID, (u, r) => new {User = u, Report = r})
                .Join(getAllTravels, ur => ur.Report.RID, c => c.RID,
                    (ur, c) => new {user = ur, report = ur, TravelInfo = c})
                .Join(getAllCountries, urc => urc.TravelInfo.CID, x => x.CID,
                    (urc, x) => new {user = urc, Report = urc, travelInfo = urc, country = x})
                .Where(urcx => urcx.country.Name == country)
                .Select(urcx => urcx.user.user.User.FirstName + "  "+ urcx.user.user.User.LastName);
            return filterUserDependingOnCountry.ToList();
        }

        public List<string> GetStatisticsOverTheCountriesAUsersBeenIn(string user)
        {
            var getAllReports = reportRepositoryMethodAccessor.GetAllReports();
            var getAllUsers = userRepositoryMethodAccessor.GetAllUsers();
            var getAllCountries = countriesRepositoryMethodAccesor.GetAllCountries();
            var getAllTravels = travelRepository.GetAllTravels();

            var filterUserDependingOnCountry = getAllUsers
                .Join(getAllReports, u => u.UID, r => r.UID, (u, r) => new { User = u, Report = r })
                .Join(getAllTravels, ur => ur.Report.RID, c => c.RID,
                    (ur, c) => new { user = ur, report = ur, TravelInfo = c })
                .Join(getAllCountries, urc => urc.TravelInfo.CID, x => x.CID,
                    (urc, x) => new { user = urc, Report = urc, travelInfo = urc, country = x })
                .Where(urcx => urcx.user.user.User.Username == user)
                .Select(urcx => urcx.country.Name);
            return filterUserDependingOnCountry.ToList();
        }

        public Decimal? GetSumOfASelectedUsersTravelDistances(string username)
        {
            
            var getAllReports = reportRepositoryMethodAccessor.GetAllReports();

            var getAllUsers = userRepositoryMethodAccessor.GetAllUsers();
            var sum = getAllReports.Join(getAllUsers, r => r.UID, u => u.UID, (r, u) => new {Report = r, User = u})
                .Where(x => x.User.Username == username)
                .Sum(x => x.Report.Kilometers);
                
                
            return sum;
        }

        public Decimal? GetSumOfReportMoney(string username)
        {
            var getAllReports = reportRepositoryMethodAccessor.GetAllReports();

            var getAllUsers = userRepositoryMethodAccessor.GetAllUsers();
            var sum = getAllReports.Join(getAllUsers, r => r.UID, u => u.UID, (r, u) => new { Report = r, User = u })
                .Where(x => x.User.Username == username)
                .Sum(x => x.Report.TotalAmount);

            return sum;
        }



        public Decimal? GetSumOfReportMoneySortedByDate(string username, DateTime startDate, DateTime endDate)
        {
            var getAllReports = reportRepositoryMethodAccessor.GetAllReports();

            var getAllUsers = userRepositoryMethodAccessor.GetAllUsers();
            var sum = getAllReports.Join(getAllUsers, r => r.UID, u => u.UID, (r, u) => new { Report = r, User = u })
                .Where(x => x.User.Username == username)
                
                .Where(x => x.Report.ReportDate <= startDate)
                .Where (x => x.Report.ReportDate >= endDate)
                .Sum(x => x.Report.TotalAmount);

            return sum;
        }

        public Decimal? GetSumOfASelectedUsersTravelDistancesSortedByDate(string username, DateTime startDate, DateTime endDate)
        {

            var getAllReports = reportRepositoryMethodAccessor.GetAllReports();

            var getAllUsers = userRepositoryMethodAccessor.GetAllUsers();
            var sum = getAllReports.Join(getAllUsers, r => r.UID, u => u.UID, (r, u) => new { Report = r, User = u })
                .Where(x => x.User.Username == username)
                 .Where(x => x.Report.ReportDate <= startDate)
                .Where(x => x.Report.ReportDate >= endDate)
                .Sum(x => x.Report.Kilometers);


            return sum;
        }

        public List<string> GetStatisticsOverTheCountriesAUsersBeenInSortedByDate(string user, DateTime startDate, DateTime endDate)
        {
            var getAllReports = reportRepositoryMethodAccessor.GetAllReports();
            var getAllUsers = userRepositoryMethodAccessor.GetAllUsers();
            var getAllCountries = countriesRepositoryMethodAccesor.GetAllCountries();
            var getAllTravels = travelRepository.GetAllTravels();

            var filterUserDependingOnCountry = getAllUsers
                .Join(getAllReports, u => u.UID, r => r.UID, (u, r) => new { User = u, Report = r })
                .Join(getAllTravels, ur => ur.Report.RID, c => c.RID,
                    (ur, c) => new { user = ur, report = ur, TravelInfo = c })
                .Join(getAllCountries, urc => urc.TravelInfo.CID, x => x.CID,
                    (urc, x) => new { user = urc, Report = urc, travelInfo = urc, country = x })
                .Where(urcx => urcx.user.user.User.Username == user)
                .Where(x => x.Report.report.Report.ReportDate <= startDate)
                .Where(x => x.Report.report.Report.ReportDate >= endDate)
                .Select(urcx => urcx.country.Name);
            return filterUserDependingOnCountry.ToList();
        }
        public List<string> GetStatisticsOverCountriesWhereUsersBeenSortedByDate(string country, DateTime startDate, DateTime endDate)
        {

            var getAllReports = reportRepositoryMethodAccessor.GetAllReports();
            var getAllUsers = userRepositoryMethodAccessor.GetAllUsers();
            var getAllCountries = countriesRepositoryMethodAccesor.GetAllCountries();
            var getAllTravels = travelRepository.GetAllTravels();

            var filterUserDependingOnCountry = getAllUsers
                .Join(getAllReports, u => u.UID, r => r.UID, (u, r) => new { User = u, Report = r })
                .Join(getAllTravels, ur => ur.Report.RID, c => c.RID,
                    (ur, c) => new { user = ur, report = ur, TravelInfo = c })
                .Join(getAllCountries, urc => urc.TravelInfo.CID, x => x.CID,
                    (urc, x) => new { user = urc, Report = urc, travelInfo = urc, country = x })
                .Where(urcx => urcx.country.Name == country)
                .Where(x => x.Report.report.Report.ReportDate <= startDate)
                .Where(x => x.Report.report.Report.ReportDate >= endDate)
                .Select(urcx => urcx.user.user.User.FirstName + "  " + urcx.user.user.User.LastName);
            return filterUserDependingOnCountry.ToList();
        }
        public List<string> SendReportToGuiDependingOnDate(string user, DateTime startDate, DateTime endDate)
        {


            var reportList = reportRepositoryMethodAccessor.GetAllReports();
            var userList = userRepositoryMethodAccessor.GetAllUsers();
            var filteredReportList = reportList.Join(userList, r => r.UID, u => u.UID,
                (r, u) => new { Report = r, User = u })
                .OrderBy(ur => ur.Report.RID)
                .Where(u => u.User.Username == user)
                .Where(x => x.Report.ReportDate <= startDate)
                .Where(x => x.Report.ReportDate >= endDate)
                .Select(ur => " Användare : " + " " + ur.User.Username + " " + " ID : " + ur.Report.RID + " " + "Status : " + ur.Report.Status).ToList();




            return filteredReportList;

        } 


        

       


        public List<string> SendCountriesToGui()
        {
            var countryList = countriesRepositoryMethodAccesor.GetAllCountries();
            var countryName = countryList.Select(x => x.Name).ToList();
            return countryName;
        }

        public List<string> SendUsersToGui()
        {
            var userList = userRepositoryMethodAccessor.GetAllUsers();
            var userName = userList.Select(x => x.Username).ToList();
            return userName;
        }

        public List<string> SendReportToGui(string user)
        {


            var reportList = reportRepositoryMethodAccessor.GetAllReports();
            var userList = userRepositoryMethodAccessor.GetAllUsers();
            var filteredReportList = reportList.Join(userList, r => r.UID, u => u.UID,
                (r, u) => new {Report = r, User = u})
                .OrderBy(ur => ur.Report.RID)
                .Where(u => u.User.Username == user)
                .Select(ur => " Användare : " + " " + ur.User.Username + " " + " ID : " + ur.Report.RID + " " + "Status : " +  ur.Report.Status).ToList();
            
            
             
           
            return filteredReportList;
        
        } 
       

      
    }
}