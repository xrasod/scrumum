using System;
using System.Collections.Generic;
using System.Linq;
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
using Scrumproject.Logic;

namespace Scrumproject.GUI
{
    /// <summary>
    /// Interaction logic for ShowAllMyReports.xaml
    /// </summary>
    public partial class ShowAllMyReports : Window
    {
        ReportHandler reportHandler = new ReportHandler();
        LogicHandler logic = new LogicHandler();
        
        
        public ShowAllMyReports()
        {
            InitializeComponent();
        }

        private void listBoxMyReports_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            var rid = logic.checkIfDigits(listBoxMyReports.SelectedValue.ToString());
            var selectedReport = reportHandler.GetSingleReport(rid);

            lblStatus.Content = selectedReport.Status;
            if (selectedReport.Status == "Nekad")
            {
                lblMotivation.Text = selectedReport.Description;
            }
        }

        private void btnLoadToDraft_Click(object sender, RoutedEventArgs e)
        {
            SeeReportInfoWIndow reportSumWindow = new SeeReportInfoWIndow();
            reportSumWindow.InitializeComponent();
            
            var rid = logic.checkIfDigits(listBoxMyReports.SelectedValue.ToString());
            var selectedReport = reportHandler.GetSingleReport(rid);
            var travelinfos = reportHandler.GetTravelInfoForSpecificReport(rid);
            var user = reportHandler.GetSingleUser(selectedReport.UID);
            var receipts = reportHandler.GetReceiptsForSpecificReport(selectedReport.RID);

            var listOftravelinfos = new List<String>();
            var listOfReceipts = new List<String>();
            foreach (var travel in travelinfos)
            {
                var visitedcountry = logic.getSingleCountry(travel.CID);
                listOftravelinfos.Add("Reste i " + visitedcountry.Name + " mellan " +
                                      travel.StartDate.Value.ToShortDateString() + " - " +
                                      travel.EndDate.Value.ToShortDateString() + " och var ledig " +
                                      travel.VacationDays + " dagar.");
            }
            var infoOnTravels = string.Join("\n", listOftravelinfos.ToArray());

            foreach (var receipt in receipts)
            {
                var savedReceipts = reportHandler.GetSingleReceipt(receipt.RID);
                listOfReceipts.Add("Kvitto: " + savedReceipts.TravelReciept + " Kostnad: " +
                                   savedReceipts.RecieptAmount);
            }
            var infoOnReceipts = string.Join("\n", listOfReceipts.ToArray());

            reportSumWindow.lblNameofReportCreator.Content = user.FirstName + " " + user.LastName;
            reportSumWindow.lblReportCreatedDate.Content = selectedReport.ReportDate.Value.ToShortDateString();
            reportSumWindow.lblTotalAmountSpent.Content = selectedReport.TotalAmount;
            reportSumWindow.tbDescription.Text = selectedReport.Description;
            reportSumWindow.tbInfoVisitedCountries.Text = infoOnTravels;
            reportSumWindow.tbInfoReceipts.Text = infoOnReceipts;
            reportSumWindow.lblKilometersDriven.Content = selectedReport.Kilometers.ToString();
            reportSumWindow.lblStatusOnReport.Content = selectedReport.Status;


            reportSumWindow.lblStatusText.Visibility = Visibility.Hidden;
            reportSumWindow.lblStatusOnReport.Visibility = Visibility.Hidden;
            reportSumWindow.btnApproveReport.Visibility = Visibility.Hidden;
            reportSumWindow.btnDenyReport.Visibility = Visibility.Hidden;
            reportSumWindow.Show();

            Hide();
        }
    }
}
