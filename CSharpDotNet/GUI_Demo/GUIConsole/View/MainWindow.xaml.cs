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

namespace GUIConsole.View
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
            SetAllControlsKeyDown(this.mainWindow);
        }

        private void SetAllControlsKeyDown(DependencyObject dependencyObject)
        {
            var control = dependencyObject as Control;
            if (control != null)
            {
                control.PreviewKeyDown += Control_PreviewKeyDown;
                control.KeyDown += Control_KeyDown;
                foreach(var child in GetChilds(control))
                {
                    SetAllControlsKeyDown(child);
                }
            }
        }

        private IEnumerable<DependencyObject> GetChilds(DependencyObject dependencyObject)
        {
            var control = dependencyObject as Control;
            int cnt = VisualTreeHelper.GetChildrenCount(control);
            for (int idx = 0; idx < cnt; idx++)
            {
                var child = VisualTreeHelper.GetChild(control, idx);
                yield return child;
            }
            yield break;
        }

        private void Control_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            this.inputCMDTextBox.Focus();
        }

        private void Control_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void AppCloseBtn_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        private void AppZoonInOutBtn_Click(object sender, RoutedEventArgs e)
        {
            switch(WindowState)
            {
                case WindowState.Maximized:
                    WindowState = WindowState.Normal;
                    break;
                case WindowState.Normal:
                    WindowState = WindowState.Maximized;
                    break;
            }
        }

        private void AppHideBtn_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void titleBorder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                this.mainWindow.DragMove();
        }
       
    }
}
