using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scrum.Data;
using Scrum.Data.Data;

namespace Scrumproject.Logic
{
    public class PrepaymentHandler
    {
        AdvancePaymentsRepository prepaymentRepository = new AdvancePaymentsRepository();
        UserRepository users = new UserRepository();



        public List<string> GetAllPrepaymentsRequest()
        {
            var prepayment = prepaymentRepository.GetAllPrepayments();
            var userList = users.GetAllUsers();
            var listOfPreypaments = (from pre in prepayment join user in userList on pre.UID equals user.UID 
             orderby pre.PID
                                     select pre.PID + ". Anv: " + user.FirstName +" "+ user.LastName+ ", Summa: " + pre.Amount + ", Beskrivning: " + pre.Description
                                     + ", Status: " + pre.Status).ToList();

            return listOfPreypaments;
        }

        public int CheckPrePaymentId(string s)
        {
            var b = string.Empty;
            int val = 0;

            b = s.Where(t => Char.IsDigit(t)).Aggregate(b, (current, t) => current + t);

            if (b.Length > 0)
                val = Int32.Parse(b);

            return val;
        }

        public void SaveStatusUpdateForDenial(string fullPrepayment, string motivation)
        {
            string PrePaymentString = fullPrepayment.Substring(0,2);
            var PrePaymentId = CheckPrePaymentId(PrePaymentString);


            string setStatusNotAccepted = "Nekad";
            prepaymentRepository.SaveDeny(PrePaymentId, setStatusNotAccepted, motivation);

        }

         public void SaveStatusUpdateForAccept(string fullPrepayment)
        {
            string PrePaymentString = fullPrepayment.Substring(0, 2);
            var PrePaymentId = CheckPrePaymentId(PrePaymentString);

            string setStatusAccepted = "Godkänd";
            prepaymentRepository.SaveAccept(PrePaymentId, setStatusAccepted);
            

        }

        
    }
    

    
}
