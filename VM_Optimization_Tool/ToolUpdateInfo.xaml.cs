using System;
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

namespace VM_Optimization_Tool
{
    /// <summary>
    /// Interaktionslogik für ToolUpdateInfo.xaml
    /// </summary>
    public partial class ToolUpdateInfo : Window
    {
        public ToolUpdateInfo(string applicationName, string description, string version)
        {
            InitializeComponent();
            Title = applicationName+ " Update Info";
            textBox.Document.Blocks.Clear();
            textBox.Document.Blocks.Add(new Paragraph(new Run(description)));
            versionLabel.Content = "Current Version: "+version;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
