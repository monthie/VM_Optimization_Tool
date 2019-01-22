using System;
using System.Reflection;
using System.Windows;
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

namespace VM_Optimization_Tool
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string updateServerUrl = "https://raw.githubusercontent.com/monthie/VM_Optimization_Tool/master/UpdateXML/UpdateXML.xml";
        public string ApplicationName
        {
            get { return "VM Optimization Tool"; }
        }

        public string ApplicationID
        {
            get { return "VM Optimization Tool"; }
        }

        public Assembly ApplicationAssembly
        {
            get { return Assembly.GetExecutingAssembly(); }
        }
        public System.Drawing.Icon ApplicationIcon
        {
            get { return ApplicationIcon; }
        }

        public Uri UpdateXmlLocation
        {
            get { return new Uri(""); }
        }

        public MainWindow()
        {
            InitializeComponent();
            string mainFrameTitle = getOSInfo();
            Title = Title + " " + mainFrameTitle;
            try
            {
            ToolUpdateXml[] updateInfo = ToolUpdateXml.Parse(new Uri(updateServerUrl));
                if (updateInfo[0].IsNewerThan(ApplicationAssembly.GetName().Version))
                {
                    UpdateAvailableForm updateAvailableForm = new UpdateAvailableForm(updateInfo[0]);
                    updateAvailableForm.Topmost = true;
                    updateAvailableForm.Show();
                }
            }
            catch(Exception e) {
                LogWriter.LogWrite(e.Message);
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (ToolUpdateXml.ExistsOnServer(new Uri(updateServerUrl))) {
                ToolUpdateXml[] updateInfo = ToolUpdateXml.Parse(new Uri(updateServerUrl));
                if (updateInfo[0].IsNewerThan(ApplicationAssembly.GetName().Version))
                {
                    UpdateAvailableForm updateAvailableForm = new UpdateAvailableForm(updateInfo[0]);
                    updateAvailableForm.Topmost = true;
                    updateAvailableForm.Show();
                } else
                {
                    UpdateAvailableForm updateAvailableForm = new UpdateAvailableForm(updateInfo[0]);
                    updateAvailableForm.updateButton.IsEnabled = false;
                    updateAvailableForm.label.Content = "Software is up to date!";
                    updateAvailableForm.Topmost = true;
                    updateAvailableForm.Show();
                }
            }
            else
            {
                ToolUpdateXml noInternet = new ToolUpdateXml(ApplicationAssembly.GetName().Version, null, "", "", "", "");
                UpdateAvailableForm updateAvailableForm = new UpdateAvailableForm(noInternet);
                updateAvailableForm.updateButton.IsEnabled = false;
                updateAvailableForm.label.Content = "No internet connection!";
                updateAvailableForm.Topmost = true;
                updateAvailableForm.Show();
            }
        }

        string getOSInfo()
        {
            //Get Operating system information.
            OperatingSystem os = Environment.OSVersion;
            //Get version information about the os.
            Version vs = os.Version;

            //Variable to hold our return value
            string operatingSystem = "";

            if (os.Platform == PlatformID.Win32Windows)
            {
                //This is a pre-NT version of Windows
                switch (vs.Minor)
                {
                    case 0:
                        operatingSystem = "95";
                        break;
                    case 10:
                        if (vs.Revision.ToString() == "2222A")
                            operatingSystem = "98SE";
                        else
                            operatingSystem = "98";
                        break;
                    case 90:
                        operatingSystem = "Me";
                        break;
                    default:
                        break;
                }
            }
            else if (os.Platform == PlatformID.Win32NT)
            {
                switch (vs.Major)
                {
                    case 3:
                        operatingSystem = "NT 3.51";
                        break;
                    case 4:
                        operatingSystem = "NT 4.0";
                        break;
                    case 5:
                        if (vs.Minor == 0)
                            operatingSystem = "2000";
                        else
                            operatingSystem = "XP";
                        break;
                    case 6:
                        if (vs.Minor == 0)
                            operatingSystem = "Vista";
                        else if (vs.Minor == 1)
                            operatingSystem = "7";
                        else if (vs.Minor == 2)
                            operatingSystem = "8";
                        else
                            operatingSystem = "8.1";
                        break;
                    case 10:
                        operatingSystem = "10";
                        break;
                    default:
                        break;
                }
            }
            //Make sure we actually got something in our OS check
            //We don't want to just return " Service Pack 2" or " 32-bit"
            //That information is useless without the OS version.
            if (operatingSystem != "")
            {
                //Got something.  Let's prepend "Windows" and get more info.
                operatingSystem = "Windows " + operatingSystem;
                //See if there's a service pack installed.
                if (os.ServicePack != "")
                {
                    //Append it to the OS name.  i.e. "Windows XP Service Pack 3"
                    operatingSystem += " " + os.ServicePack;
                }
                //Append the OS architecture.  i.e. "Windows XP Service Pack 3 32-bit"
                //operatingSystem += " " + getOSArchitecture().ToString() + "-bit";
            }
            //Return the information we've gathered.
            return operatingSystem;
        }

        private void WinUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (SetWinServices.DisableWinUpdates())
            {
                Updates.Content = "Enable Windows Updates";
            }
        }

        private void Info_Click(object sender, RoutedEventArgs e)
        {
            VersionInfo versionInfo = new VersionInfo(ApplicationAssembly.GetName().Version.ToString());
            versionInfo.Show();
        }

        private void Optimization_Click(object sender, RoutedEventArgs e)
        {
            /* Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
             dlg.DefaultExt = ".xml";
             dlg.Filter = "XML Files (*.xml)|*.xml";


             // Display OpenFileDialog by calling ShowDialog method 
             bool? result = dlg.ShowDialog();
             if (result == true)
             {
                 // Open document 
                 string filename = dlg.FileName;
                 SelectOptimization selOpti = new SelectOptimization(filename);
                 selOpti.Show();
             }*/
            SelectOptimization selOpti = new SelectOptimization();
            selOpti.Show();
        }
    }
}
