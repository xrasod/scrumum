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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Scrumproject.Logic;

namespace Scrumproject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Report reportSaving = new Report();
        Report reportLoading = new Report();
        LogicHandler reportHandler = new LogicHandler();
        LogicHandler notesHandler = new LogicHandler();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnCreateDraft_Click(object sender, RoutedEventArgs e)
        {
            reportSaving.Description = tbNotes.Text;
            reportSaving.Id = 1;
            reportSaving.NumberOfKilometersDriven = 111;
            reportSaving.UserId = 12;
            reportSaving.Status = 1;
            reportHandler.SaveDraft(reportSaving, "DraftReport.xml");
            tbNotes.Text = "";
        }

        private void btnLoadDraft_Click(object sender, RoutedEventArgs e)
        {
            reportLoading = reportHandler.LoadDraft("DraftReport.xml");
            lbCarTripLengthKm.Text = reportLoading.NumberOfKilometersDriven.ToString();
            tbNotes.Text = reportLoading.Description;
        }

        private void btnSendReport_Click(object sender, RoutedEventArgs e)
        {

        }




        

    }
}
