using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Security.Cryptography;
using System.IO;
using Ookii.Dialogs.Wpf;

namespace FileVerifier
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly BackgroundWorker _worker = new BackgroundWorker();
        public MainWindow()
        {
            InitializeComponent();
            _worker.DoWork += worker_DoWork;
            _worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            _worker.WorkerReportsProgress = true;
            _worker.ProgressChanged+=worker_ProgressChanged;
        }

        public static string Md5;
        public static string Sha1;
        public static string Sha256;
        public static string Filepath;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            VistaOpenFileDialog diag =new VistaOpenFileDialog()
            {
                
            };
            diag.CheckFileExists = true;
            diag.Multiselect = false;
            diag.CheckPathExists = true;
            if (diag.ShowDialog() == true)
            {
                var file = diag.FileName;
                FilePath.Text = file;
                // Do something with selected folder string
            }
        }
        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            try
            {
                worker.ReportProgress(0, "Calculating MD5...");
                Md5 = CheckMd5(Filepath);
                worker.ReportProgress(33, "Calculating SHA1...");
                Sha1 = CheckSha(Filepath);
                worker.ReportProgress(66, "Calculating SHA256...");
                Sha256 = CheckSha256(Filepath);
            
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }
      

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Vbutton.Content = e.UserState.ToString();
        }
        private void worker_RunWorkerCompleted(object sender,RunWorkerCompletedEventArgs e)
        {
            CalcSha256.Text = Sha256;
            CalcMd5.Text = Md5;
            CalcSha.Text = Sha1;
            Vbutton.IsEnabled = true;
            Vbutton.Content = "Verify Checksum!";
            if (OriMd5.Text != "")
            {

                var oriHash = OriMd5.Text.Replace("-", "‌​").Replace("\u200B", "").Replace("\u200C", "").ToLower();

                if (oriHash == Sha256 || oriHash == Md5 || oriHash == Sha1)

                {
                    Status.Text = "PASSED!";
                    StatGrid.Background = Brushes.LimeGreen;
                }
                else
                {
                    Status.Text = "FAILED!";
                    StatGrid.Background = Brushes.Red;
                }
            }
            else
            {
                Status.Text = "Next, Paste Original Hash, Please!";
                StatGrid.Background = null;
            }
        }
        private void vbutton_Click(object sender, RoutedEventArgs e)
        {
            Filepath = FilePath.Text;
            Vbutton.Content = "Calculating Checksum...";
            Vbutton.IsEnabled = false;
            
            _worker.RunWorkerAsync();

           
        }
        public string CheckMd5(string filename)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "‌​").Replace("\u200B", "").Replace("\u200C", "").ToLower();
                }

            }
        }
        public string CheckSha(string filename)
        {
            using (var sha =SHA1.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    return BitConverter.ToString(sha.ComputeHash(stream)).Replace("-", "‌​").Replace("\u200B", "").Replace("\u200C", "").ToLower();
                }

            }
        }
        public string CheckSha256(string filename)
        {
            using (var sha = SHA256.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    return BitConverter.ToString(sha.ComputeHash(stream)).Replace("-", "‌​").Replace("\u200B", "").Replace("\u200C", "").ToLower();
                }

            }
        }

        private void OriMD5_TextChanged(object sender, TextChangedEventArgs e)
        {
           
            if (CalcSha256.Text != "")
            {
                var oriHash = OriMd5.Text.Replace("-", "‌​").Replace("\u200B", "").Replace("\u200C", "").ToLower();
                var hash256 = CalcSha256.Text;
                var hashmd5 = CalcMd5.Text;
                var hash1 = CalcSha.Text;
               
                char[] arr = CalcSha256.Text.ToCharArray();

                if (oriHash==hash256||oriHash==hashmd5||oriHash==hash1)
               
                {
                    Status.Text = "PASSED!";
                    StatGrid.Background = Brushes.LimeGreen;
                }
                else
                {
                    Status.Text = "FAILED!";
                    StatGrid.Background = Brushes.Red;
                }
            }
            else
            {
                Status.Text = "Click Verify Button, Please!";
                StatGrid.Background = null;
            }
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            //
        }
    }
}
