using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using System.IO;

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
        private volatile bool isUserScroll = true;
        public bool IsAutoScrollEnabled { get; set; }
        private string[] commands;// {"/lowdisk", "/Online /Cleanup-Image /AnalyzeComponentStore", "/Online /Cleanup-Image /StartComponentCleanup /ResetBase", "/C sc config defragsvc start= demand", "C:\\ /H /U /V", "/C sc config defragsvc start= disabled", "-z C:" }; // 
        private string[] program;// {"cleanmgr.exe", "dism.exe", "dism.exe", "cmd.exe", "defrag.exe", "cmd.exe", "C:\\Program Files\\VM Optimization Tool\\Ressource\\sdelete.exe" }; //  
        private Regex progressRegex = new Regex(@"\[[^\]]*\]");
        private Regex defragRegex = new Regex(@"[a-zA-Z|\s|\\t]+:\s+[0-9]+\s%\s[a-zA-Z|\s]+\.{1,3}");//[a-zA-Z|\s|\\t]+:\s+[0-9]+\s%\s[a-zA-Z|\s]+\.\.\.
        private Regex sdeleteRegex1 = new Regex(@"[a-zA-Z|\s]+(:\\:\s)?[0-9]*\%\s*[a-z]*");
        private Regex sdeleteRegex2 = new Regex(@"[a-zA-z|\s]+\.{3}[||/|\-|\\]");
        private string pathLOpt;

        public Optimization(string[] program, string[] commands, string pathLOpt)
        {
            InitializeComponent();
            this.pathLOpt = pathLOpt;
            this.program = new string[program.Length];
            this.commands = new string[commands.Length];
            this.program = program;
            this.commands = commands;
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
                try
                {
                    if (!message.Contains("-1"))
                    {
                        File.AppendAllText(pathLOpt, DateTime.Now.ToString() + Environment.NewLine);
                    }
                }
                catch (Exception) { }
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
                proc.OutputDataReceived += (es, args) => textBox.Dispatcher.BeginInvoke(new Action(() => OutputHandler(es, args)));
                proc.ErrorDataReceived += (es, args) => textBox.Dispatcher.BeginInvoke(new Action(() => OutputHandler(es, args)));
                proc.Wait(token);
                list.Add(program[i]+" "+commands[i]+"\n Exit Code: "+proc.ExitCode.ToString()+"\n");
                int percentageCompleted = (int)((float)count / (float)commands.Length * 100);
                bgWorker.ReportProgress(percentageCompleted);
            }
        }

        private void OutputHandler(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null && (progressRegex.IsMatch(e.Data)||defragRegex.IsMatch(e.Data)
                || sdeleteRegex1.IsMatch(e.Data) 
                || sdeleteRegex2.IsMatch(e.Data)))
            {
                if (progressRegex.IsMatch(textBox.GetLineText(textBox.GetLastVisibleLineIndex()))
                    || defragRegex.IsMatch(textBox.GetLineText(textBox.GetLastVisibleLineIndex()))
                    || sdeleteRegex1.IsMatch(textBox.GetLineText(textBox.GetLastVisibleLineIndex()))
                    || sdeleteRegex2.IsMatch(textBox.GetLineText(textBox.GetLastVisibleLineIndex())))
                {
                    textBox.Text = textBox.Text.Remove(textBox.Text.LastIndexOf(textBox.GetLineText(textBox.GetLastVisibleLineIndex())));
                    textBox.AppendText(e.Data);
                }
                else
                {
                    textBox.AppendText(e.Data);
                }
                
            } else if(e.Data != null && e.Data != "")
            {
                textBox.AppendText("\n"+e.Data);
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
            var test = textBox.ViewportHeight;
            
            textBox.ScrollToEnd();
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