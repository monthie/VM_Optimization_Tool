using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows;


namespace VM_Optimization_Tool
{

    /// <summary>
    /// Interaktionslogik für UpdateAvailableForm.xaml
    /// </summary>
    public partial class UpdateAvailableForm : Window
    {
        /*
        /// <summary>
        /// The update version 
        /// </summary>
        public Version Version { get; set; }

        /// <summary>
        /// The update's description
        /// </summary>
        public string Description { get; set; }
        */
        /// <summary>
        /// Array with update information
        /// </summary>
        public ToolUpdateXml updateInfo;

        /// <summary>
        /// The web client to download the update
        /// </summary>
        private WebClient webClient;

        /// <summary>
        /// The thread to hash the file on
        /// </summary>
        private BackgroundWorker bgWorker;

        /// <summary>
        /// Progress frame for update process
        /// </summary>
        private ProgressWindow progressWindow;

        /// <summary>
        /// The MD5 hash of the file to download
        /// </summary>
        //private string md5;

        private volatile bool _completed;

        /// <summary>
        /// Gets the temp file path for the downloaded file
        /// </summary>
        private string TempFilePath { get; set; }

        public UpdateAvailableForm(ToolUpdateXml updateInfo)
        {
            InitializeComponent();
            this.updateInfo = updateInfo;
        }
        public void UpdateDescription()
        {
            textbox.Text = "Version: " + updateInfo.Version + "\n Change Log " + updateInfo.Description;
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            // Set the temp file name and create new 0-byte file
            TempFilePath = Path.GetTempFileName();

            progressWindow = new ProgressWindow();
            progressWindow.Topmost = true;
            progressWindow.Show();
            // Set up WebClient to download file
            webClient = new WebClient();
            webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(webClient_DownloadProgressChanged);
            webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(webClient_DownloadFileCompleted);

            // Set up backgroundworker to hash file
            bgWorker = new BackgroundWorker();
            bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork);
            bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted);

            // Download file
            try { webClient.DownloadFileAsync(updateInfo.Uri, TempFilePath); }
            catch (Exception ex) { LogWriter.LogWrite("Application updated failed: " + ex.ToString()); Close(); }
        }

        /// <summary>
        /// After successful download start update process
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (_completed) { UpdateApplication(); }
            progressWindow.Close();
        }

        /// <summary>
        /// Backgroundworker checks hash of downloaded file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            string file = ((string[])e.Argument)[0];
            string updateMD5 = ((string[])e.Argument)[1];

            // Hash the file and compare to the hash in the update xml
            if (Hasher.HashFile(file, HashType.MD5).ToUpper() != updateMD5.ToUpper())
            {
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

        /// <summary>
        /// Async eventhandler to check if download was successful
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void webClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null || e.Cancelled)
            {
                MessageBoxResult result = MessageBox.Show("Download failed",
                                          "Error",
                                          MessageBoxButton.OK,
                                          MessageBoxImage.Error);
                if (result == MessageBoxResult.OK)
                {
                    Close();
                }
            }
            else
            {
                // Show the "Hashing" label and set the progressbar to marquee
                progressWindow.textBlock1.Text = "Verifying Download...";
                progressWindow.progressBar1.IsIndeterminate = true;

                // Start the hashing
                bgWorker.RunWorkerAsync(new string[] { TempFilePath, updateInfo.MD5 });
            }
        }

        /// <summary>
        /// Downloads file from server
        /// </summary>
        private void webClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressWindow.Dispatcher.BeginInvoke(new Action(() => progressWindow.progressBar1.Value = e.ProgressPercentage));
        }

        private void Abort_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Close all background threads if download form get closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// Update process
        /// </summary>
        private void UpdateApplication()
        {
            string argument_update = "/C choice /C Y /N /D Y /T 1 & Del /F /Q \"{0}\" & choice /C Y /N /D Y /T 2 & copy /Y \"{1}\" \"{2}\"";
            string argument_update_start = argument_update + "& choice /C Y /N /D Y /T 1 & Start \"\" /D \"{3}\" \"{4}\" {5}";
            string argument_complete;
            string currentPath = Path.GetFullPath(Process.GetCurrentProcess().MainModule.FileName);

            string tempFile = TempFilePath;

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
        