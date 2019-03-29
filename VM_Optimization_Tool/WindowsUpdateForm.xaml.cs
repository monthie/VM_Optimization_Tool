using System;
using System.Diagnostics;
using System.Windows;
using WUApiLib;

namespace VM_Optimization_Tool
{
    /// <summary>
    /// Windows Updates by WUAPIlib check online for updates and install it
    /// </summary>
    public partial class WindowsUpdateForm : Window
    {
        private UpdateSession uSession;
        public IUpdateSearcher uSearcher;
        public ISearchResult sResult;
        public ProgressWindow progressWindow;
        public IInstallationResult installationResult;
        public IUpdateInstaller installer;
        public UpdateCollection installCollection;
        // Asynch Jobs
        public IDownloadJob downloadJob;
        public ISearchJob searchJob;
        public IInstallationJob installationJob;

        public UpdateDownloader updateDownloader;
        public WindowsUpdateForm()
        {
            InitializeComponent();
            InstallButton.IsEnabled = false;
            CheckForUpdates();
        }

        /// <summary>
        /// Search online for Windows Updates
        /// </summary>
        public void CheckForUpdates()
        {
            uSession = new UpdateSession();
            uSearcher = uSession.CreateUpdateSearcher();
            uSearcher.Online = true;
            try
            {
                searchJob = uSearcher.BeginSearch("IsInstalled=0", new SearchCompletedFunc(this), null);
            }
            catch (Exception ex)
            {
                textBox1.Text = "Something went wrong: " + ex.Message;
            }
        }

        /// <summary>
        /// Download updates async
        /// </summary>
        /// <param name="uCollection"></param>
        public void DownloadUpdates(UpdateCollection uCollection)
        {
            updateDownloader = new UpdateDownloader();
            progressWindow = new ProgressWindow();
            progressWindow.Show();
            progressWindow.Title = "Downloading...";
            downloadJob = null;
            updateDownloader.Updates = uCollection;
            downloadJob = updateDownloader.BeginDownload(new DownloadProgressChangedFunc(this), new DownloadCompletedFunc(this), null);
            InstallButton.IsEnabled = false;
        }

        /// <summary>
        /// exit Frame
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Start update Process
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {     
            DownloadUpdates(sResult.Updates);
        }

        /// <summary>
        /// Install all downloaded updates
        /// Auto accept Eula
        /// </summary>
        public void BeginInstallation()
        {
            progressWindow = new ProgressWindow();
            progressWindow.Show();
            progressWindow.Title = "Installation...";
            installationJob = null;
            
            installCollection = new UpdateCollection();
            foreach (IUpdate update in this.sResult.Updates)
            {
                if (update.IsDownloaded)
                    //testen ob das funktioniert
                    update.AcceptEula();
                // update.InstallationBehavior.RebootBehavior
                    installCollection.Add(update);
            }
            installer = uSession.CreateUpdateInstaller();
            installer.Updates = installCollection;
            installationJob = installer.BeginInstall(new InstallProgressChangedFunc(this), new InstallCompletedFunc(this), null);
        }
    }

    /// <summary>
    /// Download progress changed event handler
    /// </summary>
    public class DownloadProgressChangedFunc : IDownloadProgressChangedCallback{

        WindowsUpdateForm WindowsUpdateFrame;

        public DownloadProgressChangedFunc(WindowsUpdateForm WindowsUpdateFrame)
        {
            this.WindowsUpdateFrame = WindowsUpdateFrame;
        }
        public void Invoke(IDownloadJob downloadJob, IDownloadProgressChangedCallbackArgs e)
        {
            WindowsUpdateFrame.progressWindow.Dispatcher.BeginInvoke(new Action(() => WindowsUpdateFrame.progressWindow.progressBar1.Value = e.Progress.PercentComplete));
            WindowsUpdateFrame.progressWindow.Dispatcher.BeginInvoke(new Action(() => WindowsUpdateFrame.progressWindow.textBlock1.Text = e.Progress.PercentComplete.ToString()+"%"));
        }
    }

    /// <summary>
    /// download completed event handler
    /// </summary>
    public class DownloadCompletedFunc : IDownloadCompletedCallback
    {
        WindowsUpdateForm WindowsUpdateFrame;
        public DownloadCompletedFunc(WindowsUpdateForm WindowsUpdateFrame)
        {
            this.WindowsUpdateFrame = WindowsUpdateFrame;
        }
        public void Invoke(IDownloadJob downloadJob, IDownloadCompletedCallbackArgs callbackArgs)
        {
            WindowsUpdateFrame.progressWindow.Dispatcher.BeginInvoke(new Action(() => WindowsUpdateFrame.progressWindow.Close()));
            WindowsUpdateFrame.updateDownloader.EndDownload(WindowsUpdateFrame.downloadJob);
            WindowsUpdateFrame.downloadJob = null;
            WindowsUpdateFrame.Dispatcher.BeginInvoke(new Action(() => WindowsUpdateFrame.BeginInstallation()));
        }
    }

    /// <summary>
    /// search completed event handler
    /// </summary>
    public class SearchCompletedFunc : ISearchCompletedCallback
    {
        WindowsUpdateForm WindowsUpdateFrame;
        public SearchCompletedFunc(WindowsUpdateForm WindowsUpdateFrame)
        {
            this.WindowsUpdateFrame = WindowsUpdateFrame;
        }
        public void Invoke(ISearchJob searchJob, ISearchCompletedCallbackArgs callbackArgs)
        {
            WindowsUpdateFrame.sResult = WindowsUpdateFrame.uSearcher.EndSearch(WindowsUpdateFrame.searchJob);
            WindowsUpdateFrame.Dispatcher.BeginInvoke(new Action (() => WindowsUpdateFrame.textBox1.Text = "Found " + WindowsUpdateFrame.sResult.Updates.Count + " updates" + Environment.NewLine));
            foreach (IUpdate update in WindowsUpdateFrame.sResult.Updates)
            {
                WindowsUpdateFrame.Dispatcher.BeginInvoke(new Action(() => WindowsUpdateFrame.textBox1.AppendText(update.Title + Environment.NewLine)));
            }
            WindowsUpdateFrame.downloadJob = null;
            if (WindowsUpdateFrame.sResult.Updates.Count != 0)
            {
                WindowsUpdateFrame.Dispatcher.BeginInvoke(new Action(() => WindowsUpdateFrame.InstallButton.IsEnabled = true));
            }
        }
    }

    /// <summary>
    /// Install progress event handler
    /// </summary>
    public class InstallProgressChangedFunc : IInstallationProgressChangedCallback
    {
        WindowsUpdateForm WindowsUpdateFrame;
        public InstallProgressChangedFunc(WindowsUpdateForm WindowsUpdateFrame)
        {
            this.WindowsUpdateFrame = WindowsUpdateFrame;
        }
        public void Invoke(IInstallationJob installationJob, IInstallationProgressChangedCallbackArgs e)
        {
            WindowsUpdateFrame.progressWindow.Dispatcher.BeginInvoke(new Action(() => WindowsUpdateFrame.progressWindow.progressBar1.Value = e.Progress.PercentComplete));
            WindowsUpdateFrame.progressWindow.Dispatcher.BeginInvoke(new Action(() => WindowsUpdateFrame.progressWindow.textBlock1.Text = e.Progress.PercentComplete.ToString()+"%"));
        }
    }

    /// <summary>
    /// install completed event handler
    /// </summary>
    public class InstallCompletedFunc : IInstallationCompletedCallback
    {
        WindowsUpdateForm WindowsUpdateFrame;
        public InstallCompletedFunc(WindowsUpdateForm WindowsUpdateFrame)
        {
            this.WindowsUpdateFrame = WindowsUpdateFrame;
        }
        public void Invoke(IInstallationJob installationJob, IInstallationCompletedCallbackArgs callbackArgs)
        {
            WindowsUpdateFrame.installationResult = WindowsUpdateFrame.installer.EndInstall(WindowsUpdateFrame.installationJob);
            WindowsUpdateFrame.progressWindow.Dispatcher.BeginInvoke(new Action(() => WindowsUpdateFrame.textBox1.Clear()));
            // vlt i = 1 statt count-1 TESTEN!

            for (int i = 0; i < WindowsUpdateFrame.installCollection.Count; i++)
            {
                if (WindowsUpdateFrame.installationResult.GetUpdateResult(i).HResult == 0)
                {
                    WindowsUpdateFrame.Dispatcher.BeginInvoke(new Action(() => WindowsUpdateFrame.textBox1.AppendText("Installed : " + WindowsUpdateFrame.installCollection[i].Title+"\r\n")));
                }
                else
                {
                    WindowsUpdateFrame.Dispatcher.BeginInvoke(new Action(() => WindowsUpdateFrame.textBox1.AppendText("Failed : " + WindowsUpdateFrame.installCollection[i].Title + "\r\n")));
                }
            }
            if(WindowsUpdateFrame.installationResult.RebootRequired) Process.Start("shutdown", "/r /f /t 30");
            WindowsUpdateFrame.installationJob = null;
            WindowsUpdateFrame.progressWindow.Dispatcher.BeginInvoke(new Action(() => WindowsUpdateFrame.progressWindow.Close()));
            
        }
    }
}
