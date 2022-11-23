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



            // for testing code
            var config = DrawingWaveformContext.DrawingConfig;
            var ADCResolution = config.ADCResolution;
            var adcItemsSource = config.ADCItemsSource;
            for (int index = 0; index < 2000; index++)
            {
                var randomValue = (ushort)(new Random(Guid.NewGuid().GetHashCode())).Next((int)Math.Pow(2, ADCResolution) - 1);
                adcItemsSource.Add(new ACDItem(index, randomValue));
            }

            XOffsetMax = adcItemsSource.Count * config.TUP;
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

        private int xOffsetMax;

        public int XOffsetMax
        {
            get { return xOffsetMax; }
            set { xOffsetMax = value; base.RaisePropertyChanged(); }
        }

        public int MinTUP => DrawingWaveformContext.DrawingConfig.MinTUP;
        public int MaxTUP => DrawingWaveformContext.DrawingConfig.MaxTUP;

        public int TUPValue
        {
            get => DrawingWaveformContext.DrawingConfig.TUP;
            set
            {
                DrawingWaveformContext.DrawingConfig.TUP = value;
                OnRender.Invoke(ERenderType.Horizontal);
                base.RaisePropertyChanged();
            }
        }


    }
}
