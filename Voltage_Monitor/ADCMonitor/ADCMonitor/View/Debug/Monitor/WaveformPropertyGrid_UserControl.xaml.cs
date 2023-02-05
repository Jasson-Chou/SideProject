using ADCMonitor.ViewModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ADCMonitor.View.Debug.Monitor
{
    /// <summary>
    /// PropertyControl_UserControl.xaml 的互動邏輯
    /// </summary>
    public partial class WaveformPropertyGrid_UserControl : UserControl
    {

        public WaveformPropertyGrid_UserControl()
        {
            InitializeComponent();
        }

        private void propertyGrid_AutoGeneratingPropertyDefinition(object sender, Telerik.Windows.Controls.Data.PropertyGrid.AutoGeneratingPropertyDefinitionEventArgs e)
        {
            var displayName = e.PropertyDefinition.DisplayName;
            switch (displayName)
            {
                case "IsInDesignMode": e.Cancel = true; break;
                case "RefreshCommand": e.Cancel = true; break;
            }
        }
    }
}
