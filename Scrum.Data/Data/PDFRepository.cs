using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Document = iTextSharp.text.Document;


namespace Scrum.Data.Data
{
    public class PDFRepository
    {

        public void createPdf(string report, string filnamn)
        {
            FileStream fs = new FileStream(filnamn, FileMode.Create, FileAccess.Write, FileShare.None);
            Document doc = new iTextSharp.text.Document();
            PdfWriter writer = PdfWriter.GetInstance(doc, fs);
            doc.Open();
            doc.Add(new Paragraph(report));
            doc.Close();
        }

        public void createPdfandOpen(string report, string filnamn)
        {
            FileStream fs = new FileStream(filnamn, FileMode.Create, FileAccess.Write, FileShare.None);
            Document doc = new iTextSharp.text.Document();
            PdfWriter writer = PdfWriter.GetInstance(doc, fs);
            doc.Open();
            doc.Add(new Paragraph(report));
            doc.Close();
            System.Diagnostics.Process.Start(filnamn);
        }




    }
}