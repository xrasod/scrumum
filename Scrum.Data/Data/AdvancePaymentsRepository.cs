using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrum.Data.Data
{
    public class AdvancePaymentsRepository
    {
        public void AddAdvancePayment(Prepayment prepayment)
        {
            using (var context = new scrumEntities())
            {
                context.Prepayments.Add(prepayment);
                context.SaveChanges();
            }
        }

        public List<Prepayment> GetAllPrepayments()
        {
            using (var context = new scrumEntities())
            {
                return context.Prepayments.OrderBy(x => x.PID).ToList();
            }
        }
    }
}
