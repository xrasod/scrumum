﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scrumproject.CurrencyConvertor;
using Scrum.Data;
using Scrum.Data.Data;
using System.Net;

namespace Scrumproject.Logic.Entities
{
    public class CurrencyConverter
    {
        public List<Country> GetCountries()
        {
            var rept = new CountriesRepository();
            var hej = rept.GetAllCountries();
            return hej;
        }

        public string ConvertCurrency(string FromCurrency, string ToCurrency, double amount)
        {   
            WebClient web = new WebClient();

            const string urlPattern = "http://finance.yahoo.com/d/quotes.csv?s={0}{1}=X&f=l1";
            string url = String.Format(urlPattern, FromCurrency,ToCurrency);
            string response = web.DownloadString(url);

            double exchangeRate =
            double.Parse(response, System.Globalization.CultureInfo.InvariantCulture);

            string CurrencyAmount = 
            String.Format("{0} {1} = {2} {3}" , amount,FromCurrency,amount * exchangeRate, ToCurrency);

            return CurrencyAmount;
        }

        public Country GetSelectedCountrySpecifics(string country)
        {
            CountriesRepository r = new CountriesRepository();

            return r.GetSpecificsFromCountry(country);
        }

    }
}
