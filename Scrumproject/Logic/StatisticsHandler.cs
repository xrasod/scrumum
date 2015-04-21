
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

        public List<string> SendCountriesToGui()
        {
            var countryList = countriesRepositoryMethodAccesor.GetAllCountries();
            var countryName = countryList.Select(x => x.Name).ToList();
            return countryName;
        } 
    }
}