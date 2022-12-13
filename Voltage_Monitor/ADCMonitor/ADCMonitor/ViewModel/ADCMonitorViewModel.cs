using ADCMonitor.Model;
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
        public ADCMonitorViewModel()
        {
            var Instance = DrawingWaveformServiceModel.Instance;
            Instance.GenerateData();
        }

        public int HornizontalMaximum
        {
            get => DrawingWaveformServiceModel.Instance.HornizontalMaximum;
        }

        public int HornizontalOffset
        {
            get { return DrawingWaveformServiceModel.Instance.HornizontalOffset; }
            set
            {
                var Instance = DrawingWaveformServiceModel.Instance;
                Instance.HornizontalOffset = value;
                Instance.ImageRender(ERenderType.Horizontal);
                base.RaisePropertyChanged();
            }
        }

        public int MinTUP => DrawingWaveformServiceModel.Instance.TimingUnitPixelMinimum;
        public int MaxTUP => DrawingWaveformServiceModel.Instance.TimingUnitPixelMaximum;

        public int TUPValue
        {
            get => DrawingWaveformServiceModel.Instance.TimingUnitPixel;
            set
            {
                var Instance = DrawingWaveformServiceModel.Instance;
                Instance.TimingUnitPixel = value;
                Instance.ImageRender(ERenderType.All);
                base.RaisePropertyChanged();
                base.RaisePropertyChanged(() => HornizontalMaximum);
            }
        }

        public double ImageWidthMinimum => DrawingWaveformServiceModel.Instance.ImageMinimumSize.Width;  
        
        public double ImageHeightMinimum => DrawingWaveformServiceModel.Instance.ImageMinimumSize.Height;
    }

    public static class DrawingWaveformServiceExtension
    {
        public static void GenerateData(this DrawingWaveformServiceModel model)
        {
            var config = model.Configuration;
            var ADCResolution = config.ADCResolution;
            var adcItemsSource = config.ADCItemsSource;
            for (int index = 0; index < 2000; index++)
            {
                var randomValue = (ushort)(new Random(Guid.NewGuid().GetHashCode())).Next((int)Math.Pow(2, ADCResolution) - 1);
                adcItemsSource.Add(new ACDItem(index, randomValue));
            }
        }
    }
}
