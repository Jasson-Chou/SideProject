using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Diagnostics;
using GlobalPath;
using System.Windows;
using static GUIWaveform.DrawingWaveformContext;
using System.Windows.Media;

namespace GUIWaveform
{
    public delegate void OnUpdateImageMinSizeHandler(double MinWidth, double MinHeight);

    public class DrawingConfiguration
    {

        /// <summary>
        /// Raising Handler When Saving and Loading Event Capture
        /// </summary>
        public event OnUpdateImageMinSizeHandler OnUpdateMinImageSize = null;

        private ConfigurationManager ConfigurationManager { get; }

        public ADCItemList ADCItemsSource { get; }

        public CursorItemList CursorItemsSource { get; }

        public DrawingConfiguration()
        {
            ADCItemsSource = new ADCItemList();
            CursorItemsSource = new CursorItemList();
            ConfigurationManager = new ConfigurationManager(SystemFolderFileName.GUIWaveformConfiguration);
            Load();
        }

        public int XOffset { get; set; }

        public double MH { get; set; }
        public double MW { get; set; }
        
        /// <summary>
        /// Voltage Bar Width
        /// </summary>
        public double VBW { get; set; }

        /// <summary>
        /// Timer Bar Height
        /// </summary>
        public double TBH { get; set; }

        /// <summary>
        /// Mimimum Pixel Per Bit
        /// </summary>
        public double MinPPB { get; set; }

        /// <summary>
        /// Pixel Per Bit
        /// </summary>
        public double PPB { get; set; }

        /// <summary>
        /// Minimum Time Unit Pixel
        /// </summary>
        public int MinTUP { get; set; }

        /// <summary>
        /// Maximum Time Unit Pixel
        /// </summary>
        public int MaxTUP { get; set; }

        /// <summary>
        /// Time Unit Pixel
        /// </summary>
        public int TUP { get; set; }

        public double xdpi { get; set; }

        public double ydpi { get; set; }


        /// <summary>
        /// Origin Point
        /// </summary>
        internal Point OP { get; private set; }

        /// <summary>
        /// Voltage Bar Length
        /// </summary>
        internal double VBL { get; private set; }

        /// <summary>
        /// Time Bar Length
        /// </summary>
        internal double TBL { get; private set; }

        /// <summary>
        /// Voltage Bar Scale
        /// </summary>
        public double VBS { get; private set; }

        /// <summary>
        /// First Point Index
        /// </summary>
        internal int FPI { get; private set; }

        /// <summary>
        /// Last Point Index
        /// </summary>
        internal int LPI { get; private set; }

        /// <summary>
        /// Minimum Map Height
        /// </summary>
        public double MinMH { get; private set; }

        /// <summary>
        /// Minimum Map Width
        /// </summary>
        public double MinMW { get; private set; }


        /// <summary>
        /// MCU ADC Dectect Maximum Voltage
        /// </summary>
        public double ADCMaxVolt { get; set; }

        /// <summary>
        /// MCU ADC Dectect Minimum Voltage
        /// </summary>
        public double ADCMinVolt { get; set; }

        /// <summary>
        /// MCU ADC Resolution
        /// </summary>
        public ushort ADCResolution { get; set; }

        internal double ADCToVoltage(ushort adcValue)
        {
            return (ADCMaxVolt - ADCMinVolt) / (Math.Pow(2, ADCResolution) - 1.0d) * adcValue;
        }

        internal void RefreshData()
        {
            double op_x = VBW;
            double op_y = MH - TBH;
            OP = new Point(op_x, op_y);

            VBL = MH - 2 * TBH;
            TBL = MW - 2 * VBW;

            int fpi = XOffset / TUP;
            if (ADCItemsSource.Count > fpi)
                FPI = fpi;
            else
                FPI = ADCItemsSource.Count - 1;

            int lpi = ((int)TBL + XOffset) / TUP;
            if (ADCItemsSource.Count > lpi)
                LPI = lpi;
            else
                LPI = ADCItemsSource.Count - 1;

            double valueSize = Math.Pow(2, ADCResolution) - 1.0d;

            PPB = VBL * VBS / valueSize;

            if (PPB < MinPPB)
                PPB = MinPPB;


#if DEBUG
            Debug.WriteLine("----------------Refresh Data---------------");
            Debug.WriteLine($"{nameof(OP)} = {OP}");
            Debug.WriteLine($"{nameof(VBL)} = {VBL}");
            Debug.WriteLine($"{nameof(TBL)} = {TBL}");
            Debug.WriteLine($"{nameof(FPI)} = {FPI}");
            Debug.WriteLine($"{nameof(LPI)} = {LPI}");
            Debug.WriteLine("-------------------------------------------");
#endif
        }

        

        public bool Save()
        {
            try
            {
                ConfigurationManager.SetViewerPropertyItem(nameof(VBW), VBW.ToString());
                ConfigurationManager.SetViewerPropertyItem(nameof(TBH), TBH.ToString());
                ConfigurationManager.SetViewerPropertyItem(nameof(MinPPB), MinPPB.ToString());
                ConfigurationManager.SetViewerPropertyItem(nameof(MinTUP), MinTUP.ToString());
                ConfigurationManager.SetViewerPropertyItem(nameof(MaxTUP), MaxTUP.ToString());
                ConfigurationManager.SetViewerPropertyItem(nameof(VBS), VBS.ToString());
                ConfigurationManager.SetViewerPropertyItem(nameof(TUP), TUP.ToString());
                ConfigurationManager.SetViewerPropertyItem(nameof(xdpi), xdpi.ToString());
                ConfigurationManager.SetViewerPropertyItem(nameof(ydpi), ydpi.ToString());

                ConfigurationManager.SetMCUPropertyItem(nameof(ADCMaxVolt), ADCMaxVolt.ToString());
                ConfigurationManager.SetMCUPropertyItem(nameof(ADCMinVolt), ADCMinVolt.ToString());
                ConfigurationManager.SetMCUPropertyItem(nameof(ADCResolution), ADCResolution.ToString());
            }
            catch(Exception ex)
            {
#if DEBUG
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine(ex.Message);
#endif
                return false;
            }
            RaiseOnUpdateImageMinSize();
            return true;
        }

        private bool Load()
        {
            try
            {
                VBW = double.TryParse(ConfigurationManager.GetViewerPropertyItemValue(nameof(VBW)), out double _VBW) ? _VBW : throw new Exception($"{nameof(VBW)} Parsing Error");
                TBH = double.TryParse(ConfigurationManager.GetViewerPropertyItemValue(nameof(TBH)), out double _TBH) ? _TBH : throw new Exception($"{nameof(TBH)} Parsing Error");
                MinPPB = double.TryParse(ConfigurationManager.GetViewerPropertyItemValue(nameof(MinPPB)), out double _MinPPB) ? _MinPPB : throw new Exception($"{nameof(MinPPB)} Parsing Error");
                MinTUP = int.TryParse(ConfigurationManager.GetViewerPropertyItemValue(nameof(MinTUP)), out int _MinTUP) ? _MinTUP : throw new Exception($"{nameof(MinTUP)} Parsing Error");
                MaxTUP = int.TryParse(ConfigurationManager.GetViewerPropertyItemValue(nameof(MaxTUP)), out int _MaxTUP) ? _MaxTUP : throw new Exception($"{nameof(MaxTUP)} Parsing Error");
                TUP = int.TryParse(ConfigurationManager.GetViewerPropertyItemValue(nameof(TUP)), out int _TUP) ? _TUP : throw new Exception($"{nameof(TUP)} Parsing Error");
                VBS = double.TryParse(ConfigurationManager.GetViewerPropertyItemValue(nameof(VBS)), out double _VBS) ? _VBS : throw new Exception($"{nameof(VBS)} Parsing Error");
                xdpi = double.TryParse(ConfigurationManager.GetViewerPropertyItemValue(nameof(xdpi)), out double _xdpi) ? _xdpi : throw new Exception($"{nameof(xdpi)} Parsing Error");
                ydpi = double.TryParse(ConfigurationManager.GetViewerPropertyItemValue(nameof(ydpi)), out double _ydpi) ? _ydpi : throw new Exception($"{nameof(ydpi)} Parsing Error");

                ADCMaxVolt = double.TryParse(ConfigurationManager.GetMCUPropertyItemValue(nameof(ADCMaxVolt)), out double _ADCMaxVolt) ? _ADCMaxVolt : throw new Exception($"{nameof(ADCMaxVolt)} Parsing Error");
                ADCMinVolt = double.TryParse(ConfigurationManager.GetMCUPropertyItemValue(nameof(ADCMinVolt)), out double _ADCMinVolt) ? _ADCMinVolt : throw new Exception($"{nameof(ADCMinVolt)} Parsing Error");
                ADCResolution = ushort.TryParse(ConfigurationManager.GetMCUPropertyItemValue(nameof(ADCResolution)), out ushort _ADCResolution) ? _ADCResolution : throw new Exception($"{nameof(ADCResolution)} Parsing Error");
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine(ex.Message);
#endif
                return false;
            }
            RaiseOnUpdateImageMinSize();
            return true;
        }

        private void RaiseOnUpdateImageMinSize()
        {
            double valueSize = Math.Pow(2, ADCResolution) - 1.0d;

            MinMH = valueSize * MinPPB / VBS + 2 * TBH;

            MinMW = VBW * 2 + MinMH;

            OnUpdateMinImageSize?.Invoke(MinMW, MinMH);
        }

        public override string ToString()
        {
            return $"{ConfigurationManager.ToString()}{Environment.NewLine}{ADCItemsSource?.ToString() ?? $"{nameof(ADCItemsSource)} is Null"}";
        }
    }
}
