using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

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

        //Kollar så en textbox inte är tom.
        public bool ControllFiledNotEmpty(TextBox i)
        {
            if (!string.IsNullOrEmpty(i.Text))
            {
                
                return false;

            }
            return true;

        }

        
    }


}
