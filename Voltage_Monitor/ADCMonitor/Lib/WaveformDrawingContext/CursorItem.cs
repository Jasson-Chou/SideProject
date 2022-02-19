using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUIWaveform
{
    public class CursorItem
    {
        public CursorItem(int ADCIndex, uint Time)
        {
            this.ADCIndex = ADCIndex;
            this.Time = Time;
        }

        public int ADCIndex { get; }
        /// <summary>
        /// Unit: ms
        /// </summary>
        public uint Time { get; }

        internal CursorItemList CursorItemsSource { get; set; }

        public override string ToString()
        {
            return $"Cursor Index[{ADCIndex}] - Time \"{Time}\"";
        }
    }

    public class CursorItemList:List<CursorItem>
    {
        public new void Add(CursorItem cursorItem)
        {
            cursorItem.CursorItemsSource = this;
            base.Add(cursorItem);
        }

        public new void AddRange(IEnumerable<CursorItem> collection)
        {
            foreach (var item in collection)
                item.CursorItemsSource = this;
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
