
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





        public List<string> GetStatisticsOverCountriesWhereUsersBeen(string country)
        {
            var getAllReports = reportRepositoryMethodAccessor.GetAllReports();
            var getAllUsers = userRepositoryMethodAccessor.GetAllUsers();
            var getAllCountries = countriesRepositoryMethodAccesor.GetAllCountries();

            var filterUserDependingOnCountry = getAllUsers
                .Join(getAllReports, u => u.UID, r => r.UID, (u, r) => new {User = u, Report = r})
                .Join(getAllCountries, ur => ur.Report.TravelInfoes, c => c.TravelInfoes,
                    (ur, c) => new {user = ur, report = ur, Country = c})
                .Where(x => x.Country.Name == country)
                .Select(x => x.user.User.Username);
            return filterUserDependingOnCountry.ToList();
        }
    }
}