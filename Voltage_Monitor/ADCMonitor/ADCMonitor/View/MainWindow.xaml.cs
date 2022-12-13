using ADCMonitor.View.Monitor;
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
using Telerik.Windows.Controls;

namespace ADCMonitor.View
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        private ADCMonitorView adcMonitorView;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void RadNavigationViewItem_Click(object sender, RoutedEventArgs e)
        {
            var content = ((System.Windows.Controls.ContentControl)e.OriginalSource).Content.ToString();
            switch(content)
            {
                case "Monitor":
                    mainFrame.Navigate(adcMonitorView);
                    break;
                default:
                    RadWindow.Alert("incomplete in this version.");
                break;
            }
            e.Handled = true;
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            adcMonitorView = new ADCMonitorView();
            mainFrame.Navigate(adcMonitorView);
        }
    }
}
