using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrumproject.Logic
{
    public class Validator
    {
        public static bool ControlInputConverter(string input)
        {
            double i;
            
            if (Double.TryParse(input, out i))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
