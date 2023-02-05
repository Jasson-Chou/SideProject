using ADCMonitor.Model;
using ADCMonitor.ViewModel;
using CommonServiceLocator;
using GUIWaveform;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using Telerik.Windows.Controls;
using static GUIWaveform.DrawingWaveformContext;

namespace ADCMonitor.View.Monitor
{
    /// <summary>
    /// Interaction logic for ADCMonitorView.xaml
    /// </summary>
    public partial class ADCMonitorView
    {
        public ADCMonitorView()
        {
            InitializeComponent();
            ((App)(App.Current)).WaveformServiceModel.DrawingImage = this.waveformImage;
        }

        private void Page_Initialized(object sender, EventArgs e)
        {
            
        }

        private void WaveformBorder_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"[Border Size W * H]: {e.NewSize.Width} * {e.NewSize.Height}");
            System.Diagnostics.Debug.WriteLine($"[ Image Size W * H]: {waveformImage.ActualWidth} * {waveformImage.ActualHeight}");

            ((App)(App.Current)).WaveformServiceModel.ImageRender(e.NewSize);
        }

        public void Close()
        {
            ((App)(App.Current)).WaveformServiceModel.DrawingImage = null;
        }

        private void waveformBorder_MouseEnter(object sender, MouseEventArgs e)
        {
            ((App)(App.Current)).WaveformServiceModel.ShowMouseCursor = true;
        }

        private void waveformBorder_MouseLeave(object sender, MouseEventArgs e)
        {
            ((App)(App.Current)).WaveformServiceModel.ShowMouseCursor = false;
        }

        private void waveformBorder_MouseMove(object sender, MouseEventArgs e)
        {
            var point = e.GetPosition(sender as Border);
            ((App)(App.Current)).WaveformServiceModel.ImageRender(point);
        }

    }
}
