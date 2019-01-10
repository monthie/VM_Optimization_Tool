using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace ToolUpdate
{
    public class ToolUpdater
    {
        private IToolUpdatable applicationInfo;
        private BackgroundWorker bgWorker;

        public ToolUpdater(IToolUpdatable applicationInfo)
        {
            this.applicationInfo = applicationInfo;

            this.bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork);
            this.bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted);
        }
        public void DoUpdate()
        {
            if (!this.bgWorker.IsBusy)
            {
                this.bgWorker.RunWorkerAsync(this.applicationInfo);
            }
        }
        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                ToolUpdateXml update = (ToolUpdateXml)e.Result;
                if (update != null && update.IsNewerThan(this.applicationInfo.ApplicationAssembly.GetName().Version))
                {
                    /// Hier fehlt was
                    this.DownloadUpdate(update);
                }
            }
        }

        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            IToolUpdatable application = (IToolUpdatable)e.Argument;

            if (!ToolUpdateXml.ExistsOnServer(application.UpdateXmlLocation))
            {
                e.Cancel = true;
            }
            else
            {
                e.Result = ToolUpdateXml.Parse(application.UpdateXmlLocation);
            }
        }  
        private void DownloadUpdate(ToolUpdateXml update)
        {

        }
    }
}
