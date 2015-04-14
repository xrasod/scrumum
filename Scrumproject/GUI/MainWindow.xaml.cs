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
    using Scrumproject.Data;
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
        LogicHandler addUserHandler = new LogicHandler();
        Notes notesSaving = new Notes();
        Notes notesLoading = new Notes();

        


        public MainWindow()
        {
            InitializeComponent();
            PopulateCurrencyData();
            PopulateListViewUsers();

            tbUserID.IsEnabled = false;
            tbUsername.IsEnabled = false;
            tbBoss.IsEnabled = false;
            

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
        //Fyllar listview med användare
        private void PopulateListViewUsers()
        {
            var users = BossRepository.GetAll();

            foreach (var user in users)
            {
                lvUsers.Items.Add(user.Username);
            }
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
        //Lägger till användare
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Validator validera = new Validator();
            
            //var BID = Int32.Parse(lbLoggedInUser.Content.ToString());
            var email = tbEmail.Text;
            var firstName = tbFirstName.Text;
            var lastName = tbLastNamne.Text;
            var pw = tbPassword.Text;
            var SSN = tbSsn.Text;

            if (validera.ControllFiledNotEmpty(tbEmail))
            {
                MessageBox.Show("Du måste ange en e-post!");
            }
            else if (validera.ControllFiledNotEmpty(tbFirstName))
            {
                MessageBox.Show("Du måste ange ett förnamn!");
            }
            else if (validera.ControllFiledNotEmpty(tbLastNamne))
            {
                MessageBox.Show("Du måste ange ett efternamn!");
            }
            else if (validera.ControllFiledNotEmpty(tbPassword))
            {
                MessageBox.Show("Du måste ange ett lösenord!");
            }
            else if (validera.ControllFiledNotEmpty(tbSsn))
            {
                MessageBox.Show("Du måste ange ett personnummer!");
            }
            else
            {
                addUserHandler.registeruser(firstName, lastName, email, pw, 1, SSN);
                MessageBox.Show(tbFirstName.Text + " " + tbLastNamne.Text + " är nu tillagd!");
                tbUsername.IsEnabled = true;
                tbBoss.IsEnabled = true;
                tbUserID.IsEnabled = true;
                tbPassword.Clear();
                tbFirstName.Clear();
                tbLastNamne.Clear();
                tbEmail.Clear();
                tbSsn.Clear();
            }

            

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

       
        //Uppdatera Användare
        private void btnUpdateInfo_Click(object sender, RoutedEventArgs e)
        {
            LogicHandler updateUserHandler = new LogicHandler();
            Validator validate = new Validator();
            var bossID = tbBoss.Text;
            var userID = tbUserID.Text;

            var id = Int32.Parse(userID);
            var username = tbUsername.Text;
            var firstname = tbFirstName.Text;
            var lastname = tbLastNamne.Text;
            var password = tbPassword.Text;
            var boss = Int32.Parse(bossID);
            var ssn = tbSsn.Text;
            var mail = tbEmail.Text;

            if (validate.ControllFiledNotEmpty(tbUsername))
            {
                MessageBox.Show("Du måste ange ett användarnamn!");
            }
            else if (validate.ControllFiledNotEmpty(tbFirstName))
            {
                MessageBox.Show("Du måste ange ett förnamn!");
            }
            else if (validate.ControllFiledNotEmpty(tbLastNamne))
            {
                MessageBox.Show("Du måste ange ett efternamn!");
            }
            else if (validate.ControllFiledNotEmpty(tbPassword))
            {
                MessageBox.Show("Du måste ange ett lösenord!");
            }
            else if (validate.ControllFiledNotEmpty(tbSsn))
            {
                MessageBox.Show("Du måste ange ett personnummer!");
            }
            else if (validate.ControllFiledNotEmpty(tbEmail))
            {
                MessageBox.Show("Du måste ange en email-adress!");
            }
            else if (validate.ControllFiledNotEmpty(tbBoss))
            {
                MessageBox.Show("Du måste ange vem som är chef för användaren!");
            }
            else if (validate.ControllFiledNotEmpty(tbUserID))
            {
                MessageBox.Show("Användaren måste ha ett anställnigsnummer!");
            }
            
            else
            {
                updateUserHandler.uppdateUser(id, username, firstname, lastname, password, ssn, mail, boss);
                MessageBox.Show(tbFirstName.Text + " " + tbLastNamne.Text + " har uppdaterats!");
            }
        }
        //Fyller i TB's med en användare man valt att uppdatera ur Listan
        private void lvUsers_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            var selected = lvUsers.SelectedValue.ToString();
            var users = BossRepository.GetAll();

            foreach (var user in users)
            {
                if (selected == user.Username)
                {
                    tbFirstName.Text = user.FirstName;
                    tbLastNamne.Text = user.LastName;
                    tbEmail.Text = user.Email;
                    tbPassword.Text = user.PW;
                    tbUsername.Text = user.Username;
                    tbSsn.Text = user.SSN;
                    tbBoss.Text = user.BID.ToString();
                    tbUserID.Text = user.UID.ToString();

                }

            }
        }
     

      


        

    }
}
