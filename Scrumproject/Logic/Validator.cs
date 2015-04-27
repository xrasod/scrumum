using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Mapping;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Scrumproject.Logic
{
    public class Validator
    {
        bool invalid = false;

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

        //Kollar om det är en korrekt mailadress.
        public bool IsEmailValid(string mail)
        {
            return !Regex.IsMatch(mail,
                @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
        }

        //Kollar så att personnummer är inmatat rätt.
        public bool IsSsnValid(string persnr)
        {
            return !Regex.IsMatch(persnr, @"^[12]{1}[90]{1}[0-9]{6}-[0-9]{4}$");

        }

        //Kollar om ett lbvärde är null
        public bool IsLbEmptyPDF(ListBox listBox)
        {
            if (listBox.Items.IsEmpty)
            {
                MessageBox.Show("Du måste välja en rapport att visa i PDF!");
                return false;
            }
            else
            {
                return true;
            }         
        }

        public static bool CheckIfText(string input)
        {
            if (input.All(Char.IsLetter))
            {
                return false;
            }
            else
            {              
                return true;
            }
        }



       
        
    }


}
