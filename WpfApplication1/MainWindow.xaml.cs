using System;
using System.Collections.Generic;
using System.IO;
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
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string accountName = "scrumprojekt";
        string accessKey =
            "J0b3PkuN5FHd5QNjnjS7080NRdWAILm/uSJV32rhEjB8Sxw3tuKyGyyqsi9JxM0LrfvVx7U1qzJN4uNJdn88cw==";
        public MainWindow()
        {
            InitializeComponent();
        }


        private void btnUploadReceipt_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void btnUploadReceipt_Click_1(object sender, RoutedEventArgs e)
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
                
                lblFilename.Content = filename;
            }
            //inlogg till Azure storage kontot
           
            try
            {
                StorageCredentials creds = new StorageCredentials(accountName, accessKey);
                CloudStorageAccount account = new CloudStorageAccount(creds, useHttps: true);

                CloudBlobClient client = account.CreateCloudBlobClient();
                //Den Storage container jag skapat för att lagra kvitton
                //Här kan vi göra valet att lägga alla kvitton i samma container (mapp) och autogenerera kvittonamnen så att de blir unika
                //Alternativt kan man skapa en container för varje användare. Vet faktiskt inte vad som är smartast. 
                CloudBlobContainer kvittoContainer = client.GetContainerReference("kvitton");
                kvittoContainer.CreateIfNotExists();

                CloudBlockBlob blob = kvittoContainer.GetBlockBlobReference("APictureFie.jpg"); //Här sätter vi namnet på filen när den laddas upp. Viktigt att få med .jpg liknande
                using (Stream file = System.IO.File.OpenRead(dlg.FileName)) //Använder sökvägen ifrån "openfiledialog" och skapar en stream
                {
                    blob.UploadFromStream(file); //Själva uppladdningen
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            StorageCredentials creds = new StorageCredentials(accountName, accessKey);
            CloudStorageAccount account = new CloudStorageAccount(creds, useHttps: true);
            CloudBlobClient client = account.CreateCloudBlobClient();
            CloudBlobContainer kvittoContainer = client.GetContainerReference("kvitton");
            CloudBlockBlob blob = kvittoContainer.GetBlockBlobReference("APictureFie.jpg");

            using (Stream outputFile = new FileStream("downloaded.jpg", FileMode.Create))
            {
                blob.DownloadToStream(outputFile);
            }
        }


    }   
}
