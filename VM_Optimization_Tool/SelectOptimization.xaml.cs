using System;
using System.Windows;
using System.Xml;
using System.Windows.Controls;
using System.Windows.Data;
using System.ComponentModel;

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
            pathToXML = SelectFile();
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
                return null;
            }
        }

        private void ParseXMLFile(string pathToXML)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(pathToXML);

            XmlNodeList nodes = doc.SelectNodes("sequence[@runOnOs='16']");
            foreach (XmlNode node in nodes)
            {
                // If the node doesn't exist, there is no win 10 xml file
                if (node == null)
                {
                    MessageBoxResult result = MessageBox.Show("Wrong file",
                                          "Confirmation",
                                          MessageBoxButton.OK,
                                          MessageBoxImage.Question);
                    if (result == MessageBoxResult.OK)
                    {
                        Close();
                    }
                }

                // Parse data
                string runOnOs = node.InnerText;
                Console.WriteLine(runOnOs);


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
            }
            btnLoad.IsEnabled = true;
            //Close();
        }
        /// <summary>
        /// function to save checkbox changes in xml file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBoxChanged(object sender, RoutedEventArgs e)
        {
            ((CheckBox)sender).GetBindingExpression(CheckBox.IsCheckedProperty).UpdateTarget();
            xmlData.Document.Save(xmlData.Source.AbsolutePath);
        }
        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            xmlData.Document.Save(xmlData.Source.AbsolutePath);
            XmlParser[] xmlParsers = XmlParser.Parser(xmlData.Source);
            ApplyChanges.Changes(xmlParsers, bgWorker);
            if (bgWorker.CancellationPending == true)
            {
                e.Cancel = true;
                return;
            }
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
        }
        private void bgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
        }
    }
}