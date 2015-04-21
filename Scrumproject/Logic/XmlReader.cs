using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Xml;
using Scrum.Data;
using Scrum.Data.Data;
using Scrumproject.Logic.Entities;

namespace Scrumproject.Logic
{

    class XmlReader
    {
        CountryXML<Countries> countryxml = new CountryXML<Countries>();
        private string path = "Country.xml";


        public List<string> GetAllCountries()
        {

            var GetXML = countryxml.Ladda(path);
            var getAllCountries = GetXML.Select(x => x.Name).ToList();

            return getAllCountries;

        }

        public string getCountyValuta(string CountryName)
        {

            var GetXML = countryxml.Ladda(path);
            string currency = GetXML.Where(x => x.Name == CountryName).Select(x => x.Currency).FirstOrDefault();

            return currency;
        }

        public int getCountyTraktamente(string CountryName)
        {

            var GetXML = countryxml.Ladda(path);
            int traktamente = GetXML.Where(x => x.Name == CountryName).Select(x => x.Subsistence).FirstOrDefault();

            return traktamente;
        }


    }
}