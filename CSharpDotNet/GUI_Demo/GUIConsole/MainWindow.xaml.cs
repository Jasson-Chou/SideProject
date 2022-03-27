using System;
using System.Collections;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GUIConsole
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Initialized(object sender, EventArgs e)
        {
            this.mainWindow.PreviewKeyDown += MainWindow_PreviewKeyDown;
            int cnt = VisualTreeHelper.GetChildrenCount(this.mainWindow);
            for(int idx = 0; idx < cnt; idx++)
            {
                var child = VisualTreeHelper.GetChild(this.mainWindow, idx);

            }

        }
        

        private void MainWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
