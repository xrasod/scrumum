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
        Report report = new Report();
        LogicHandler reportHandler = new LogicHandler();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnCreateDraft_Click(object sender, RoutedEventArgs e)
        {
            report.Description = "HEJHEJHEJ";
            report.Id = 1;
            report.NumberOfKilometersDriven = 111;
            report.UserId = 12;
            report.Status = 1;
            reportHandler.SaveDraft(report, "DraftReport.xml");
            btnCreateDraft.Visibility = Visibility.Hidden;
        }

        

    }
}
