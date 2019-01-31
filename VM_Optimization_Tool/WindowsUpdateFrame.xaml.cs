using System;
using System.Windows;
using WUApiLib;

namespace VM_Optimization_Tool
{
    /// <summary>
    /// Interaktionslogik für WindowsUpdateFrame.xaml
    /// </summary>
    public partial class WindowsUpdateFrame : Window
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
        public WindowsUpdateFrame()
        {
            InitializeComponent();
            InstallButton.IsEnabled = false;
            CheckForUpdates();
        }

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

        public void DownloadUpdates(UpdateCollection uCollection)
        {
            updateDownloader = new UpdateDownloader();
            progressWindow = new ProgressWindow();
            progressWindow.Show();
            progressWindow.Title = "Downloading...";
            downloadJob = null;
            updateDownloader.Updates = uCollection;
            downloadJob = updateDownloader.BeginDownload(new DownloadProgressChangedFunc(this), new DownloadCompletedFunc(this), null);
            
        }


        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {     
            DownloadUpdates(sResult.Updates);
        }

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
                    installCollection.Add(update);
            }
            installer = uSession.CreateUpdateInstaller();
            installer.Updates = installCollection;
            installationJob = installer.BeginInstall(new InstallProgressChangedFunc(this), new InstallCompletedFunc(this), null);
        }
    }

    // handler
    public class DownloadProgressChangedFunc : IDownloadProgressChangedCallback{

        WindowsUpdateFrame windowsUpdateFrame;

        public DownloadProgressChangedFunc(WindowsUpdateFrame windowsUpdateFrame)
        {
            this.windowsUpdateFrame = windowsUpdateFrame;
        }
        public void Invoke(IDownloadJob downloadJob, IDownloadProgressChangedCallbackArgs e)
        {
            windowsUpdateFrame.progressWindow.Dispatcher.BeginInvoke(new Action(() => windowsUpdateFrame.progressWindow.progressBar1.Value = e.Progress.PercentComplete));
            windowsUpdateFrame.progressWindow.Dispatcher.BeginInvoke(new Action(() => windowsUpdateFrame.progressWindow.textBlock1.Text = e.Progress.PercentComplete.ToString()));
        }
    }
    public class DownloadCompletedFunc : IDownloadCompletedCallback
    {
        WindowsUpdateFrame windowsUpdateFrame;
        public DownloadCompletedFunc(WindowsUpdateFrame windowsUpdateFrame)
        {
            this.windowsUpdateFrame = windowsUpdateFrame;
        }
        public void Invoke(IDownloadJob downloadJob, IDownloadCompletedCallbackArgs callbackArgs)
        {
            windowsUpdateFrame.progressWindow.Dispatcher.BeginInvoke(new Action(() => windowsUpdateFrame.progressWindow.Close()));
            windowsUpdateFrame.updateDownloader.EndDownload(windowsUpdateFrame.downloadJob);
            windowsUpdateFrame.downloadJob = null;
            windowsUpdateFrame.Dispatcher.BeginInvoke(new Action(() => windowsUpdateFrame.BeginInstallation()));
        }
    }
    public class SearchCompletedFunc : ISearchCompletedCallback
    {
        WindowsUpdateFrame windowsUpdateFrame;
        public SearchCompletedFunc(WindowsUpdateFrame windowsUpdateFrame)
        {
            this.windowsUpdateFrame = windowsUpdateFrame;
        }
        public void Invoke(ISearchJob searchJob, ISearchCompletedCallbackArgs callbackArgs)
        {
            windowsUpdateFrame.sResult = windowsUpdateFrame.uSearcher.EndSearch(windowsUpdateFrame.searchJob);
            windowsUpdateFrame.Dispatcher.BeginInvoke(new Action (() => windowsUpdateFrame.textBox1.Text = "Found " + windowsUpdateFrame.sResult.Updates.Count + " updates" + Environment.NewLine));
            foreach (IUpdate update in windowsUpdateFrame.sResult.Updates)
            {
                windowsUpdateFrame.Dispatcher.BeginInvoke(new Action(() => windowsUpdateFrame.textBox1.AppendText(update.Title + Environment.NewLine)));
            }
            windowsUpdateFrame.downloadJob = null;
            if (windowsUpdateFrame.sResult.Updates.Count != 0)
            {
                windowsUpdateFrame.Dispatcher.BeginInvoke(new Action(() => windowsUpdateFrame.InstallButton.IsEnabled = true));
            }
        }
    }
    public class InstallProgressChangedFunc : IInstallationProgressChangedCallback
    {
        WindowsUpdateFrame windowsUpdateFrame;
        public InstallProgressChangedFunc(WindowsUpdateFrame windowsUpdateFrame)
        {
            this.windowsUpdateFrame = windowsUpdateFrame;
        }
        public void Invoke(IInstallationJob installationJob, IInstallationProgressChangedCallbackArgs e)
        {
            windowsUpdateFrame.progressWindow.Dispatcher.BeginInvoke(new Action(() => windowsUpdateFrame.progressWindow.progressBar1.Value = e.Progress.PercentComplete));
            windowsUpdateFrame.progressWindow.Dispatcher.BeginInvoke(new Action(() => windowsUpdateFrame.progressWindow.textBlock1.Text = e.Progress.PercentComplete.ToString()));
        }
    }
    public class InstallCompletedFunc : IInstallationCompletedCallback
    {
        WindowsUpdateFrame windowsUpdateFrame;
        public InstallCompletedFunc(WindowsUpdateFrame windowsUpdateFrame)
        {
            this.windowsUpdateFrame = windowsUpdateFrame;
        }
        public void Invoke(IInstallationJob installationJob, IInstallationCompletedCallbackArgs callbackArgs)
        {
            windowsUpdateFrame.installationResult = windowsUpdateFrame.installer.EndInstall(windowsUpdateFrame.installationJob);
            windowsUpdateFrame.progressWindow.Dispatcher.BeginInvoke(new Action(() => windowsUpdateFrame.textBox1.Clear()));
            for (int i = 0; i < windowsUpdateFrame.installCollection.Count-1; i++)
            {
                if (windowsUpdateFrame.installationResult.GetUpdateResult(i).HResult == 0)
                {
                    windowsUpdateFrame.Dispatcher.BeginInvoke(new Action(() => windowsUpdateFrame.textBox1.AppendText("Installed : " + windowsUpdateFrame.installCollection[i].Title)));
                }
                else
                {
                    windowsUpdateFrame.Dispatcher.BeginInvoke(new Action(() => windowsUpdateFrame.textBox1.AppendText("Failed : " + windowsUpdateFrame.installCollection[i].Title)));
                }
            }
            windowsUpdateFrame.installationJob = null;
            windowsUpdateFrame.progressWindow.Dispatcher.BeginInvoke(new Action(() => windowsUpdateFrame.progressWindow.Close()));
        }
    }
}
