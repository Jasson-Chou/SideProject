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
            InitializeWaveformImage();
        }

        private void InitializeWaveformImage()
        {
            DrawingConfiguration_OnUpdateMinImageSize(DrawingConfiguration.MinMW, DrawingConfiguration.MinMH);
        }

        private void SubscriptADCVMEvent()
        {
            ADCMonitorVM.OnRender += ADCMonitorVM_OnRender;
            DrawingConfiguration.OnUpdateMinImageSize += DrawingConfiguration_OnUpdateMinImageSize;
        }

        private void DrawingConfiguration_OnUpdateMinImageSize(double MinWidth, double MinHeight)
        {
            this.waveformImage.MinWidth = MinWidth;
            this.waveformImage.MinHeight = MinHeight;

            this.waveformBorder.MinWidth = MinWidth;
            this.waveformBorder.MinHeight = MinHeight;
        }

        private void UnsubscriptADCVMEvent()
        {
            ADCMonitorVM.OnRender -= ADCMonitorVM_OnRender;
            DrawingConfiguration.OnUpdateMinImageSize -= DrawingConfiguration_OnUpdateMinImageSize;
        }

        private void ADCMonitorVM_OnRender(DrawingWaveformContext.ERenderType eRenderType)
        {
            DrawingWaveformContext.Render(waveformImage, eRenderType);
        }

        private void Page_Initialized(object sender, EventArgs e)
        {
            
        }

        private void WaveformBorder_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Debug.WriteLine($"[Border Size W * H]: {e.NewSize.Width} * {e.NewSize.Height}");
            Debug.WriteLine($"[ Image Size W * H]: {waveformImage.ActualWidth} * {waveformImage.ActualHeight}");
            
            DrawingConfiguration.MW = e.NewSize.Width;

            DrawingConfiguration.MH = e.NewSize.Height;

            DrawingWaveformContext.Render(waveformImage, DrawingWaveformContext.ERenderType.All);
            
        }

        public void Close()
        {
            UnsubscriptADCVMEvent();
        }
    }
}
