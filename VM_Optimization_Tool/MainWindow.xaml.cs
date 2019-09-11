using System;
using System.Reflection;
using System.Threading;
using System.Windows;



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
            string mainFrameTitle = GetOSInfo();
            Title = Title + " " + mainFrameTitle;
            try
            {
            ToolUpdateXml[] updateInfo = ToolUpdateXml.Parse(new Uri(updateServerUrl));
                if (updateInfo[0].IsNewerThan(ApplicationAssembly.GetName().Version))
                {
                    UpdateAvailableForm updateAvailableForm = new UpdateAvailableForm(updateInfo[0]);
                    updateAvailableForm.Topmost = true;
                    updateAvailableForm.UpdateDescription();
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
            UpdateAvailableForm updateAvailableForm = new UpdateAvailableForm(null); ;
            //Check for Update XML on Server
            if (ToolUpdateXml.ExistsOnServer(new Uri(updateServerUrl))) {
                ToolUpdateXml[] updateInfo = ToolUpdateXml.Parse(new Uri(updateServerUrl));

                updateAvailableForm.updateInfo = updateInfo[0];
                

                if (!updateInfo[0].IsNewerThan(ApplicationAssembly.GetName().Version))
                {
                    updateAvailableForm.updateButton.IsEnabled = false;
                    updateAvailableForm.label.Content = "Software is up to date!";
                    updateAvailableForm.Topmost = true;
                }
            }
            else
            {
                updateAvailableForm.updateButton.IsEnabled = false;
                updateAvailableForm.label.Content = "No internet connection!";
            }
            updateAvailableForm.UpdateDescription();
            updateAvailableForm.Show();
        }

        private string GetOSInfo()
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

        private void windowsUpdatesButton_Click(object sender, RoutedEventArgs e)
        {
            WindowsUpdateForm windowsUpdateFrame = new WindowsUpdateForm();
            windowsUpdateFrame.Show();
        }

        private void Info_Click(object sender, RoutedEventArgs e)
        {
            VersionInfo versionInfo = new VersionInfo();
            versionInfo.Show();
        }

        private void Optimization_Click(object sender, RoutedEventArgs e)
        {
            SelectOptimization selOpti = new SelectOptimization();
            selOpti.Show();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void OptimizationButton_Click(object sender, RoutedEventArgs e)
        {
            PreOptimizationWindow preOptimizationWindow = new PreOptimizationWindow();
            preOptimizationWindow.Show();
        }
    }
}
