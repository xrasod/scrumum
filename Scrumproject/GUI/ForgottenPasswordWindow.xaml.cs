
using System.Windows;

using Scrumproject.Logic;

namespace Scrumproject.GUI
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private void ButtonSendForgotten_Click(object sender, RoutedEventArgs e)
        {
            EmailHandler EmailHandler = new EmailHandler();

            string username = tbForgottenUsername.Text;
           
            EmailHandler.SendEmailToBoss(username);
            EmailHandler.SendEmailToUser(username);
            

        }
    }
}
