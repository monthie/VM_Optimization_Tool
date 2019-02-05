using System.Reflection;
using System.Windows;

namespace VM_Optimization_Tool
{
    /// <summary>
    /// Interaktionslogik für VersionInfo.xaml
    /// </summary>
    public partial class VersionInfo : Window
    {
        public VersionInfo()
        {
            InitializeComponent();
            versionLabel.Content = "Version: " + ApplicationAssembly.GetName().Version;
            applicationLabel.Content = ApplicationAssembly.GetName().Name;
        }
        public Assembly ApplicationAssembly
        {
            get { return Assembly.GetExecutingAssembly(); }
        }
    }
}
