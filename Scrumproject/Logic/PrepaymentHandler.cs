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

    }
    

    
}
