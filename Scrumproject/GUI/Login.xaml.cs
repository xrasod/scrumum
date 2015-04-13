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

namespace Scrumproject.GUI
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void BtnAvbryt_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            // ~ Dolis gillar killar ~ 
            // 
//           ▄█▀█▀█▄
//        ▄█▀　　█　　▀█▄
//       ▄█▀　　　　　　　▀█▄
//       █　　　　　　　　　　　█
//       █　　　　　　　　　　　█
//       ▀█▄▄　　█　　　▄█▀
//         █　　▄▀▄　　█
//         █　▀　　　▀　█
//         █　　　　　　　█
//         █　　　　　　　█
//         █　　　　 　　 █
//         █　　　　　　　█
//         █　　　　　　　█
//   ▄█▀▀█▄█　　　　　　　█▄█▀█▄
// ▄█▀▀　　　　▀　　　　　　　　　　　▀▀█
//█▀　　　　　　　　　Pontus Ågren　　▀█
//█　　　　　　　　　　　　　　　　　　　　　　█
//█　　　　　　　　　　　▄█▄　　　　　　　　　█
//▀█　　　　　　　　　█▀　▀█　　　　　　　　█▀
// ▀█▄　　　　　　█▀　　　▀█　　　　　▄█▀
//   ▀█▄▄▄█▀　　　　　　▀█▄▄▄█▀
        }
    }
}
