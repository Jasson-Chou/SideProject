using GalaSoft.MvvmLight;
using GUIWaveform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADCMonitor.ViewModel
{
    public class ADCMonitorViewModel:ViewModelBase
    {
        public DrawingWaveformContext DrawingWaveformContext { get; }
        public event Action OnRender = null;
        public ADCMonitorViewModel()
        {
            DrawingWaveformContext = new DrawingWaveformContext();
        }


    }
}
