using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUIWaveform
{
    public class ACDItem
    {
        public ACDItem(int index, ushort value)
        {
            this.Index = index;
            this.Value = value;
        }

        public int Index { get; }

        /// <summary>
        /// Unit: ADC Value
        /// </summary>
        public ushort Value { get; }

        internal ADCItemList ADCItemsSource { get; set; }

        public void Remove()
        {
            ADCItemsSource.Remove(this);
            ADCItemsSource = null;
        }

        public override string ToString()
        {
            return $"Index[{Index}] - ADC Value \"{Value}\"";
        }
    }

    public class ADCItemList:List<ACDItem>
    {
        public new void Add(ACDItem voltageItem)
        {
            voltageItem.ADCItemsSource = this;
            base.Add(voltageItem);
        }

        public new void AddRange(IEnumerable<ACDItem> collection)
        {
            foreach (var item in collection)
                item.ADCItemsSource = this;
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
