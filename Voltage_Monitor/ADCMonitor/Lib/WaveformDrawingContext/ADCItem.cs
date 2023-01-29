using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace GUIWaveform
{
    public class ADCItem
    {
        public ADCItem(int index, ushort value)
        {
            this.Index = index;
            this.Value = value;
        }

        public int Index { get; }

        /// <summary>
        /// Unit: ADC Value
        /// </summary>
        public ushort Value { get; set; }

        public override string ToString()
        {
            return $"Index[{Index}] - ADC Value \"{Value}\"";
        }
    }

    public class ADCItemCollection:IEnumerable<ADCItem>
    {
        private readonly string fileStreamName;
        private readonly Stream stream;
        private readonly List<ADCItem> _items;

        public int Count { get; private set; }

        public ADCItemCollection()
        {
            fileStreamName = Guid.NewGuid().ToString();
            stream = new FileStream($"{fileStreamName}.dat", FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
            _items = new List<ADCItem>();
            Count = 0;
        }

        public void Add(ADCItem voltageItem)
        {
            StreamWriter writer = new StreamWriter(stream);
            writer.WriteLine($"{voltageItem.Index}:{voltageItem.Value}");
            ++Count;
        }

        public void AddRange(IEnumerable<ADCItem> collection)
        {
            StreamWriter writer = new StreamWriter(stream);
            var result = string.Join(Environment.NewLine, collection.Select(item => $"{item.Index}:{item.Value}"));
            writer.WriteLine(result);
            Count += collection.Count();
        }

        public ADCItem this[int index]
        {
            get
            {
                stream.Flush();
                long currPosition = stream.Position;



                stream.Position = currPosition;
            }
        }

        public IEnumerator<ADCItem> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, this.Select(item => item.ToString()));
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }
    }
}
