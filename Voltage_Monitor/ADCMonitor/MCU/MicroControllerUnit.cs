using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCU
{

    public delegate void OnDiagnosisHandler(string Message);
    public delegate void OnADCValueHandler(ushort value);

    public class MicroControllerUnit
    {
        private Comport Comport { get; }

        public event OnDiagnosisHandler OnDiagnosis;

        public event OnADCValueHandler OnADCValue;

        public bool IsSimulation { get; set; }

        public bool IsBusy { get; set; }

        public bool IsConnecting { get; private set; }

        public MicroControllerUnit()
        {
            IsSimulation = true;
        }

        public bool Connect()
        {
            if(IsSimulation)
                return true;
            return true;
        }

        public bool Disconnect()
        {
            if (IsSimulation)
                return true;
            return true;
        }

        public void StartCapture(CaptureProperty captureProperty)
        {
            if (IsBusy)
                return;
            IsBusy = true;

            ADCCapture(captureProperty);

            IsBusy = false;
        }

        private void ADCCapture(CaptureProperty captureProperty)
        {
            var MemDepthLen = GetDepthLength(captureProperty.CaptureDepth);
            if (IsSimulation)
            {
                for(uint index = 0; index < MemDepthLen; index++)
                {

                }

            }
            else
            {

            }
        }

        private uint GetDepthLength(ECaptureDepth eCaptureDepth)
        {
            return (uint)eCaptureDepth / sizeof(ushort);
        }

        public void StopCapture()
        {

        }

        public void Diagnosis()
        {

        }

        public override string ToString()
        {
            return base.ToString();
        }

        internal void RaiseOnDiagnosis(string Message)
        {
            OnDiagnosis?.Invoke(Message);
        }

        internal void RaiseOnADCValue(ushort Value)
        {
            OnADCValue?.Invoke(Value);
        }
    }
}
