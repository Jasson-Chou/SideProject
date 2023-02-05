using ADCMonitor.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
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
    public class ADCMonitorViewModel : ViewModelBase
    {
        public ADCMonitorViewModel()
        {
            ((App)(App.Current)).WaveformServiceModel.GenerateData();
        }

        public int HornizontalMaximum
        {
            get => ((App)(App.Current)).WaveformServiceModel.HornizontalMaximum;
        }

        public int HornizontalOffset
        {
            get { return ((App)(App.Current)).WaveformServiceModel.HornizontalOffset; }
            set
            {
                var Instance = ((App)(App.Current)).WaveformServiceModel;
                Instance.HornizontalOffset = value;
                Instance.ImageRender(ERenderType.Horizontal);
                base.RaisePropertyChanged();
            }
        }

        public int MinTUP => ((App)(App.Current)).WaveformServiceModel.TimingUnitPixelMinimum;
        public int MaxTUP => ((App)(App.Current)).WaveformServiceModel.TimingUnitPixelMaximum;

        public int TUPValue
        {
            get => ((App)(App.Current)).WaveformServiceModel.TimingUnitPixel;
            set
            {
                var Instance = ((App)(App.Current)).WaveformServiceModel;
                Instance.TimingUnitPixel = value;
                Instance.ImageRender(ERenderType.All);
                base.RaisePropertyChanged();
                base.RaisePropertyChanged(() => HornizontalMaximum);
            }
        }

        public double ImageWidthMinimum => ((App)(App.Current)).WaveformServiceModel.ImageMinimumSize.Width;

        public double ImageHeightMinimum => ((App)(App.Current)).WaveformServiceModel.ImageMinimumSize.Height;

        public RelayCommand RefreshCommand => new RelayCommand(() =>
        {
            ((App)(App.Current)).WaveformServiceModel.ImageRender(ERenderType.All);
        });
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
                adcItemsSource.Add(new ADCItem(index, randomValue));
            }
        }
    }
}
