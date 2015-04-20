using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml;
namespace Scrumproject.Logic
{
    class XmlReader
    {
        public void ShowCountryData(ListBox listBox, string traktamente, string landnamn, string valuta)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("Country.xml");
            var nodes = doc.SelectNodes("ArrayOfCountries/Countries");
            foreach (XmlElement node in nodes)
            {
                if (node.SelectSingleNode("Name").InnerText == listBox.SelectedValue.ToString())
                {
                    node.SelectSingleNode("Subsistence").InnerText = traktamente;
                    node.SelectSingleNode("Name").InnerText = landnamn;
                    node.SelectSingleNode("Currency").InnerText = valuta;
                }

            }
        }

        public void LoadCbWithCountries(ListBox listBox)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("Country.xml");
            var nodes = doc.SelectNodes("ArrayOfCountries/Countries");
            foreach (XmlElement node in nodes)
            {
                listBox.Items.Add(node.SelectSingleNode("Name").InnerText);
            }
        }


    }
}