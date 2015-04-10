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
using Scrum.Data.Data;
using Scrumproject.Logic;
using Scrumproject.Logic.Entities;

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
        Notes notesSaving = new Notes();
        Notes notesLoading = new Notes();


        public MainWindow()
        {
            InitializeComponent();


            var rep = new CountriesRepository();

            var hej = rep.GetAllCountries();


            foreach(var x in hej)
            {
                CbCountries.Items.Add(x.Name);
            }
            
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

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnSaveNotes_Click(object sender, RoutedEventArgs e)
        {
            notesSaving.Note = tbNotes.Text;
            notesHandler.SaveNotes(notesSaving, "Notes.xml");
            tbNotes.Text = "Dina anteckningar är sparade!";
            btnSaveNotes.Visibility = Visibility.Hidden;
        }

        private void btnLoadNotes_Click(object sender, RoutedEventArgs e)
        {
            tbNotes.Text = "";
            notesLoading = notesHandler.LoadNotes("Notes.xml");
            tbNotes.Text = notesLoading.Note;
            btnSaveNotes.Visibility = Visibility.Visible;
            btnLoadNotes.Visibility = Visibility.Hidden;
        }




        

    }
}
