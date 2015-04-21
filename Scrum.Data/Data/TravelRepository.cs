using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrum.Data.Data
{
    public class TravelRepository
    {
        public List<TravelInfo> GetAllTravels()
        {
            using (var context = new scrumEntities())
            {
                return context.TravelInfoes.ToList();
            }
        }
    }
}
