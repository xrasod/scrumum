﻿    using System;
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
    using System.Windows.Shell;
    using iTextSharp.text.pdf;
    using Scrum.Data;
using Scrum.Data.Data;
    using Scrumproject.Data;
    using Scrumproject.Logic;
using Scrumproject.Logic.Entities;
using Scrumproject.GUI;


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
        LogicHandler localHandeler = new LogicHandler();
        Notes notesSaving = new Notes();
        Notes notesLoading = new Notes();
        LogicHandler localHandeler = new LogicHandler();

        internal static MainWindow main;
        internal string Status
        {
            get { return lbLoggedInAsThisUser.Content.ToString(); }
            set { Dispatcher.Invoke(new Action(() => { lbLoggedInAsThisUser.Content = value; })); }
        }
        internal string BossStatus
        {
            get { return lbLoggedInUser.Content.ToString(); }
            set { Dispatcher.Invoke(new Action(() => { lbLoggedInUser.Content = value; })); }
        }

        public MainWindow()
        {
            InitializeComponent();
            PopulateCurrencyData();
            PopulateListViewUsers();
            PupulateListViewCountries();
            localHandeler.SaveCountriesfromDBtoXML();


            TbTotalKm.IsReadOnly = true;
            tbUserID.IsEnabled = false;
            tbUsername.IsEnabled = false;
            tbBoss.IsEnabled = false;
            notesLoading = notesHandler.LoadNotes("Notes.xml");
            tbNotes.Text = notesLoading.Note;
            var rep = new CountriesRepository();

            main = this;

            var hej = rep.GetAllCountries();


            foreach(var x in hej)
            {
                CbCountries.Items.Add(x.Name);       
            }

            try
            {
                _reportDraftLoading = reportHandler.LoadDraft("DraftReport.xml");
                TbTotalKm.Text = _reportDraftLoading.NumberOfKilometersDrivenInTotal.ToString();
                TbCarTripLengthKm.Text = _reportDraftLoading.KilometersDriven.ToString();
                tbDoneOnTrip.Text = _reportDraftLoading.Description;
                dpStartDate.Text = _reportDraftLoading.StartDate;
                dpEndDate.Text = _reportDraftLoading.EndDate;
                foreach (var kvitto in _reportDraftLoading.imagePathsList)
                {
                    listBoxReceipts.Items.Add(kvitto);
                }
                foreach (var dayinfo in _reportDraftLoading.daysSpentInCountry)
                {
                    listBoxDays.Items.Add(dayinfo);
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
                              "Antal körda kilometer totalt: " + TbCarTripLengthKm.Text + "\n \n \n" +
                              "Sparade kvitton: " ;

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

            logic.changeStatus(listBoxUsers.SelectedValue.ToString());
        
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
                listBoxUsers.Items.Clear();
                PopulateListViewUsers();
            }

        }

        private void CbCountries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
            CurrencyConverter c = new CurrencyConverter();
            var selectedCountry = CbCountries.SelectedItem.ToString();
            var content = c.GetSelectedCountrySpecifics(selectedCountry);
            LbTraktamente.Content = content.Subsistence;
        }
            catch { }
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
            listBoxReceipts.Items.Add(tbReceiptFile.Text);
        }

        private void btnRemoveSelectedReceipt_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = listBoxReceipts.SelectedItem;
            listBoxReceipts.Items.Remove(selectedItem);
        }

        public void saveDraft()
        {
            _reportDraftSaving.Description = tbDoneOnTrip.Text;
            _reportDraftSaving.NumberOfKilometersDrivenInTotal = Int32.Parse(TbTotalKm.Text);
            _reportDraftSaving.KilometersDriven = Int32.Parse(TbCarTripLengthKm.Text);
            _reportDraftSaving.imagePathsList = listBoxReceipts.Items.Cast<String>().ToList();
            _reportDraftSaving.StartDate = dpStartDate.Text;
            _reportDraftSaving.EndDate = dpEndDate.Text;
            _reportDraftSaving.daysSpentInCountry = listBoxDays.Items.Cast<String>().ToList();
            reportHandler.SaveDraft(_reportDraftSaving, "DraftReport.xml");
            tbDoneOnTrip.Text = "";
        }

        private void btnUpdateTotalDriven_Click(object sender, RoutedEventArgs e)
        {
            try
            {
            int carTripLength = Convert.ToInt32(TbCarTripLengthKm.Text);
            int updatedCarTripLength = Convert.ToInt32(TbTotalKm.Text);

            int totalKmDriven = carTripLength + updatedCarTripLength;
            TbTotalKm.Text = totalKmDriven.ToString();
            TbTotalKm.IsReadOnly = true;
            }
            catch
            {
                MessageBox.Show("Du måste ange antal kilometer.");
            }
           
        }

        private void btnSaveDraft_Click(object sender, RoutedEventArgs e)
        {

        }

        public void updateDays()
        {
            listBoxDays.Items.Clear();
            var dateHandler = new DateHandler();
            
            var setDate = dateHandler.GetDays(dpStartDate.SelectedDate.Value, dpEndDate.SelectedDate.Value);
            //if (setDate.Count == 0)
            //{
            //    MessageBox.Show("Du kan inte resa mindre dagar än dagar du är ledig eller välja senare start- än slutdatum.");
            //}
            //else
            //{
            foreach (var item in setDate)
            {
                var hej = String.Format(item.Day.ToString() + "/" + item.Month.ToString() + "/" + item.Year.ToString());
                listBoxDays.Items.Add(hej);
            }
            //}
            }


        private void btnUpdateList_Click(object sender, RoutedEventArgs e)
        {
            updateDays();
        }

        private void Window_Closing_1(object sender, System.ComponentModel.CancelEventArgs e)
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

        //Fyllar listview med användare
        private void PopulateListViewUsers()
        {
            var logic = new LogicHandler();
            var users = logic.getInfoOnSelectedUser();

            foreach (var user in users)
            {
                listBoxUsers.Items.Add("Anst nr: " + user.UID + " " + user.FirstName + " " + user.LastName);
            }
        }
        
        
        //Fyller listViewn med ländernas namn
        private void PupulateListViewCountries()
        {
            var logic = new LogicHandler();
            var count = logic.getInfoOnSelectedCountry();

            foreach (var country in count)
            {
                lvCountriesEdit.Items.Add(country.Name);
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
                listBoxUsers.SelectedValue = tbUsername.Text;
                listBoxUsers.Items.Clear();
                PopulateListViewUsers();
            }
        }



        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {

                }


        
        
        private void BtnLogIn_Click(object sender, RoutedEventArgs e)
        {
            var source = 2;
            LoginWindow l = new LoginWindow(source);
            l.Show();
    }
        
        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            lbLoggedInAsThisUser.Content = "";
            BtnLogIn.Visibility = Visibility.Visible;
            btnLogOut.Visibility = Visibility.Hidden;
}
        
        private void btnLogOutChef_Click(object sender, RoutedEventArgs e)
        {
            lbLoggedInUser.Content = "";
            btnLogInChef.Visibility = Visibility.Visible;
            btnLogOutChef.Visibility = Visibility.Hidden;
        }
        
        
        
        private void btnSaveStartCountry_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dayinfo = listBoxDays.SelectedItem.ToString() + " - " + CbCountries.SelectedItem.ToString() + " - " +
                              LbTraktamente.Content.ToString() + " kr";
                listBoxDays.Items[listBoxDays.SelectedIndex] = dayinfo;
            }
            catch
            {
                MessageBox.Show("Du måste välja en dag samt fylla i information att spara.");
            }
        }
        
        private void btnDeleteCountry_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var firstSpace = listBoxDays.SelectedValue.ToString().IndexOf(' ');
                listBoxDays.Items[listBoxDays.SelectedIndex] = listBoxDays.SelectedValue.ToString()
                    .Substring(0, firstSpace);
            }
            catch
            {
                MessageBox.Show("Du måste välja en dag att ta bort.");
            }
        }
        
        private void btnLogInChef_Click(object sender, RoutedEventArgs e)
        {
            var source = 1;
            LoginWindow l = new LoginWindow(source);
            l.Show();
        }
        
        //Fyller i TB's med en användare man valt att uppdatera ur Listan
        private void listBoxUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tbUserID.IsEnabled = true;
            tbUsername.IsEnabled = true;
            tbBoss.IsEnabled = true;
            var logic =  new LogicHandler();
            var selected = listBoxUsers.SelectedValue.ToString();
            var u = logic.getInfoOnSelectedUser();
        
            var val = logic.checkIfDigits(selected);
        
        
            foreach (var user in u.Where(user => val.Equals(user.UID)))
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
        //Uppdaterar land
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var currname = lvCountriesEdit.SelectedValue.ToString();
            var name = tbCountryName.Text;
            var curr = tbCurrency.Text;
            var sub = Int32.Parse(tbMaxCash.Text);
            var logic = new LogicHandler();
        
        
            logic.uppdateCountry(currname, name, curr, sub);
            lvCountriesEdit.Items.Clear();
            PupulateListViewCountries();
        }
        
        //Fyller Tbs med text av landet man valt.
        
        private void lvCountriesEdit_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            var logic = new LogicHandler();
        
            var selected = lvCountriesEdit.SelectedValue.ToString();
            var count = logic.getInfoOnSelectedCountry();

            foreach (var countries in count)
            {
                if (selected == countries.Name)
                {
                    tbCountryName.Text = countries.Name;
                    tbMaxCash.Text = countries.Subsistence.ToString();
                    tbCurrency.Text = countries.Currency;
                }
            }

          
        
        }
        //Lägg till land
        private void btnAddCountry_Click_1(object sender, RoutedEventArgs e)
        {
                  
            var name = tbCountryName.Text;
            var curr = tbCurrency.Text;
            var sub = Int32.Parse(tbMaxCash.Text);
            var logic = new LogicHandler();
            if (lvCountriesEdit.Items.Contains(name))
            {
                MessageBox.Show("Detta land finns redan!");

            }
            else
            {
                logic.AddNewCountry(name, curr, sub);
                lvCountriesEdit.Items.Clear();
                PupulateListViewCountries();
                
            }
            
        }

        //Söka användare
        private void btnSearchUser1_Click(object sender, RoutedEventArgs e)
        {
            var search = tbSearchUser.Text.ToLower();
            var logic = new LogicHandler();
            var allUsers = logic.getInfoOnSelectedUser();
            
            listBoxUsers.Items.Clear();
            
            var u = (from m in allUsers where m.FirstName.ToLower().Contains(search) select m);

            foreach (var item in u)
        {
                

                listBoxUsers.Items.Add("Anst nr: " + item.UID + " " + item.FirstName + " " + item.LastName);
}

        }
           
        

        private void btnSaveStartCountry_Click(object sender, RoutedEventArgs e)
        {
            LogicHandler l = new LogicHandler();

            var dayinfo = "";
            var breakfast = CHBBreakfast.IsChecked.Value;
            var lunch = CHBLunch.IsChecked.Value;
            var dinner = CHBDinner.IsChecked.Value;
            double subsistenceDouble = Convert.ToDouble(LbTraktamente.Content.ToString());

            var subsistence = l.CalculateSubsistenceDeduction(breakfast, lunch, dinner, subsistenceDouble);

            try
            {
                if (CHBVacationday.IsChecked == true)
                {
                    dayinfo = listBoxDays.SelectedItem.ToString() + " - " + CbCountries.SelectedItem.ToString() + " - " +
                    " LEDIG";
                    listBoxDays.Items[listBoxDays.SelectedIndex] = dayinfo;
                }
                else
                {
                    dayinfo = listBoxDays.SelectedItem.ToString() + " - " + CbCountries.SelectedItem.ToString() + " - " +
                                  subsistence + " kr";
                listBoxDays.Items[listBoxDays.SelectedIndex] = dayinfo;
            }
            }
            catch
            {
                MessageBox.Show("Du måste välja en dag samt fylla i information att spara.");
            }
        }

            }
        }

        private void btnLogInChef_Click(object sender, RoutedEventArgs e)
        {
            var source = 1;
            LoginWindow l = new LoginWindow(source);
            l.Show();
        }

        private void btnDeleteDraft_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Vill du verkligen radera allt?", "Radera utkast", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                var today = DateTime.Now.ToString();
                var tomorrow = DateTime.Now.AddDays(1).ToString();
                tbDoneOnTrip.Text = "";
                listBoxReceipts.Items.Clear();
                listBoxDays.Items.Clear();
                TbTotalKm.Text = "0";
                TbCarTripLengthKm.Text = "0";
                dpStartDate.Text = today;
                dpEndDate.Text = tomorrow;
                tbReceiptFile.Text = "";
                tbSum.Text = "";
                LbTraktamente.Content = "";
                CbCountries.SelectedIndex = -1;

                saveDraft();
                MessageBox.Show("Utkast raderat");
            }
            else
            {
                MessageBox.Show("Vilken tur att jag frågade");
            }
        }

       
}
    }

