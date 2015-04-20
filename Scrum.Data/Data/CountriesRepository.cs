using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrum.Data.Data
{
    public class CountriesRepository
    {
        public List<Country> GetAllCountries()
        {
            using (var context = new scrumEntities())
            {
                return context.Countries.OrderBy(x => x.Name).ToList();
            }
        }

        public Country GetSpecificsFromCountry(string country)
        {
            using (var context = new scrumEntities())
            {
                return context.Countries.FirstOrDefault(x => x.Name.Equals(country));
            }
        }


        public void addCountry(Country C)
        {

            using (var context = new scrumEntities())
            {
                try
                {

                    context.Countries.Add(C);

                    context.SaveChanges();
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName,
                                validationError.ErrorMessage);
                        }
                    }
                }

            }
        }

        public static void UpdateCountry(int cID,string newname, string newcurrency, int newsub)
        {
            using (var context = new scrumEntities())
            {

                try
                {
                    var c = context.Countries
                        .FirstOrDefault(x => x.CID == cID);
                    c.Name = newname;
                    c.Currency = newcurrency;
                    c.Subsistence = newsub;
                    


                    context.SaveChanges();
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName,
                                validationError.ErrorMessage);
                        }
                    }
                }
            }

        }

        public void DeleteCountry(Country c)
        {
            using (var context = new scrumEntities())
            {
                try
                {

                    context.Countries.Remove(c);

                    context.SaveChanges();
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName,
                                validationError.ErrorMessage);
                        }
                    }
                }
            }
        }
    }
}
