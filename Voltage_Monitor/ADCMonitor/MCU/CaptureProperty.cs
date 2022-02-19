using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCU
{
    public enum ECaptureDepth
    {
        _16K = 16,
        _24K = 24,
    }
    public class CaptureProperty
    {
        public CaptureProperty(int SamplingRate, ECaptureDepth captureDepth)
        {
            this.SamplingRate = SamplingRate;
            this.CaptureDepth = captureDepth;
        }
        public int SamplingRate { get; }
        public ECaptureDepth CaptureDepth { get; }
    }
}
