using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using ToolUpdate;
using System.Diagnostics;

namespace VM_Optimization_Tool
{
    /// <summary>
    /// Interaktionslogik für DownloadForm.xaml
    /// </summary>
    public partial class DownloadForm : Window
    {
        /// <summary>
        /// The web client to download the update
        /// </summary>
        private WebClient webClient;

        /// <summary>
        /// The thread to hash the file on
        /// </summary>
        private BackgroundWorker bgWorker;

        /// <summary>
        /// The MD5 hash of the file to download
        /// </summary>
        private string md5;

        private volatile bool _completed;

        

        /// <summary>
        /// Gets the temp file path for the downloaded file
        /// </summary>
        internal string TempFilePath { get; }
        public DownloadForm(Uri location, string md5)
        {
            InitializeComponent();

            
            // Set the temp file name and create new 0-byte file
            TempFilePath = Path.GetTempFileName();

            this.md5 = md5;
            _completed = false;

            // Set up WebClient to download file
            webClient = new WebClient();
            webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(webClient_DownloadProgressChanged);
            webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(webClient_DownloadFileCompleted);

            // Set up backgroundworker to hash file
            bgWorker = new BackgroundWorker();
            bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork);
            bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted);

            // Download file
            try { webClient.DownloadFileAsync(location, TempFilePath); }
            catch { Close(); }
        }

        public bool DownloadCompleted { get { return _completed; } }
        /// <summary>
        /// Downloads file from server
        /// </summary>
        private void webClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            // Update progressbar on download
            this.lblProgress.Content = string.Format("Downloaded {0} of {1}", FormatBytes(e.BytesReceived, 1, true), FormatBytes(e.TotalBytesToReceive, 1, true));
            this.progressBar.Value = e.ProgressPercentage;
        }

        private void webClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBoxResult result = MessageBox.Show("Download failed",
                                          "Confirmation",
                                          MessageBoxButton.OK,
                                          MessageBoxImage.Question);
                if (result == MessageBoxResult.OK)
                {
                    Close();
                }
            }
            else if (e.Cancelled)
            {
                MessageBoxResult result = MessageBox.Show("Download failed",
                                          "Confirmation",
                                          MessageBoxButton.OK,
                                          MessageBoxImage.Question);
                if (result == MessageBoxResult.OK)
                {
                    Close();
                }
            }
            else
            {
                // Show the "Hashing" label and set the progressbar to marquee
                lblProgress.Content = "Verifying Download...";
                progressBar.IsIndeterminate = true;

                // Start the hashing
                bgWorker.RunWorkerAsync(new string[] { TempFilePath, md5 });
            }
        }

        /// <summary>
        /// Formats the byte count to closest byte type
        /// </summary>
        /// <param name="bytes">The amount of bytes</param>
        /// <param name="decimalPlaces">How many decimal places to show</param>
        /// <param name="showByteType">Add the byte type on the end of the string</param>
        /// <returns>The bytes formatted as specified</returns>
        private string FormatBytes(long bytes, int decimalPlaces, bool showByteType)
        {
            double newBytes = bytes;
            string formatString = "{0";
            string byteType = "B";

            // Check if best size in KB
            if (newBytes > 1024 && newBytes < 1048576)
            {
                newBytes /= 1024;
                byteType = "KB";
            }
            else if (newBytes > 1048576 && newBytes < 1073741824)
            {
                // Check if best size in MB
                newBytes /= 1048576;
                byteType = "MB";
            }
            else
            {
                // Best size in GB
                newBytes /= 1073741824;
                byteType = "GB";
            }

            // Show decimals
            if (decimalPlaces > 0)
                formatString += ":0.";

            // Add decimals
            for (int i = 0; i < decimalPlaces; i++)
                formatString += "0";

            // Close placeholder
            formatString += "}";

            // Add byte type
            if (showByteType)
                formatString += byteType;

            return string.Format(formatString, newBytes);
        }

        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            string file = ((string[])e.Argument)[0];
            string updateMD5 = ((string[])e.Argument)[1];

            // Hash the file and compare to the hash in the update xml
            if (Hasher.HashFile(file, HashType.MD5).ToUpper() != updateMD5.ToUpper()) {
                MessageBoxResult result = MessageBox.Show("Download failed",
                                      "Confirmation",
                                      MessageBoxButton.OK,
                                      MessageBoxImage.Question);
            }
            else
            {
                _completed = true;
            }
        }

        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (_completed) {UpdateApplication(); }
            Close();
        }

        private void DownloadForm_Closed(object sender, CancelEventHandler e)
        {
            if (webClient.IsBusy)
            {
                webClient.CancelAsync();
            }

            if (bgWorker.IsBusy)
            {
                bgWorker.CancelAsync();
            }
        }

        /// <summary>
        /// Function to close remove and move application
        /// </summary>
        private void UpdateApplication()
        {
            string argument_update = "/C choice /C Y /N /D Y /T 1 & Del /F /Q \"{0}\" & choice /C Y /N /D Y /T 2 & copy /Y \"{1}\" \"{2}\"";
            string argument_update_start = argument_update + "& choice /C Y /N /D Y /T 1 & Start \"\" /D \"{3}\" \"{4}\" {5}";
            string argument_complete;
            string currentPath = Path.GetFullPath(Process.GetCurrentProcess().MainModule.FileName);
            //Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);

            string tempFile = TempFilePath;
            //string newPath = Path.GetFullPath(applicationInfo.ApplicationPath);

            argument_complete = string.Format(argument_update_start, currentPath, tempFile, currentPath, Path.GetDirectoryName(currentPath), Path.GetFileName(currentPath), "");
            Console.WriteLine("Update and run main app: " + argument_complete);

            ProcessStartInfo cmd_main = new ProcessStartInfo
            {
                Arguments = argument_complete,
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                FileName = "cmd.exe"
            };
            Process.Start(cmd_main);
            Environment.Exit(0);
        }
    }
}