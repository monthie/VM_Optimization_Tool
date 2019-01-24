﻿using System;
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
using System.Windows.Threading;
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
        public ProgressWindow progressWindow1;
        public ProgressWindow progressWindow2;
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
                searchJob = uSearcher.BeginSearch("IsInstalled=0 And IsHidden=0", new SearchCompletedFunc(this), null);
            }
            catch (Exception ex)
            {
                textBox1.Text = "Something went wrong: " + ex.Message;
            }
        }

        public void DownloadUpdates(UpdateCollection uCollection)
        {
            updateDownloader = new UpdateDownloader();
            progressWindow1 = new ProgressWindow();
            progressWindow1.Show();
            progressWindow1.Title = "Downloading...";
            downloadJob = null;
            updateDownloader.Updates = uCollection;
            downloadJob = updateDownloader.BeginDownload(new DownloadProgressChangedFunc(this), new DownloadCompletedFunc(this), null);
            BeginInstallation();
        }


        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {     
            DownloadUpdates(sResult.Updates);
        }

        private void BeginInstallation()
        {
            progressWindow2 = new ProgressWindow();
            progressWindow2.Show();
            progressWindow2.Title = "Installation...";
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

    public class DownloadProgressChangedFunc : IDownloadProgressChangedCallback{

        WindowsUpdateFrame windowsUpdateFrame;

        public DownloadProgressChangedFunc(WindowsUpdateFrame windowsUpdateFrame)
        {
            this.windowsUpdateFrame = windowsUpdateFrame;
        }
        public void Invoke(IDownloadJob downloadJob, IDownloadProgressChangedCallbackArgs e)
        {
            windowsUpdateFrame.progressWindow1.Dispatcher.BeginInvoke(new Action(() => windowsUpdateFrame.progressWindow1.progressBar1.Value = e.Progress.PercentComplete));
            windowsUpdateFrame.progressWindow1.Dispatcher.BeginInvoke(new Action(() => windowsUpdateFrame.progressWindow1.textBlock1.Text = e.Progress.PercentComplete.ToString()));
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
            windowsUpdateFrame.progressWindow1.Dispatcher.BeginInvoke(new Action(() => windowsUpdateFrame.progressWindow1.Close()));
            windowsUpdateFrame.updateDownloader.EndDownload(windowsUpdateFrame.downloadJob);
            windowsUpdateFrame.downloadJob = null;
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
            windowsUpdateFrame.progressWindow2.Dispatcher.BeginInvoke(new Action(() => windowsUpdateFrame.progressWindow2.progressBar1.Value = e.Progress.PercentComplete));
            windowsUpdateFrame.progressWindow2.Dispatcher.BeginInvoke(new Action(() => windowsUpdateFrame.progressWindow2.textBlock1.Text = e.Progress.PercentComplete.ToString()));
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
            windowsUpdateFrame.progressWindow2.Dispatcher.BeginInvoke(new Action(() => windowsUpdateFrame.textBox1.Text = "" ));
            for (int i = 0; i < windowsUpdateFrame.installCollection.Count; i++)
            {
                if (windowsUpdateFrame.installationResult.GetUpdateResult(i).HResult == 0)
                {
                    windowsUpdateFrame.progressWindow2.Dispatcher.BeginInvoke(new Action(() => windowsUpdateFrame.textBox1.AppendText("Installed : " + windowsUpdateFrame.installCollection[i].Title)));
                }
                else
                {
                    windowsUpdateFrame.progressWindow2.Dispatcher.BeginInvoke(new Action(() => windowsUpdateFrame.textBox1.AppendText("Failed : " + windowsUpdateFrame.installCollection[i].Title)));
                }
            }
            windowsUpdateFrame.installationJob = null;
        }
    }
}