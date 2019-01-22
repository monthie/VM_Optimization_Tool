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
    /// Interaktionslogik für UpdateAvailableForm.xaml
    /// </summary>
    public partial class UpdateAvailableForm : Window
    {
        private ToolUpdateXml updateInfo;
        public UpdateAvailableForm(ToolUpdateXml updateInfo)
        {
            this.updateInfo = updateInfo;
            InitializeComponent();
            textbox.Text = "Version: " +updateInfo.Version +"\n Change Log "+ updateInfo.Description;

        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            DownloadForm downloadForm = new DownloadForm(updateInfo.Uri, updateInfo.MD5);
            downloadForm.Show();
            Close();
        }

        private void Abort_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
