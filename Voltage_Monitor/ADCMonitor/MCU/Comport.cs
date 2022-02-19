using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCU
{
    internal class Comport
    {
        public string Name { get; set; }
        public int BaudRate { get; set; }
        public bool IsConnecting { get; set; }
        public bool Connect()
        {
            return true;
        }
        public bool Disconnect()
        {
            return true;
        }
        public bool Write(byte[] data)
        {
            return true;
        }

        public bool Read(byte[] data)
        {
            return true;
        }
    }
}
