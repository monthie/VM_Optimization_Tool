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

namespace VM_Optimization_Tool
{ 

    /// <summary>
    /// Interaktionslogik für SelectOptimization.xaml
    /// </summary>
    public partial class SelectOptimization : Window
    {
        public SelectOptimization(string pathToXML)
        {
            DirectoryInfo test = new DirectoryInfo("C:\\Users\\Du\\source\\repos\\VM_Optimization_Tool\\VM_Optimization_Tool\\bin\\Debug");
            DataContext = GetTree(test);
            //ParseXMLFile(pathToXML);
            InitializeComponent();

        }

        public Node GetTree(DirectoryInfo di)
        {
            Node item = new Node();
            item.Name = di.Name;
            foreach (DirectoryInfo s in di.GetDirectories())
            {
                item.Children.Add(GetTree(s));
            }
            foreach (FileInfo fi in di.GetFiles())
            {
                item.Children.Add(new Node { Name = fi.Name });
            }
            return item;
        }

        private List<string> GetSelectedNames()
        {
            List<string> selectedNames = new List<string>();
            foreach (var item in treeView.Items.OfType<Node>())
                GetSelected(item, ref selectedNames);
            return selectedNames;
        }

        public void GetSelected(Node node, ref List<string> s)
        {
            if (node.IsChecked)
                s.Add(node.Name);

            foreach (Node child in node.Children)
                GetSelected(child, ref s);
        }

        private void ParseXMLFile(string pathToXML)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(pathToXML);

            XmlNodeList nodes = doc.SelectNodes("sequence[@runOnOs='16']");
            foreach (XmlNode node in nodes)
            {
                // If the node doesn't exist, there is no win 10 xml file
                if (node == null) { 
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
    }
}
