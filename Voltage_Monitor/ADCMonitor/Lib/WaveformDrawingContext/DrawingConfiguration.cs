using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Diagnostics;
using GlobalPath;

namespace GUIWaveform
{
    public class DrawingConfiguration
    {
        private ConfigurationManager ConfigurationManager { get; }
        public DrawingConfiguration()
        {
            ConfigurationManager = new ConfigurationManager(SystemFolderFileName.GUIWaveformConfiguration);
            Load();
        }

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
        /// Zero Voltage Height
        /// </summary>
        public double ZVH { get; set; }

        /// <summary>
        /// Time Unit Pixel
        /// </summary>
        public double TUP { get; set; }

        public double xdpi { get; set; }

        public double ydpi { get; set; }

        public bool Save()
        {
            try
            {
                
            }
            catch(Exception ex)
            {
                Debug.Write(ex.Message);
                return false;
            }
            return true;
        }

        private bool Load()
        {
            try
            {
                VBW = double.TryParse(ConfigurationManager.GetPropertyItemValue(nameof(VBW)), out double _VBW) ? _VBW : throw new Exception($"{nameof(VBW)} Parseing Error");
                TBH = double.TryParse(ConfigurationManager.GetPropertyItemValue(nameof(TBH)), out double _TBH) ? _TBH : throw new Exception($"{nameof(TBH)} Parseing Error");
                ZVH = double.TryParse(ConfigurationManager.GetPropertyItemValue(nameof(ZVH)), out double _ZVH) ? _ZVH : throw new Exception($"{nameof(ZVH)} Parseing Error");
                TUP = double.TryParse(ConfigurationManager.GetPropertyItemValue(nameof(TUP)), out double _TUP) ? _TUP : throw new Exception($"{nameof(TUP)} Parseing Error");
                xdpi = double.TryParse(ConfigurationManager.GetPropertyItemValue(nameof(xdpi)), out double _xdpi) ? _xdpi : throw new Exception($"{nameof(xdpi)} Parseing Error");
                ydpi = double.TryParse(ConfigurationManager.GetPropertyItemValue(nameof(ydpi)), out double _ydpi) ? _ydpi : throw new Exception($"{nameof(ydpi)} Parseing Error");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine(ex.Message);
                return false;
            }
            return true;
        }

        public override string ToString()
        {
            return
                $"{nameof(VBW)} : {VBW}{Environment.NewLine}" +
                $"{nameof(TBH)} : {TBH}{Environment.NewLine}" +
                $"{nameof(ZVH)} : {ZVH}{Environment.NewLine}" +
                $"{nameof(TUP)} : {TUP}{Environment.NewLine}"+
                $"{nameof(xdpi)} : {xdpi}{Environment.NewLine}"+
                $"{nameof(ydpi)} : {ydpi}{Environment.NewLine}";
        }
    }
}
