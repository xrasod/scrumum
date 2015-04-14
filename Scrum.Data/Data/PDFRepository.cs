using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;


namespace Scrum.Data.Data
{
    public class PDFRepository
    {

        public void createPdf(string report, string filnamn)
        {
            FileStream fs = new FileStream(filnamn, FileMode.Create, FileAccess.Write, FileShare.None);
            Document doc = new Document();
            PdfWriter writer = PdfWriter.GetInstance(doc, fs);
            doc.Open();
            doc.Add(new Paragraph(report));
            doc.Close();
        }




    }
}