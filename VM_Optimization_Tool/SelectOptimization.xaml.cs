using System;
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
        public SelectOptimization()
        {
            InitializeComponent();
            
            
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

        private void loadXML_Click(object sender, RoutedEventArgs e)
        {

        }

        private void abort_Click(object sender, RoutedEventArgs e)
        {

        }
        private void CheckBoxChanged(object sender, RoutedEventArgs e)
        {
            ((CheckBox)sender).GetBindingExpression(CheckBox.IsCheckedProperty).UpdateTarget();

            XmlDataProvider test = new XmlDataProvider();
            test = TryFindResource("xmlData") as XmlDataProvider;
            test.Document.Save(test.Source.AbsolutePath);
            // MessageBox.Show(e.OriginalSource.ToString());
        }
    }
}