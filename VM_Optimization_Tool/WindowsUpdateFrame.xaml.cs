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
using WUApiLib;

namespace VM_Optimization_Tool
{
    /// <summary>
    /// Interaktionslogik für WindowsUpdateFrame.xaml
    /// </summary>
    public partial class WindowsUpdateFrame : Window
    {
        public WindowsUpdateFrame()
        {
            InitializeComponent();
            CheckForUpdates();
        }

        public void CheckForUpdates()
        {
            UpdateSession uSession = new UpdateSession();
            IUpdateSearcher uSearcher = uSession.CreateUpdateSearcher();
            uSearcher.Online = true;
            try
            {
                ISearchResult sResult = uSearcher.Search("IsInstalled=1 And IsHidden=0");
                textBox1.Text = "Found " + sResult.Updates.Count + " updates" + Environment.NewLine;
                foreach (IUpdate update in sResult.Updates)
                {
                    textBox1.AppendText(update.Title + Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                textBox1.Text = "Something went wrong: " + ex.Message;
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ProgressWindow progressWindow = new ProgressWindow();
            progressWindow.Show();
        }
    }
}
