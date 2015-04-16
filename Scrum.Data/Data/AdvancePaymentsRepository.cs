using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrum.Data.Data
{
    public class AdvancePaymentsRepository
    {
        public void AddPrepaymentRequest(Prepayment prepayment)
        {
            using (var context = new scrumEntities())
            {
                context.Prepayments.Add(prepayment);
            }
        }
    }
}
