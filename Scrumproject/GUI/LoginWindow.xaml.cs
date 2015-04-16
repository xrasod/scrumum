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
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        int source;

        public LoginWindow(int source)
        {
            InitializeComponent();
            this.source = source;
        }

        private void BtnCloseWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            LogicHandler l = new LogicHandler();

            var username = TbUsername.Text;
            var password = TbPassword.Text;

            var loggedUser = l.loginUser(username, password);
            var loggedBoss = l.loginBoss(username, password);

            if (loggedUser != null && source == 2) //source, chef eller vanlig användare
            {
                MainWindow.main.Status = loggedUser.Username;
                MainWindow.main.btnLogOut.Visibility = Visibility.Visible;
                MainWindow.main.BtnLogIn.Visibility = Visibility.Hidden;
                this.Close();
            }
            else if (loggedBoss != null && source == 1)
            {
                var isBoss = String.Format(loggedBoss.Username + " (Chef)");
                MainWindow.main.BossStatus = isBoss;
                MainWindow.main.btnLogOutChef.Visibility = Visibility.Visible;
                MainWindow.main.btnLogInChef.Visibility = Visibility.Hidden;
                this.Close();
            }
            else
            {
                lbError.Content = "Misslyckad inloggning! Försök igen tack å hej";
            }

        }
    }
}
