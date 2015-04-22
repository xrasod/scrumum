using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Scrum.Data.Data
{
    public class CountryXML<T> where T : class
    {

        public XmlSerializer Xml;


        public void Spara(List<T> obj, string sokvag)
        {
            Xml = new XmlSerializer(typeof(List<T>));
            using (var streamwriter = new StreamWriter(sokvag))
            {
                Xml.Serialize(streamwriter, obj);
            }
        }

        public List<T> Ladda(string sokvag)
        {
            Xml = new XmlSerializer(typeof(List<T>));
            using (var streamreader = new StreamReader(sokvag))
            {
                return Xml.Deserialize(streamreader) as List<T>;
            }
        }

    }
}