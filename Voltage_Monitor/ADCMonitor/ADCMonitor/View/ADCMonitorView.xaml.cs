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

namespace ADCMonitor.View
{
    /// <summary>
    /// Interaction logic for ADCMonitorView.xaml
    /// </summary>
    public partial class ADCMonitorView
    {
        public ADCMonitorViewModel ADCMonitorVM => ServiceLocator.Current.GetInstance<ADCMonitorViewModel>();

        public DrawingConfiguration DrawingConfiguration => ADCMonitorVM.DrawingWaveformContext.DrawingConfig;

        public DrawingWaveformContext DrawingWaveformContext => ADCMonitorVM.DrawingWaveformContext;

        public ADCMonitorView()
        {
            InitializeComponent();
            SubscriptADCVMEvent();
        }

        public void SubscriptADCVMEvent()
        {
            ADCMonitorVM.OnRender += ADCMonitorVM_OnRender;
        }

        public void UnsubscriptADCVMEvent()
        {
            ADCMonitorVM.OnRender -= ADCMonitorVM_OnRender;
        }

        private void ADCMonitorVM_OnRender()
        {
            throw new NotImplementedException();
        }

        private void Page_Initialized(object sender, EventArgs e)
        {
            
        }

        private void WaveformBorder_SizeChanged(object sender, SizeChangedEventArgs e)
        {
#if DEBUG
            Debug.WriteLine($"[Border Size W * H]: {e.NewSize.Width} * {e.NewSize.Height}");
            Debug.WriteLine($"[ Image Size W * H]: {waveformImage.Width} * {waveformImage.Height}");
#endif
            waveformImage.Width = waveformBorder.ActualWidth;

            waveformImage.Height = waveformBorder.ActualHeight;

            DrawingConfiguration.MW = waveformBorder.ActualWidth;

            DrawingConfiguration.MH = waveformBorder.ActualHeight;

            DrawingWaveformContext.Render(waveformImage, DrawingWaveformContext.ERenderType.All);
        }

        public void Close()
        {
            UnsubscriptADCVMEvent();
        }
    }
}
