using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace VM_Optimization_Tool
{
    /// <summary>
    /// Interaktionslogik für Optimization.xaml
    /// </summary>
    public partial class Optimization : Window
    {
        private BackgroundWorker bgWorker;
        private CancellationTokenSource cts;
        private CancellationToken token;
        private List<string> list = new List<string>();
        private string[] commands = new string[7] { "/lowdisk", "/Online /Cleanup-Image /AnalyzeComponentStore", "/Online /Cleanup-Image /StartComponentCleanup /ResetBase", "/C sc config defragsvc start= demand", "C:\\ /H /U /V", "/C sc config defragsvc start= disabled", "-z C:" };
        private string[] program = new string[7] { "cleanmgr.exe", "dism.exe", "dism.exe", "cmd.exe", "defrag.exe", "cmd.exe", "C:\\Program Files\\VM Optimization Tool\\Ressource\\sdelete.exe" };

        public Optimization()
        {
            InitializeComponent();
            Closing += windowClosing;
            bgWorker = new BackgroundWorker();
            bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork);
            bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted);
            bgWorker.ProgressChanged += new ProgressChangedEventHandler(bgWorker_ProgressChanged);
            bgWorker.WorkerSupportsCancellation = true;
            bgWorker.WorkerReportsProgress = true;
            cts = new CancellationTokenSource();
            token = cts.Token;
            bgWorker.RunWorkerAsync();
        }

        private void bgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
        }

        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }
            else if (e.Cancelled)
            {
                MessageBox.Show("Cancelled");
            }
            else
            {
                MessageBox.Show("Succeded");     
            }
            textBox.Dispatcher.BeginInvoke(new Action(() => textBox.Text += "Summary:\n"));
            foreach (string message in list)
            {
                textBox.Dispatcher.BeginInvoke(new Action(() => textBox.Text += message));
            }
            btnAbort.Dispatcher.BeginInvoke(new Action(() => btnAbort.Content = "Close"));
        }

        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            int count = 0;
            for (int i = 0; i < commands.Length; i++)
            {
                if (bgWorker.CancellationPending == true)
                {
                    e.Cancel = true;
                    return;
                }
                count += 1;
                textBlock.Dispatcher.BeginInvoke(new Action(() => textBlock.Text = "Step: "+count.ToString()+"/"+commands.Length.ToString()));
                windowName.Dispatcher.BeginInvoke(new Action(() => windowName.Title = program[i] + " " + commands[i]));
                var proc = new ProcessUtil(program[i], commands[i]); //, commands[i]
                proc.Start();
                proc.OutputDataReceived += (es, args) => textBox.Dispatcher.BeginInvoke(new Action(() => textBox.AppendText(args.Data + "\n")));
                proc.ErrorDataReceived += (es, args) => textBox.Dispatcher.BeginInvoke(new Action(() => textBox.AppendText(args.Data + "\n")));
                proc.Wait(token);
                list.Add(program[i]+" "+commands[i]+"\n Exit Code: "+proc.ExitCode.ToString()+"\n");
                int percentageCompleted = (int)((float)count / (float)commands.Length * 100);
                bgWorker.ReportProgress(percentageCompleted);
            }
        }

        private void abort_Click(object sender, RoutedEventArgs e)
        {
            if (bgWorker.WorkerSupportsCancellation == true)
            {
                cts.Cancel();
                // Cancel the asynchronous operation.
                bgWorker.CancelAsync();
            }
            if(btnAbort.Content.ToString() == "Close")
            {
                this.Close();
            }
            //Close();
        }
        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            textBox.ScrollToEnd();
        }
        private void WindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            myCanvas.Width = e.NewSize.Width;
            myCanvas.Height = e.NewSize.Height;

            double xChange = 1, yChange = 1;

            if (e.PreviousSize.Width != 0)
                xChange = (e.NewSize.Width / e.PreviousSize.Width);

            if (e.PreviousSize.Height != 0)
                yChange = (e.NewSize.Height / e.PreviousSize.Height);

            foreach (FrameworkElement fe in myCanvas.Children)
            {
                /*because I didn't want to resize the grid I'm having inside the canvas in this particular instance. (doing that from xaml) */
                if (fe is Grid == false)
                {
                    fe.Height = fe.ActualHeight * yChange;
                    fe.Width = fe.ActualWidth * xChange;

                    Canvas.SetTop(fe, Canvas.GetTop(fe) * yChange);
                    Canvas.SetLeft(fe, Canvas.GetLeft(fe) * xChange);

                }
            }
        }
        /// <summary>
        /// If window will be closed cancel all processes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void windowClosing(object sender, EventArgs e)
        {
            if (bgWorker.WorkerSupportsCancellation == true)
            {
                cts.Cancel();
                // Cancel the asynchronous operation.
                bgWorker.CancelAsync();
            }
        }
    }
}
