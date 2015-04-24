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
        Notes notesSaving = new Notes();
        Notes notesLoading = new Notes();
        LogicHandler localHandeler = new LogicHandler();
        XmlReader Xmlreader = new XmlReader();
        ReportHandler reportDanger = new ReportHandler();
        StatisticsHandler statisticsHandler = new StatisticsHandler();
        List<DayHandler> dayhandler = new List<DayHandler>();
        PrepaymentHandler prepaymentHandler = new PrepaymentHandler();
        SortHandler sortHandler = new SortHandler();
        List<RecieptHandler> recieptInfo = new List<RecieptHandler>(); 
        Validator validera = new Validator();
        

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
            getcountriesfromXML();
            PopulateStatisticsWindowCountryCB();
            fillCbOfStatuses();
            

            TbTotalKm.IsReadOnly = true;
            notesLoading = notesHandler.LoadNotes("Notes.xml");
            tbNotes.Text = notesLoading.Note;
            var rep = new CountriesRepository();

            main = this;

          


         

            try
            {
                _reportDraftLoading = reportHandler.LoadDraft("DraftReport.xml");
                TbTotalKm.Text = _reportDraftLoading.NumberOfKilometersDrivenInTotal.ToString();
                TbCarTripLengthKm.Text = _reportDraftLoading.KilometersDriven.ToString();
                tbDoneOnTrip.Text = _reportDraftLoading.Description;
                dpStartDate.Text = _reportDraftLoading.StartDate;
                dpEndDate.Text = _reportDraftLoading.EndDate;
                TbDaysOff.Text = _reportDraftLoading.DaysOff;
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
            string[] arr = new string[3];
            List<DayHandler> d = new List<DayHandler>();
            RecieptHandler rh = new RecieptHandler();
            List<string> list = new List<string>();
            var j = listBoxDays.Items;

            for (int i = 0; listBoxDays.Items.Count > i; i++)
            {
                j.MoveCurrentToNext();
                list.Add(j.CurrentItem.ToString());
            }

            foreach (var h in list)
            {
                DayHandler dh = new DayHandler();
                h.Trim();
                string replaced = h.Replace("kr", "");
                string[] words = replaced.Split('-');
                dh.date = words[0].Trim();
                dh.country = words[1].Trim();
                var o = words[2];
                double dou = Convert.ToDouble(o);
                dh.subsistence = dou;
                d.Add(dh);
            }
  

            //List<string> recieptList = new List<string>();
            //for (int i = 0; listBoxReceipts.Items.Count > i; i++)
            //{
            //    recieptList.Add(listBoxReceipts.Items[i].ToString());
            //}
            DayHandler day = new DayHandler();
            var totalkm = TbTotalKm.Text;
            var totalreciept = tbTotalRecieptAmount.Text;
            var totalrecieptdec = Convert.ToDouble(totalreciept);
            double totalkmdec = Convert.ToDouble(totalkm);
            decimal totalkmdecimal = Convert.ToDecimal(totalkmdec);
            var totalAmount = day.CalculateTotalAmount(dayhandler, totalkmdec, totalrecieptdec);
            day.StoreReport(dayhandler, totalkmdecimal, tbDoneOnTrip.Text.ToString(), totalAmount, lbLoggedInAsThisUser.Content.ToString(), recieptInfo);

            try
            {
                List<string> savedReceipts = new List<string>();
                List<string> visitedCountries = new List<string>();
                foreach (var receipt in listBoxReceipts.Items)
                {
                    savedReceipts.Add(receipt.ToString());
                }
                foreach (var country in listBoxDays.Items)
                {
                    visitedCountries.Add("Dag " + country.ToString() + " maxtraktamente.");
                }
                var receiptsinfo = string.Join("\n", savedReceipts.ToArray());
                var countryinfo = string.Join("\n", visitedCountries.ToArray());
                var chef = pdfHandler.GetUsersBoss(lbLoggedInAsThisUser.Content.ToString());
                var user = pdfHandler.GetFullNameFromTheUserName(lbLoggedInAsThisUser.Content.ToString());
                var result = MessageBox.Show("Vill du även spara rapporten som pdf?", "Spara som pdf", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    var pdfinfo = "Ansökande: " + user + "\n" + "Chef: " + chef + "\n" +
                                  "Resperiod: " + dpStartDate.Text + " - " + dpEndDate.Text + "\n" +
                                  "Antal lediga dagar: " + TbDaysOff.Text + "\n" +
                                  "Antal körda kilometer totalt: " + TbCarTripLengthKm.Text + "\n\n" +
                                  "Besökta länder " + "\n" + countryinfo + "\n\n" +
                                  "Sparade kvitton " + "\n" + receiptsinfo + "\n\n" +
                                  "Resebeskrivning " + "\n" + tbDoneOnTrip.Text + "\n\n";

                    pdfHandler.CreatePdf(pdfinfo, DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".pdf");
                    MessageBox.Show("Din rapport har sparats.");

                }
            }
            catch { MessageBox.Show("Du måste logga in för att kunna skicka din ansökan."); }
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
            try
            {
                string fromCurrency = lbFromCurrency.Content.ToString();
                string toCurrency = lbToCurrency.Content.ToString();
                string amount = TbFromCurrency.Text.ToString();
                DateTime date = DPdate.SelectedDate.Value;


                if (Validator.ControlInputConverter(amount))
                {
                    double amounten = Convert.ToDouble(amount);
                    CurrencyConverter c = new CurrencyConverter();

                    string hej = c.ConvertCurrency(fromCurrency, toCurrency, amounten, date);
                    TbToCurrency.Text = hej;
                }
                else
                {
                    TbToCurrency.Text = "failed to convert";
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show("Du måste välja valuta att konvertera!");
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
            try
            {
                var logic = new LogicHandler();

                logic.changeStatus(listBoxUsers.SelectedValue.ToString());
            }
            catch (Exception ee)
            {
                MessageBox.Show("Du måste välja en anställd!");
            }
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
            try
            {

                if (validera.IsEmailValid(email))
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
                else if (validera.IsSsnValid(SSN))
                {
                    MessageBox.Show("Du måste ange ett personnummer!");
                }
                else
                {
                    addUserHandler.registeruser(firstName, lastName, email, pw, 1, SSN);
                    MessageBox.Show(tbFirstName.Text + " " + tbLastNamne.Text + " är nu tillagd!");
                    tbPassword.Clear();
                    tbFirstName.Clear();
                    tbLastNamne.Clear();
                    tbEmail.Clear();
                    tbSsn.Clear();
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
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
                "JPG Files (*.jpg)|*.jpeg|PNG Files (*.png)|*.png|JPEG Files (*.jpeg)|*.jpg|GIF Files (*.gif)|*.gif";


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
            RecieptHandler rh = new RecieptHandler();

            try
            {
                double reciept = Convert.ToDouble(tbSum.Text);
                double TotalReciept = Convert.ToDouble(tbTotalRecieptAmount.Text);

                double totalRecieptAmount = reciept + TotalReciept;
                tbTotalRecieptAmount.Text = totalRecieptAmount.ToString();
                tbTotalRecieptAmount.IsReadOnly = true;
                var tbSumma = tbSum.Text;
                var tbSummaDecimal = Convert.ToDecimal(tbSumma);
                rh.RecieptAmount = tbSummaDecimal;
                rh.TravelReciept = tbReceiptFile.Text;
                recieptInfo.Add(rh);
            }
            catch
            {
                MessageBox.Show("Ange summa!");
            }
        }

        private void btnRemoveSelectedReceipt_Click(object sender, RoutedEventArgs e)
        {

                var selectedItem = listBoxReceipts.SelectedItem;
                listBoxReceipts.Items.Remove(selectedItem);


        }

        public void saveDraft()
        {
            try
            {
                _reportDraftSaving.Description = tbDoneOnTrip.Text;
                _reportDraftSaving.NumberOfKilometersDrivenInTotal = Int32.Parse(TbTotalKm.Text);
                _reportDraftSaving.KilometersDriven = Int32.Parse(TbTotalKm.Text);
                _reportDraftSaving.imagePathsList = listBoxReceipts.Items.Cast<String>().ToList();
                _reportDraftSaving.StartDate = dpStartDate.Text;
                _reportDraftSaving.EndDate = dpEndDate.Text;
                _reportDraftSaving.daysSpentInCountry = listBoxDays.Items.Cast<String>().ToList();
                _reportDraftSaving.DaysOff = TbDaysOff.Text;
                reportHandler.SaveDraft(_reportDraftSaving, "DraftReport.xml");
                tbDoneOnTrip.Text = "";
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }
        //Ska valideras
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
            try
            {
                listBoxDays.Items.Clear();
                var dateHandler = new DateHandler();

                var setDate = dateHandler.GetDays(dpStartDate.SelectedDate.Value, dpEndDate.SelectedDate.Value);

                foreach (var item in setDate)
                {
                    var hej =
                        String.Format(item.Year.ToString() + "/" + item.Month.ToString() + "/" + item.Day.ToString());
                    listBoxDays.Items.Add(hej);
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }

        }


        private void btnUpdateList_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                updateDays();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
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

 
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }

        
        //Fyller Tbs med text av landet man valt.
        private void lvCountriesEdit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var selected = lvCountriesEdit.SelectedValue.ToString();
                var allCountries = localHandeler.getAllCountriesToList();


                foreach (var countries in allCountries)
                {
                    if (selected == countries.Name)
                    {
                        tbCountryName.Text = countries.Name;
                        tbMaxCash.Text = countries.Subsistence.ToString();
                        tbCurrency.Text = countries.Currency;
                    }
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        private void btnAddCountry_Click(object sender, RoutedEventArgs e)
        {
            try
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
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                var currname = lvCountriesEdit.SelectedValue.ToString();
                var name = tbCountryName.Text;
                var curr = tbCurrency.Text;
                var sub = Int32.Parse(tbMaxCash.Text);
                var logic = new LogicHandler();


                logic.uppdateCountry(currname, name, curr, sub);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }


        }
        private void BtnLogIn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var source = 2;
                LoginWindow l = new LoginWindow(source);
                l.Show();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lbLoggedInAsThisUser.Content = "";
                BtnLogIn.Visibility = Visibility.Visible;
                btnLogOut.Visibility = Visibility.Hidden;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        private void btnLogOutChef_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lbLoggedInUser.Content = "";
                btnLogInChef.Visibility = Visibility.Visible;
                btnLogOutChef.Visibility = Visibility.Hidden;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }



        private void btnSaveStartCountry_Click(object sender, RoutedEventArgs e)
        {
            LogicHandler l = new LogicHandler();
            DayHandler d = new DayHandler();

            var dayinfo = "";
            var breakfast = CHBBreakfast.IsChecked.Value;
            var lunch = CHBLunch.IsChecked.Value;
            var dinner = CHBDinner.IsChecked.Value;
            

            try
            {
                double subsistenceDouble = Convert.ToDouble(LbTraktamente.Content.ToString());

                var subsistence = l.CalculateSubsistenceDeduction(breakfast, lunch, dinner, subsistenceDouble);

                if (CHBVacationday.IsChecked == true)
                {
                    d.date = listBoxDays.SelectedItem.ToString();
                    d.country = CbCountries.SelectedItem.ToString();

                    d.subsistence = 0;
                    
                    dayinfo = listBoxDays.SelectedItem.ToString() + " - " + CbCountries.SelectedItem.ToString() + " - " +
                    "0 kr";
                    listBoxDays.Items[listBoxDays.SelectedIndex] = dayinfo;
                }
                else
                {
                    d.date = listBoxDays.SelectedItem.ToString();
                    d.country = CbCountries.SelectedItem.ToString();
                    d.subsistence = subsistence;
                    var selectedDays = listBoxDays.SelectedItems.Count;

                    if (selectedDays > 1)
                    {
                        for (int i = 0; selectedDays > i; i++ )
                        {
                            dayinfo = listBoxDays.SelectedItem.ToString() + " - " + CbCountries.SelectedItem.ToString() + " - " +
                                          subsistence + " kr";
                            listBoxDays.Items[listBoxDays.SelectedIndex] = dayinfo;
                        }
                    }
                    else
                    {
                        dayinfo = listBoxDays.SelectedItem.ToString() + " - " + CbCountries.SelectedItem.ToString() + " - " +
                                          subsistence + " kr";
                        listBoxDays.Items[listBoxDays.SelectedIndex] = dayinfo;
                    }
                    
                }
                dayhandler.Add(d);
                //lägger in datum, land, ledighet och traktamente i en lista av typen dayhandler.
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
                var count = 0;
                for (int i = 0; i < dayhandler.Count; i++)
                {

                    foreach (var day in dayhandler)
                    {

                        if (day.date.Contains(listBoxDays.SelectedValue.ToString()))
                        {
                            dayhandler.RemoveAt(count);
                        }

                    }
                    count++;
                }

                var firstSpace = listBoxDays.SelectedValue.ToString().IndexOf(' ');
                listBoxDays.Items[listBoxDays.SelectedIndex] = listBoxDays.SelectedValue.ToString()
                    .Substring(0, firstSpace);
            }
            catch
            {
                MessageBox.Show("Du måste välja en dag att ta bort.");
            }
        }

        private void btnDeleteDraft_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = MessageBox.Show("Vill du verkligen radera allt?", "Radera utkast", MessageBoxButton.YesNo,
                    MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    var today = DateTime.Now.ToString();
                    var tomorrow = DateTime.Now.AddDays(1).ToString();
                    tbDoneOnTrip.Text = "";
                    listBoxReceipts.Items.Clear();
                    listBoxDays.Items.Clear();
                    TbTotalKm.Text = "0";
                    TbCarTripLengthKm.Text = "0";
                    TbDaysOff.Text = "";
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
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        //Fyller tb's med användar info
        public void fillUserInfoOnChange(int userID)
        {
            try
            {

                var users = localHandeler.getInfoOnSelectedUser();
                var bosses = localHandeler.getInfoOnSelectedBoss();

                foreach (var user in users)
                {
                    if (userID == user.UID)
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

                foreach (var boss in bosses)
                {
                    if (userID == boss.BID)
                    {
                        tbFirstName.Text = boss.FirstName;
                        tbLastNamne.Text = boss.LastName;
                        tbEmail.Text = boss.Email;
                        tbPassword.Text = boss.PW;
                        tbUsername.Text = boss.Username;
                        tbSsn.Text = boss.SSN;
                        tbBoss.Text = boss.AprovalBoss.ToString();
                        tbUserID.Text = boss.BID.ToString();
                    }

                }

            }
            catch
            {
                MessageBox.Show("Något blev fel.");
            }
        }


        //Fyllar listview med användare
        private void PopulateListViewUsers()
        {
            try
            {
                var users = localHandeler.getInfoOnSelectedUser();
                var bosses = localHandeler.getInfoOnSelectedBoss();

                foreach (var user in users)
                {
                    listBoxUsers.Items.Add("Anst nr: " + user.UID + " " + user.FirstName + " " + user.LastName);
                }
                foreach (var boss in bosses)
                {
                    listBoxUsers.Items.Add("Anst nr: " + boss.BID + " " + boss.FirstName + " " + boss.LastName);
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }


        //Fyller listViewn med ländernas namn
        private void PupulateListViewCountries()
        {
            try
            {
                var allCountries = localHandeler.getAllCountriesToList();

                foreach (var country in allCountries)
                {
                    lvCountriesEdit.Items.Add(country.Name);
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }

        }



        //Uppdatera Användare
        private void btnUpdateInfo_Click(object sender, RoutedEventArgs e)
        {
            LogicHandler updateUserHandler = new LogicHandler();
            Validator validate = new Validator();
            var bossID = tbBoss.Text;
            var userID = tbUserID.Text;

            try
            {
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
                else if (validate.IsEmailValid(mail))
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
            catch (Exception ee)
            {
                MessageBox.Show("Du måste välja en anställd!");
            }
        }


        //Fyller i TB's med en användare man valt att uppdatera ur Listan
        private void listBoxUsers_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

            try
            {
                var selected = listBoxUsers.SelectedValue.ToString();
                var id = localHandeler.checkIfDigits(selected);
                fillUserInfoOnChange(id);
                DisplayReportsForUser(id);
            }
            catch (Exception ee)
            {

            }

        
        }

        //Lägg till land
        private void btnAddCountry_Click_2(object sender, RoutedEventArgs e)
        {
            try
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
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show("Du måste fylla i alla uppgifter innan du lägger till ett land!");
            }

        }

        //Fyller Tbs med text av landet man valt.
        private void lvCountriesEdit_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
             
            try
            {
                var selected = lvCountriesEdit.SelectedValue.ToString();
                var allCountries = localHandeler.getAllCountriesToList();

                foreach (var countries in allCountries)
                {
                    if (selected == countries.Name)
                    {
                        tbCountryName.Text = countries.Name;
                        tbMaxCash.Text = countries.Subsistence.ToString();
                        tbCurrency.Text = countries.Currency;
                    }
                }
            }
            catch
            {

            }
        
        }

        //Update Country
        private void btnUpdateCountry_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var currname = lvCountriesEdit.SelectedValue.ToString();
                var name = tbCountryName.Text;
                var curr = tbCurrency.Text;
                var sub = Int32.Parse(tbMaxCash.Text);
                var logic = new LogicHandler();


                logic.uppdateCountry(currname, name, curr, sub);
                lvCountriesEdit.Items.Clear();
                PupulateListViewCountries();
                MessageBox.Show(tbCountryName.Text + " Har uppdaterats!");
            }
            catch (Exception ee)
            {
                MessageBox.Show("Du måste välja ett land att uppdatera!");
            }
        }

        //Söka användare
        private void btnSearchUser1_Click(object sender, RoutedEventArgs e)
        {                   
        
            try
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
            catch (Exception)
            {

                throw;
            }

        
        }
        //Hämtar länder från XML
        public void getcountriesfromXML()
        {
            var list = Xmlreader.GetAllCountries();
            CbCountries.ItemsSource = list;

        }
       
        private void btnSendPrepaymet_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var prepayment = new AdvancePayments();

                prepayment.UserID = localHandeler.GetUserId(lbLoggedInAsThisUser.Content.ToString());
                prepayment.Description = tbDescriptionPrepay.Text;
                prepayment.Amount = Decimal.Parse(tbTotalAmounth.Text);

                localHandeler.AddPrepayment(prepayment);
                MessageBox.Show("Förskottsansökan skickad.");
            }
            catch
            {
                MessageBox.Show("Logga in först.");
            }
        }
       
        //Ta bort land
        private void btnRemoveCountry_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selected = lvCountriesEdit.SelectedValue.ToString();

                localHandeler.DeletSelectedCountry(selected);
                MessageBox.Show(selected + " Har tagits bort!");
                lvCountriesEdit.Items.Clear();
                tbCountryName.Clear();
                tbMaxCash.Clear();
                tbCurrency.Clear();
                PupulateListViewCountries();
            }
            catch (Exception ee)
            {
                MessageBox.Show("Du måste välja ett land att ta bort!");
            }
        }

        private void btnFillListWithUsersIAmBossFor_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                listBoxUsers.Items.Clear();
                var myUsers = localHandeler.GetUsersWhoWorksForMe(lbLoggedInUser.Content.ToString());
                var myBosses = localHandeler.GetBossesICanApprove(lbLoggedInUser.Content.ToString());

                foreach (var user in myUsers)
                {
                    listBoxUsers.Items.Add("Anst nr: " + user.UID + " " + user.FirstName + " " + user.LastName);
                }
                foreach (var boss in myBosses)
                {
                    listBoxUsers.Items.Add("Anst nr: " + boss.BID + " " + boss.FirstName + " " + boss.LastName);
                }

            }
            catch
            {
                MessageBox.Show("Logga in först.");
            }
        }

        private void btnShowReports_Click(object sender, RoutedEventArgs e)
        {
            

            lbShowReports.ItemsSource = reportDanger.GetReportList();
        }

        private void btnApprove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cbShowPrepayments.IsChecked == true)
                {
                    string Prepaymentwindowfullstring = lbShowReports.SelectedItem.ToString();
                    prepaymentHandler.SaveStatusUpdateForAccept(Prepaymentwindowfullstring);
                    lbShowReports.ItemsSource = null;
                    lbShowReports.ItemsSource = prepaymentHandler.GetAllPrepaymentsRequest();

                }
                else
                {

                    string fullstringbitch = lbShowReports.SelectedItem.ToString();
                    reportDanger.Acceptpost(fullstringbitch);
                    lbShowReports.ItemsSource = null;
                    lbShowReports.ItemsSource = reportDanger.GetReportList();
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show("Du måste välja en rapport eller förskottsansökan att godkänna!" + ee.Message);
            }
        }


        private void btnDeny_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cbShowPrepayments.IsChecked == true)
                {

                    string Prepaymentwindowfullstring = lbShowReports.SelectedItem.ToString();
                    string Motivation = tbWhyDenied.Text;
                    prepaymentHandler.SaveStatusUpdateForDenial(Prepaymentwindowfullstring, Motivation);
                    lbShowReports.ItemsSource = null;
                    lbShowReports.ItemsSource = prepaymentHandler.GetAllPrepaymentsRequest();

                }

                else
                {
                    string reportwindowfullstring = lbShowReports.SelectedItem.ToString();
                    string motivation = tbWhyDenied.Text;
                    reportDanger.SaveStatusUpdateForDenial(reportwindowfullstring, motivation);
                    lbShowReports.ItemsSource = null;
                    lbShowReports.ItemsSource = reportDanger.GetReportList();
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show("Du måste välja en rapport eller förskottsansökan att neka!");
            }

        }

        private void btnLogInChef_Click(object sender, RoutedEventArgs e)
        {
            var source = 1;
            LoginWindow l = new LoginWindow(source);
            l.Show();
        }

        private void btnForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            Window1 window1 = new Window1();
            window1.Show();
        }
       
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            tbUserID.Clear();
            tbUsername.Clear();
            tbBoss.Clear();
            tbFirstName.Clear();
            tbLastNamne.Clear();
            tbEmail.Clear();
            tbPassword.Clear();
            tbSsn.Clear();
        }

        public void PopulateStatisticsWindowCountryCB()
        {

            cbAllCountriesStatistics.ItemsSource = statisticsHandler.SendCountriesToGui();
        }

        private void cbAllCountriesStatistics_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        
        }

        private void cbAllCountriesStatistics_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            lbReports.ItemsSource =
            statisticsHandler.GetStatisticsOverCountriesWhereUsersBeen(
                cbAllCountriesStatistics.SelectedItem.ToString());

        }

        private void btnShowPDF_Click(object sender, RoutedEventArgs e)
        {
            if (!validera.IsLbEmptyPDF(lbShowReports)) return;
            var reportId = localHandeler.checkIfDigits(lbShowReports.SelectedValue.ToString());
            reportDanger.createPdfFromDbReport(reportId);
        }

        private void btnSearchReport_Click(object sender, RoutedEventArgs e)
        {
            var search = tbSearchReport.Text;

            lbShowReports.ItemsSource = reportDanger.searchReports(search);

        }

        private void cbShowPrepayments_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
               

                lbShowReports.ItemsSource = prepaymentHandler.GetAllPrepaymentsRequest();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        private void cbSortReports_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
            lbShowReports.ItemsSource = sortHandler.GetSortByStatusResult(cbSortReports.SelectedValue.ToString());
        }

        public void fillCbOfStatuses()
        {
            cbSortReports.ItemsSource = sortHandler.GetCbSortList();
        }

        public void DisplayReportsForUser(int id)
        {
            lbShowReports.ItemsSource = sortHandler.GetReportsForSpecificUser(id);
        }

        private void rbSortDate_Checked(object sender, RoutedEventArgs e)
        {
            lbShowReports.ItemsSource = sortHandler.GetReportsByDate();
        }

        private void rbSortName_Checked(object sender, RoutedEventArgs e)
        {
            lbShowReports.ItemsSource = sortHandler.GetReportsByName();
        }

        private void btnSeeReportSummary_Click(object sender, RoutedEventArgs e)
        {
            SeeReportInfoWIndow reportSumWindow = new SeeReportInfoWIndow();
            reportSumWindow.InitializeComponent();
            var selected = lbShowReports.SelectedValue.ToString();
            var id = localHandeler.checkIfDigits(selected);
            var selectedReport = localHandeler.GetSingleReport(id);

            var user = reportDanger.GetSingleUser(selectedReport.UID);
            var travelinfos = reportDanger.GetTravelInfoForSpecificReport(selectedReport.RID);
            var receipts = reportDanger.GetReceiptsForSpecificReport(selectedReport.RID);

            var listOftravelinfos = new List<String>();
            var listOfReceipts = new List<String>();
            foreach (var travel in travelinfos)
            {
                var visitedcountry = localHandeler.getSingleCountry(travel.CID);
                    listOftravelinfos.Add("Reste i " + visitedcountry.Name + " mellan " + travel.StartDate.Value.ToShortDateString() +" - " + travel.EndDate.Value.ToShortDateString()  + " och var ledig " + travel.VacationDays + " dagar.");
            }
            var infoOnTravels = string.Join("\n", listOftravelinfos.ToArray());

            foreach(var receipt in receipts)
            {
                var savedReceipts = reportDanger.GetSingleReceipt(receipt.RID);
                listOfReceipts.Add("Kvitto: " + savedReceipts.TravelReciept + " Kostnad: " + savedReceipts.RecieptAmount);
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
            
            if (selectedReport.Status == null)
            {
                reportSumWindow.lblStatusOnReport.Content = "Ej behandlad"; 
            }
            
            reportSumWindow.Show();

        }

        


    }
    }

