using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace VM_Optimization_Tool
{
    /// <summary>
    /// Interaktionslogik für PreOptimizationWindow.xaml
    /// </summary>
    public partial class PreOptimizationWindow : Window
    {
        private List<string> commandsList = new List<string>();
        private List<string> programList = new List<string>();
        private CheckBox[] checkBoxes = new CheckBox[4];
        private string path = @"C:\\Program Files\\VM Optimization Tool\\LOpt";
        private string readLine;
        private List<DateTime> lOptList = new List<DateTime>();

        public PreOptimizationWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// not in use
        /// tryed to get last optimization for preselecting steps
        /// </summary>
        private void GetLastOptimization()
        {
            if (File.Exists(path))
            {
                // Read the file and display it line by line.  
                StreamReader file = new StreamReader(path);
                while ((readLine = file.ReadLine()) != null)
                {
                    try
                    {
                        DateTime parsedDate = DateTime.Parse(readLine);
                        lOptList.Add(parsedDate);
                    }
                    catch (Exception)
                    {

                    }
                }
                file.Close();
            } else
            {
                File.Create(path);
            }
            checkBoxes[0] = checkCleanMgr;
            checkBoxes[1] = checkDism;
            checkBoxes[2] = checkDefrag;
            checkBoxes[3] = checkSDelete;
            if (lOptList.Count > 0)
            {
                DateTime[] tmp = new DateTime[lOptList.Count];
                tmp = lOptList.ToArray();
                for (int i = 0; i < tmp.Length; i++)
                {
                    if ((tmp[i].Month + 1 + tmp[i].Year) <= DateTime.Now.Month + DateTime.Now.Year)
                    {
                        checkBoxes[i].IsChecked = true;
                    }
                    else
                    {
                        checkBoxes[i].IsChecked = false;
                    }
                }
            }
            else
            {
                foreach (CheckBox box in checkBoxes)
                {
                    box.IsChecked = true;
                    box.Content = box.Content + " never optimized";
                }
            }
        }
        /// <summary>
        /// Various start options.
        /// sagerun 64 and 32 are different options for cleanmgr
        /// The registry key is set here during the installation of the tools.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)checkCleanMgr.IsChecked)
            {
                if ((bool)radioButton1.IsChecked)
                {
                    commandsList.Add("/sagerun:64");
                }
                else if ((bool)radioButton2.IsChecked)
                {
                    commandsList.Add("/sagerun:32");
                }
                else if ((bool)radioButton3.IsChecked)
                {
                    commandsList.Add("/lowdisk");
                }
                programList.Add("cleanmgr.exe");
            }
            if ((bool)checkDism.IsChecked)
            {
                commandsList.Add("/Online /Cleanup-Image /AnalyzeComponentStore");
                commandsList.Add("/Online /Cleanup-Image /StartComponentCleanup /ResetBase");
                programList.Add("dism.exe");
                programList.Add("dism.exe");
            }
            if ((bool)checkDefrag.IsChecked)
            {
                commandsList.Add("/C sc config defragsvc start=demand");
                commandsList.Add("C:\\ /H /U /V");
                commandsList.Add("/C sc config defragsvc start= disabled");
                programList.Add("cmd.exe");
                programList.Add("defrag.exe");
                programList.Add("cmd.exe");
            }
            if ((bool)checkSDelete.IsChecked)
            {
                commandsList.Add("-z C:");
                programList.Add("C:\\Program Files\\VM Optimization Tool\\Ressource\\sdelete.exe");
            }
            
            Optimization optWindow = new Optimization(programList.ToArray(), commandsList.ToArray(), path);
            optWindow.Show();
            Close();
        }

        private void AbortButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void CheckCleanMgr_Checked(object sender, RoutedEventArgs e)
        {
            if (radioButton1 != null && (bool)checkCleanMgr.IsChecked)
            {
                radioButton1.IsEnabled = true;
                radioButton2.IsEnabled = true;
                radioButton3.IsEnabled = true;
            }
            else if (radioButton1 != null && !(bool)checkCleanMgr.IsChecked)
            {
                radioButton1.IsEnabled = false;
                radioButton2.IsEnabled = false;
                radioButton3.IsEnabled = false;
            }
        }
    }
}
