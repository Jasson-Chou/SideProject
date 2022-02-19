using GalaSoft.MvvmLight;
using GUIWaveform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GUIWaveform.DrawingWaveformContext;

namespace ADCMonitor.ViewModel
{
    public delegate void OnRenderHandler(ERenderType eRenderType);
    public class ADCMonitorViewModel:ViewModelBase
    {
        public DrawingWaveformContext DrawingWaveformContext { get; }
        public event OnRenderHandler OnRender = null;
        public ADCMonitorViewModel()
        {
            DrawingWaveformContext = new DrawingWaveformContext();
        }

        public int XOffset
        {
            get { return DrawingWaveformContext.DrawingConfig.XOffset; }
            set
            {
                DrawingWaveformContext.DrawingConfig.XOffset = value;
                OnRender?.Invoke(ERenderType.Horizontal);
                base.RaisePropertyChanged();
            }
        }


    }
}
