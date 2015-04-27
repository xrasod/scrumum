using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Scrum.Data;
using Scrumproject.Logic;

namespace Scrumproject.GUI
{
    /// <summary>
    /// Interaction logic for SeeReportInfoWIndow.xaml
    /// </summary>
    public partial class SeeReportInfoWIndow : Window
    {
        
        PrepaymentHandler prepayHandler = new PrepaymentHandler();
        LogicHandler logic = new LogicHandler();
        ReportHandler reportHandler = new ReportHandler();
        Report report = new Report();
        
        public SeeReportInfoWIndow()
        {
            InitializeComponent();
            try
            {
                report = SelectedReport();
            }
            catch { }

        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        private void btnApproveReport_Click(object sender, RoutedEventArgs e)
        {
            ApproveReport();
        }

        private void btnDenyReport_Click(object sender, RoutedEventArgs e)
        {
            DenyReport();
        }

        public void ApproveReport()
        {
            try
            {
                if (MainWindow.main.cbShowPrepayments.IsChecked == true)
                {
                    string Prepaymentwindowfullstring = MainWindow.main.lbShowReports.SelectedItem.ToString();
                    prepayHandler.SaveStatusUpdateForAccept(Prepaymentwindowfullstring);
                    MainWindow.main.lbShowReports.ItemsSource = null;
                    MainWindow.main.lbShowReports.ItemsSource = prepayHandler.GetAllPrepaymentsRequest();

                }
                else
                {

                    string fullstringbitch = report.RID.ToString();
                    reportHandler.Acceptpost(fullstringbitch);
                    MainWindow.main.lbShowReports.ItemsSource = null;
                    MainWindow.main.lbShowReports.ItemsSource = reportHandler.GetReportList();
                    var updatedReport = logic.GetSingleReport(report.RID);
                    lblStatusOnReport.Content = updatedReport.Status;
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show("Du måste välja en rapport eller förskottsansökan att godkänna!");
            }


        }
        public void DenyReport()
        {
            try
            {
                if (MainWindow.main.cbShowPrepayments.IsChecked == true)
                {

                    string Prepaymentwindowfullstring = MainWindow.main.lbShowReports.SelectedItem.ToString();
                    string Motivation = MainWindow.main.tbWhyDenied.Text;
                    prepayHandler.SaveStatusUpdateForDenial(Prepaymentwindowfullstring, Motivation);
                    MainWindow.main.lbShowReports.ItemsSource = null;
                    MainWindow.main.lbShowReports.ItemsSource = prepayHandler.GetAllPrepaymentsRequest();

                }

                else
                {
                    string reportwindowfullstring = report.RID.ToString();
                    string motivation = MainWindow.main.tbWhyDenied.Text;
                    reportHandler.SaveStatusUpdateForDenial(reportwindowfullstring, motivation);
                    MainWindow.main.lbShowReports.ItemsSource = null;
                    MainWindow.main.lbShowReports.ItemsSource = reportHandler.GetReportList();
                    var updatedReport = logic.GetSingleReport(report.RID);
                    lblStatusOnReport.Content = updatedReport.Status;
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show("Du måste välja en rapport eller förskottsansökan att neka!");
            }


        }

        public Report SelectedReport()
        {
            try
            {
                var selected = MainWindow.main.lbShowReports.SelectedValue.ToString();
                var id = logic.checkIfDigits(selected);
                var selectedReport = logic.GetSingleReport(id);
                lblStatusOnReport.Content = selectedReport.Status;
                return selectedReport;
            }
            catch { }

            return report;
        }

    }
}
