using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrum.Data.Data
{
    public class ReceiptRepository
    {
        public List<Reciept> GetAllReceipts()
        {
            using (var context = new scrumEntities())
            {
                return context.Reciepts.OrderBy(x => x.TravelReciept).ToList();
            }
        }
        public Reciept GetSingleReciept(int id)
        {
            using (var context = new scrumEntities())
            {
                return context.Reciepts.FirstOrDefault(x => x.RID.Equals(id));
            } 
        }

    }
}
