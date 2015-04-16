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

        //internal XmlSerializer Xml;

        //public CountryXML()
        //{
        //    Xml = new XmlSerializer(typeof(T));
        //}

        //public void Spara(T obj, string sokvag)
        //{
        //    using (var streamwriter = new StreamWriter(sokvag))
        //    {
        //        Xml.Serialize(streamwriter, obj);
        //    }
        //}

        //public T Ladda(string sokvag)
        //{
        //    using (var streamreader = new StreamReader(sokvag))
        //    {
        //        return Xml.Deserialize(streamreader) as T;
        //    }
        //}




    }
}