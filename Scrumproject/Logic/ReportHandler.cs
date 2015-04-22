﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using Scrum.Data;
using Scrum.Data.Data;
using Scrumproject.Data;
using Scrumproject.Logic.Entities;

namespace Scrumproject.Logic
{
    public class ReportHandler
    {

        ReportTestClass ReportTestClass = new ReportTestClass();
        UserRepository UserRepository = new UserRepository();
        PDFRepository pdfRep = new PDFRepository();



        public List<string> GetReportList()
        {
           
            var reportList = ReportTestClass.GetAllReports();
            var userList = UserRepository.GetAllUsers();
            var filteredReportList = reportList.Join(userList, r => r.UID, u => u.UID,
                (r, u) => new {Report = r, User = u})
                .OrderBy(ur => ur.Report.RID)

                .Select(ur => " Användare : " + " " + ur.User.Username + " " + " ID : " + ur.Report.RID + " " + "Status : " +  ur.Report.Status).ToList();
            
            
             
           
            return filteredReportList;
        }

        

        public int GetReportId(string reportname)
        {


            var reportList = ReportTestClass.GetAllReports();

            int reportId = reportList.Where(y => y.Description == reportname).Select(x => x.RID).FirstOrDefault();



            return reportId;
        }

        public void Rejectpost(string reportname)
        {
            int reportId = CheckReportId(reportname);

            string setStatusNotAccepted = "Nekad";
            ReportTestClass.SaveUpdatedReportStatus(reportId, setStatusNotAccepted);
        }

        public void Acceptpost(string reportname)
        {
            int reportId = CheckReportId(reportname);
            string setStatusAccepted = "Godkänd";
            ReportTestClass.SaveUpdatedReportStatus(reportId,setStatusAccepted);
        }
        public int CheckReportId(string s)
        {
            var b = string.Empty;
            int val = 0;

            b = s.Where(t => Char.IsDigit(t)).Aggregate(b, (current, t) => current + t);

            if (b.Length > 0)
                val = Int32.Parse(b);

            return val;
        }

        public void createPdfFromDbReport(int id)
        {
            var report = ReportTestClass.GetSingleReport(id);

            var pdfReport = "Inskickad av: " + report.User.FirstName + report.User.LastName;

            pdfRep.createPdfandOpen(pdfReport, DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".pdf");


        }


    }
}