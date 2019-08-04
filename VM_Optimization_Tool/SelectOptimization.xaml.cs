using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.ComponentModel;
using System.IO;
using System.Xml;
using System.Collections.Generic;

namespace VM_Optimization_Tool
{

    /// <summary>
    /// Interaktionslogik für SelectOptimization.xaml
    /// </summary>
    public partial class SelectOptimization : Window
    {
        private XmlDataProvider xmlData;
        private string pathToXML;
        private BackgroundWorker bgWorker;
        private string pathToTemp;

        public SelectOptimization()
        {
            InitializeComponent();

            //bgWorker process for slow cmd processes
            bgWorker = new BackgroundWorker();
            bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork);
            bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted);
            bgWorker.ProgressChanged += new ProgressChangedEventHandler(bgWorker_ProgressChanged);
            bgWorker.WorkerSupportsCancellation = true;
            bgWorker.WorkerReportsProgress = true;

            xmlData = new XmlDataProvider();
            xmlData = TryFindResource("xmlData") as XmlDataProvider;
            try
            {
                pathToXML = SelectFile();
                pathToXML = CreateTempXMLFile(pathToXML);
            } catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            if (pathToXML == null) {
                
            } else {
                xmlData.Source = new Uri(pathToXML); 
            }  
        }
        /// <summary>
        /// function to return selected filepath
        /// </summary>
        /// <returns>filepath</returns>
        private string SelectFile()
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".xml";
            dlg.Filter = "XML Files (*.xml)|*.xml";

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name
            if (result == true)
            {
                return dlg.FileName;
            }
            else
            { 
                // if nothing selected disable all buttons
                btnLoad.IsEnabled = false;
                btnAbort.IsEnabled = false;
                return null;
            }
        }

        /// <summary>
        /// Create a temp file of XML. Escape it and modify it for functional treeview
        /// </summary>
        /// <param name="pathToOriginal"></param>
        private string CreateTempXMLFile(string pathToOriginal)
        {
            try
            {
                pathToTemp = Path.GetTempFileName();
                File.WriteAllText(pathToTemp, (File.ReadAllText(pathToOriginal).Replace("&", "&amp;").Replace("&#34;", "&quot;").Replace("'", "&apos;")));
                XmlDocument doc = new XmlDocument();
                doc.Load(pathToTemp);

                XmlNodeList firstLevel = doc.DocumentElement.SelectNodes("/sequence/group/group");
                foreach (XmlNode secoundLevel in firstLevel)
                {
                    XmlAttribute newAttr = doc.CreateAttribute("defaultSelected");
                    newAttr.Value = "false";
                    secoundLevel.Attributes.Append(newAttr);
                }
                doc.Save(pathToTemp);
                return pathToTemp;
            } catch(Exception e)
            {
                return null;
            }
            
        }
       
        /// <summary>
        /// function to start optimization
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loadXML_Click(object sender, RoutedEventArgs e)
        {
            btnLoad.IsEnabled = false;
            bgWorker.RunWorkerAsync();
        }
        /// <summary>
        /// Abort function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void abort_Click(object sender, RoutedEventArgs e)
        {
            if (bgWorker.WorkerSupportsCancellation == true)
            {
                // Cancel the asynchronous operation.
                bgWorker.CancelAsync();
                btnLoad.IsEnabled = true;
            }
            Close();
        }
        /// <summary>
        /// function to save checkbox changes in xml file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBoxChanged(object sender, RoutedEventArgs e)
        {
            ((CheckBox)sender).GetBindingExpression(CheckBox.IsCheckedProperty).UpdateTarget();
            xmlData.Document.Save(xmlData.Source.AbsolutePath.ToString().Replace("%20", " "));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            xmlData.Document.Save(xmlData.Source.AbsolutePath.ToString().Replace("%20", " "));
            XmlParser[] xmlParsers = XmlParser.Parser(xmlData.Source);
            ApplyChanges.Changes(xmlParsers, bgWorker);
            if (bgWorker.CancellationPending == true)
            {
                e.Cancel = true;
                return;
            }
        }

        /// <summary>
        /// Check after completed job whether successful or not
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        }

        /// <summary>
        /// Update progressbar async
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
        }

        private void ClosingWindow(object sender, CancelEventArgs args)
        {
            try
            {
                File.Delete(pathToTemp);
            }catch(Exception e)
            {

            }
        }
    }
}