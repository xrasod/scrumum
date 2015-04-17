using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
namespace Scrumproject.Logic
{
    class XmlReader
    {

        XmlDocument doc = new XmlDocument();
            doc.Load("Feeds.xml");
            var nodes = doc.SelectNodes("ListOfFeeds/ListaAvFeeds/Feed");
            {
                foreach (XmlElement node in nodes)
                {
                    if (node.SelectSingleNode("ValdKategori").InnerText == kategori)
                    {
                        listbox.Items.Add(node.SelectSingleNode("FeedName").InnerText);
                    }

                }
            }


    }
}
