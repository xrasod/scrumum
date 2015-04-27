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

        

        public void SaveDeny(int id, string status, string motivation)
        {

            using (var context = new scrumEntities())
            {
                var updatePrePaymetquery = context.Prepayments.First(prepayId => prepayId.PID == id);
                updatePrePaymetquery.Status = status;
                updatePrePaymetquery.Description = motivation;
                {
                    context.SaveChanges();
                }
            }
        }


        public void SaveAccept(int id, string status)
        {
            using (var context = new scrumEntities())
            {
                var updatePaymetquery = context.Prepayments.First(paymentId => paymentId.PID == id);
                updatePaymetquery.Status = status;
                {
                    context.SaveChanges();
                }
            }
        }
    }
}
