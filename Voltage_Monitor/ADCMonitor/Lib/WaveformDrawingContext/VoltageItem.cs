using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUIWaveform
{
    public class VoltageItem
    {
        public VoltageItem(int index, double value)
        {
            this.Index = index;
            this.Value = value;
        }

        public int Index { get; }

        /// <summary>
        /// Unit: mV
        /// </summary>
        public double Value { get; }

        internal VoltageItemList voltageItemsSource { get; set; }

        public void Remove()
        {
            voltageItemsSource.Remove(this);
            voltageItemsSource = null;
        }

        public override string ToString()
        {
            return $"Index{Index} - Value {Value} mVolt";
        }
    }

    public class VoltageItemList:List<VoltageItem>
    {
        public new void Add(VoltageItem voltageItem)
        {
            voltageItem.voltageItemsSource = this;
            base.Add(voltageItem);
        }

        public new void AddRange(IEnumerable<VoltageItem> collection)
        {
            foreach (var item in collection)
                item.voltageItemsSource = this;
            base.AddRange(collection);
        }

        public override string ToString()
        {
            string result = string.Empty;
            foreach (var item in this)
                result += item.ToString() + Environment.NewLine;
            return result;
        }
    }
}
