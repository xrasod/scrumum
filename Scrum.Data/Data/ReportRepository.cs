using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Scrum.Data.Data
{
    public class ReportRepository<T> where T: class 
    {
        internal XmlSerializer Xml;

        public ReportRepository()
        {
            Xml = new XmlSerializer(typeof(T));
        }

        public void Spara(T obj, string sokvag)
        {
            using (var streamwriter = new StreamWriter(sokvag))
            {
                Xml.Serialize(streamwriter, obj);
            }
        }

        public T Ladda(string sokvag)
        {
            
            
                using (var streamreader = new StreamReader(sokvag))
                {
                    return Xml.Deserialize(streamreader) as T;
                }
            
            
        }

        public List<Report> GetAllReports()
        {
            using (var context = new scrumEntities())
            {
                return context.Reports.ToList();
            }
        }

    }



}
