using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrum.Data.Data
{
    public class CountriesRepository
    {
        public List<Country> GetAllCountries()
        {
            using (var context = new scrumEntities())
            {
                return context.Countries.OrderBy(x => x.Name).ToList();
            }
        }
    }
}
