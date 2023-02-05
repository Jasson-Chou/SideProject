using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
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


        private ushort value;
        /// <summary>
        /// Unit: ADC Value
        /// </summary>
        public ushort Value 
        {
            get => value;
            set
            {
                this.value = value;
                SetValueCallback?.Invoke(this);
            }
        }

        internal Action<ADCItem> SetValueCallback { get; set; }

        public override string ToString()
        {
            return $"Index[{Index}] - ADC Value \"{Value}\"";
        }
    }

    public class ADCItemCollection:IEnumerable<ADCItem>
    {
        private readonly string fileStreamName;

        public int Count { get; private set; }

        public ADCItemCollection()
        {
            fileStreamName = Guid.NewGuid().ToString();
            Count = 0;
        }

        public void Add(ADCItem item)
        {
            using (FileStream fs = new FileStream(fileStreamName, FileMode.Append))
            {
                using (BinaryWriter writer = new BinaryWriter(fs))
                {
                    writer.Write(item.Index);
                    writer.Write(item.Value);
                }
            }
            item.SetValueCallback = this.SetValueCallback;
            ++Count;
        }

        public void AddRange(IEnumerable<ADCItem> collection)
        {
            using (StreamWriter writer = new StreamWriter(fileStreamName, true))
            {
                foreach (var item in collection)
                {
                    writer.Write(item.Index);
                    writer.Write(item.Value);
                    item.SetValueCallback = this.SetValueCallback;
                }
                
            }

            Count += collection.Count();
        }

        public ADCItem this[int index]
        {
            get
            {
                if (index < 0) throw new ArgumentOutOfRangeException();
                using(FileStream fs = new FileStream(fileStreamName, FileMode.OpenOrCreate))
                {
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        long position = index * 6;
                        if (position >= br.BaseStream.Length) return null;
                        fs.Seek(position, SeekOrigin.Begin);
                        int itemIndex =  br.ReadInt32();
                        ushort itemValue = br.ReadUInt16();
                        var item = new ADCItem(itemIndex, itemValue) { SetValueCallback = this.SetValueCallback };
                        return item;
                    }
                }
                
            }
        }

        public IEnumerator<ADCItem> GetEnumerator()
        {
            using (FileStream fs = new FileStream(fileStreamName, FileMode.OpenOrCreate))
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    while (br.BaseStream.Position < br.BaseStream.Length)
                    {
                        int itemIndex = br.ReadInt32();
                        ushort itemValue = br.ReadUInt16();
                        yield return new ADCItem(itemIndex, itemValue) { SetValueCallback = this.SetValueCallback };
                    }
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, this.Select(item => item.ToString()));
        }

        private void SetValueCallback(ADCItem item)
        {
            using (FileStream fs = new FileStream(fileStreamName, FileMode.Open))
            {
                using (BinaryWriter writer = new BinaryWriter(fs))
                {
                    fs.Position = item.Index * 6 + 4;
                    writer.Write(item.Value);
                }
            }
        }
    }
}
