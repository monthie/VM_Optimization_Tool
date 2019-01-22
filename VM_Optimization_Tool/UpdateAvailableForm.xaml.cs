using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
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
        /// <summary>
        /// The update version #
        /// </summary>
        public Version Version { get; set; }

        /// <summary>
        /// The location of the update binary
        /// </summary>
        public Uri Uri { get; set; }

        /// <summary>
        /// The file path of the binary
        /// for use on local computer
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// The MD5 of the update's binary
        /// </summary>
        public string MD5 { get; set; }

        /// <summary>
        /// The update's description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The arguments to pass to the updated application on startup
        /// </summary>
        public string LaunchArgs { get; set; }

        private ToolUpdateXml updateInfo;
        public UpdateAvailableForm()
        {
            InitializeComponent();
            //textbox.Text = "Version: " +updateInfo.Version +"\n Change Log "+ updateInfo.Description;
        }
        public void UpdateDescription()
        {
            textbox.Text = "Version: " + Version + "\n Change Log " + Description;
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
