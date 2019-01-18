﻿using System;
using System.Windows;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Xml.Linq;

namespace VM_Optimization_Tool
{

    /// <summary>
    /// Interaktionslogik für SelectOptimization.xaml
    /// </summary>
    public partial class SelectOptimization : Window
    {
        private XmlDataProvider xmlData;
        private string pathToXML;
        public SelectOptimization()
        {
            InitializeComponent();
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
            xmlData.Document.Save(xmlData.Source.AbsolutePath);
            XmlParser[] xmlParsers = XmlParser.Parser(xmlData.Source);
            ApplyChanges.Changes(xmlParsers);
        }
        /// <summary>
        /// Abort function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void abort_Click(object sender, RoutedEventArgs e)
        {
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
            xmlData.Document.Save(xmlData.Source.AbsolutePath);
        }
    }
}