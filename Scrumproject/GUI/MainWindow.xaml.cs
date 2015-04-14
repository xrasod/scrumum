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
        ReportDraft _reportDraftSaving = new ReportDraft();
        ReportDraft _reportDraftLoading = new ReportDraft();
        LogicHandler reportHandler = new LogicHandler();
        LogicHandler notesHandler = new LogicHandler();
        LogicHandler addUserHandler = new LogicHandler();
        LogicHandler pdfHandler = new LogicHandler();
        Notes notesSaving = new Notes();
        Notes notesLoading = new Notes();


        public MainWindow()
        {
            InitializeComponent();
            PopulateCurrencyData();

            var rep = new CountriesRepository();

            var hej = rep.GetAllCountries();


            foreach(var x in hej)
            {
                CbCountries.Items.Add(x.Name);       
            }

            try
            {
                _reportDraftLoading = reportHandler.LoadDraft("DraftReport.xml");
                lbCarTripLengthKm.Text = _reportDraftLoading.NumberOfKilometersDriven.ToString();
                tbDoneOnTrip.Text = _reportDraftLoading.Description;
                foreach (var kvitto in _reportDraftLoading.imagePathsList)
                {
                    LvReceipts.Items.Add(kvitto);
                }
                
            }
            catch
            {
                MessageBox.Show("Det finns inget sparat utkast att ladda");
            }


        }

        private void btnSendReport_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Vill du även spara rapporten som pdf?", "Spara som pdf", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                var pdfinfo = "Vad har gjorts under resan: " + tbDoneOnTrip.Text + "\n \n \n" +
                              "Antal körda kilometer totalt: " + lbCarTripLengthKm.Text + "\n \n \n";

                pdfHandler.CreatePdf(pdfinfo, DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")+".pdf");
                MessageBox.Show("Din rapport har sparats.");
                
            }
        }       

        private void btnSaveNotes_Click(object sender, RoutedEventArgs e)
        {
            notesSaving.Note = tbNotes.Text;
            notesHandler.SaveNotes(notesSaving, "Notes.xml");
            tbNotes.Text = "Dina anteckningar är sparade!";
            btnSaveNotes.Visibility = Visibility.Hidden;
            btnLoadNotes.Visibility = Visibility.Visible;
        }

        private void btnLoadNotes_Click(object sender, RoutedEventArgs e)
        {
            tbNotes.Text = "";
            notesLoading = notesHandler.LoadNotes("Notes.xml");
            tbNotes.Text = notesLoading.Note;
            btnSaveNotes.Visibility = Visibility.Visible;
            btnLoadNotes.Visibility = Visibility.Hidden;
        }

        private void PopulateCurrencyData()
        {
            var logic = new CurrencyConverter();

            var hej = logic.GetCountries();

            foreach (var x in hej)
            {
                CbFromCurrency.Items.Add(x.Name);
                CbToCurrency.Items.Add(x.Name);
            }
            CbFromCurrency.SelectedIndex = 0;
            CbToCurrency.SelectedIndex = 0;
        }

        private void BtnConvert_Click(object sender, RoutedEventArgs e)
        {
            string fromCurrency= lbFromCurrency.Content.ToString();
            string toCurrency = lbToCurrency.Content.ToString();
            string amount = TbFromCurrency.Text.ToString();
            if (Validator.ControlInputConverter(amount))
            {
            double amounten = Convert.ToDouble(amount);
            CurrencyConverter c = new CurrencyConverter();
            
                string hej = c.ConvertCurrency(fromCurrency, toCurrency, amounten);
                TbToCurrency.Text = hej;
            }
            else
            {
                TbToCurrency.Text = "failed to convert";
            }
        }

        private void CbFromCurrency_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrencyConverter c = new CurrencyConverter();

            var selectedCountry = CbFromCurrency.SelectedItem.ToString();

            var content = c.GetSelectedCountrySpecifics(selectedCountry);

            lbFromCurrency.Content = content.Currency;
        }

        private void CbToCurrency_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrencyConverter c = new CurrencyConverter();

            var selectedCountry = CbToCurrency.SelectedItem.ToString();

            var content = c.GetSelectedCountrySpecifics(selectedCountry);

            lbToCurrency.Content = content.Currency;
        }

        private void btnInactive_Click(object sender, RoutedEventArgs e)
        {
            
            var logic = new LogicHandler();

            logic.changeStatus(lvUsers.SelectedValue.ToString());
        
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //var BID = Int32.Parse(lbLoggedInUser.Content.ToString());
            var email = tbEmail.Text;
            var firstName = tbFirstName.Text;
            var lastName = tbLastNamne.Text;
            var pw = tbPassword.Text;
            var SSN = tbSsn.Text;

            addUserHandler.registeruser(firstName, lastName, email, pw, 1, SSN);
        }

        private void CbCountries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrencyConverter c = new CurrencyConverter();
            var selectedCountry = CbCountries.SelectedItem.ToString();

            var content = c.GetSelectedCountrySpecifics(selectedCountry);

            LbTraktamente.Content = content.Subsistence;
        }

        private void btnUploadReceipt_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();



            // Set filter for file extension and default file extension
            dlg.DefaultExt = ".png";
            dlg.Filter =
                "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";


            // Display OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = dlg.ShowDialog();


            // Get the selected file name and display in a TextBox
            if (result == true)
            {
                // Open document
                string filename = dlg.FileName;
                tbReceiptFile.Text = filename;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            LvReceipts.Items.Add(tbReceiptFile.Text);
        }

        private void btnRemoveSelectedReceipt_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = LvReceipts.SelectedItem;
            LvReceipts.Items.Remove(selectedItem);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Vill du spara ett utkast av din resa som laddas vid nästa körning?", "Utkast", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                saveDraft();
                MessageBox.Show("Utkast sparat.");
                e.Cancel = false;
            }
            else
            {
                MessageBoxResult res = MessageBox.Show("Är du verkligen säker? Du måste fylla i allt igen annars", "Säker", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (res == MessageBoxResult.Yes)
                {
                    MessageBox.Show("Skyll dig själv. Allt du fyllde i är nu raderat för alltid. Farväl.");
                    e.Cancel = false;
                }
                else
                {
                    saveDraft();
                    MessageBox.Show("Bra val min vän. Ditt utkast har nu sparats. Puss och kram.");
                    e.Cancel = false;
                }

            }
        }

        public void saveDraft()
        {
            _reportDraftSaving.Description = tbDoneOnTrip.Text;
            _reportDraftSaving.NumberOfKilometersDriven = 111;
            _reportDraftSaving.imagePathsList = LvReceipts.Items.Cast<String>().ToList();
            reportHandler.SaveDraft(_reportDraftSaving, "DraftReport.xml");
            tbDoneOnTrip.Text = "";
        }

    }
}
