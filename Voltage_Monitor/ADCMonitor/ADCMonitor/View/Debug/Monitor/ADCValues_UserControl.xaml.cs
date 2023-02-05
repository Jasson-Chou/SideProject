using ADCMonitor.Model;
using GalaSoft.MvvmLight;
using GUIWaveform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ADCMonitor.View.Debug.Monitor
{
    /// <summary>
    /// ADCValues_UserControl.xaml 的互動邏輯
    /// </summary>
    public partial class ADCValues_UserControl : UserControl
    {
        private const int ADCIndexFieldIndex = 0;
        private const int ADCValueFieldIndex = 1;
        private const int TimingFieldIndex = 2;
        private const int VoltageFieldIndex = 3;
        private const int ColumnSize = 4;

        public ADCValues_UserControl()
        {
            InitializeComponent();
            if(!ViewModelBase.IsInDesignModeStatic)
                this.visualGrid.Reset(((App)(App.Current)).WaveformServiceModel.Configuration.ADCItemsSource.Count, ColumnSize);
        }

        private void visualGrid_CellValueNeeded(object sender, Telerik.Windows.Controls.VirtualGrid.CellValueEventArgs e)
        {
            var config = ((App)(App.Current)).WaveformServiceModel.Configuration;
            var items = config.ADCItemsSource;
            if (e.ColumnIndex == ADCIndexFieldIndex) e.Value = e.RowIndex.ToString();
            else if (e.ColumnIndex == ADCValueFieldIndex) e.Value = items[e.RowIndex].Value.ToString();
            else if (e.ColumnIndex == TimingFieldIndex) e.Value = $"{config.ADCCapturePeriod * e.RowIndex} ms";
            else if (e.ColumnIndex == VoltageFieldIndex) e.Value = $"{Math.Round(config.ADCToVoltage(items[e.RowIndex].Value), 3, MidpointRounding.AwayFromZero)} Volt";
            else e.Value = null;
        }

        private void visualGrid_EditorNeeded(object sender, Telerik.Windows.Controls.VirtualGrid.EditorNeededEventArgs e)
        {
            if (e.ColumnIndex != 1) { visualGrid.CancelEdit(); return; }

            var config = ((App)(App.Current)).WaveformServiceModel.Configuration;
            var items = config.ADCItemsSource;
            TextBox tb = new TextBox();
            tb.PreviewMouseUp += Tb_PreviewMouseUp;
            e.Editor = tb;
            e.EditorProperty = TextBox.TextProperty;
            tb.Text = items[e.RowIndex].Value.ToString();
        }

        private void Tb_PreviewMouseUp(object sender, RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            tb.PreviewMouseUp -= Tb_PreviewMouseUp;
            tb.SelectAll();
        }

        private void visualGrid_EditorValueChanged(object sender, Telerik.Windows.Controls.VirtualGrid.CellValueEventArgs e)
        {
            if (e.ColumnIndex != ADCValueFieldIndex) { visualGrid.CancelEdit(); return; }

            int rowIndex = e.RowIndex;
            var adcItems = ((App)(App.Current)).WaveformServiceModel.Configuration.ADCItemsSource;
            var item = adcItems[rowIndex];
            bool result = ushort.TryParse(e.Value.ToString(), out ushort adcValue);
            if (result)
            {
                item.Value = adcValue;

                if (visualGrid.SelectedCells.Count > 1)
                {
                    foreach (var cell in visualGrid.SelectedCells)
                    {
                        if (cell.ColumnIndex == ADCValueFieldIndex) adcItems[cell.RowIndex].Value = adcValue;
                    }
                }

            }
            else
            {
                visualGrid.CancelEdit();
            }
        }

        private void visualGrid_CellEditEnded(object sender, Telerik.Windows.Controls.VirtualGrid.CellEditEndedEventArgs e)
        {
            this.visualGrid.PushCellValue(e.RowIndex, e.ColumnIndex, e.Value);
            var selectedCells = visualGrid.SelectedCells;
            if (selectedCells.Count > 1)
            {
                foreach (var cell in selectedCells)
                {
                    if (cell.ColumnIndex == ADCValueFieldIndex && e.RowIndex != cell.RowIndex)
                    {
                        visualGrid.PushCellValue(cell.RowIndex, cell.ColumnIndex, e.Value);
                    }
                }
            }

        }

        private void visualGrid_HeaderSizeNeeded(object sender, Telerik.Windows.Controls.VirtualGrid.HeaderSizeEventArgs e)
        {

        }

        private void visualGrid_HeaderValueNeeded(object sender, Telerik.Windows.Controls.VirtualGrid.HeaderValueEventArgs e)
        {
            if(e.HeaderOrientation == Telerik.Windows.Controls.VirtualGrid.VirtualGridOrientation.Horizontal)
            {
                if (e.Index == ADCIndexFieldIndex) e.Value = "Index";
                else if (e.Index == ADCValueFieldIndex) e.Value = "ADC Value";
                else if (e.Index == TimingFieldIndex) e.Value = "Timing";
                else if (e.Index == VoltageFieldIndex) e.Value = "Voltage";
            }
        }

        private void visualGrid_CopyingCellClipboardContent(object sender, Telerik.Windows.Controls.VirtualGridCellClipboardEventArgs e)
        {
            //if (e.Cell.ColumnIndex == 0)
            //{
            //    var item = this.clubsSource.ElementAt(e.Cell.RowIndex);
            //    e.Value = string.Format("{0} {1}", item.Name, item.StadiumCapacity);
            //}

        }

        private void visualGrid_Copying(object sender, Telerik.Windows.Controls.VirtualGridClipboardEventArgs e)
        {

        }

        private void visualGrid_Copied(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {

        }

        private void visualGrid_CellToolTipNeeded(object sender, Telerik.Windows.Controls.VirtualGridCellToolTipEventArgs e)
        {
            int rowIndex = e.RowIndex;
            var adcItems = ((App)(App.Current)).WaveformServiceModel.Configuration.ADCItemsSource;
            if (rowIndex < 0 || rowIndex >= adcItems.Count) return;
            if (e.ColumnIndex == ADCIndexFieldIndex) e.Value = rowIndex.ToString();
            else if (e.ColumnIndex == ADCValueFieldIndex)
            {
                e.Value = adcItems[rowIndex].Value.ToString();
            }
            else if (e.ColumnIndex == TimingFieldIndex)
            {
                e.Value = $"{((App)(App.Current)).WaveformServiceModel.Configuration.ADCCapturePeriod * rowIndex} ms";
            }
        }

        private void visualGrid_CellDecorationsNeeded(object sender, Telerik.Windows.Controls.VirtualGrid.CellDecorationEventArgs e)
        {
            var ADCItems = ((App)(App.Current)).WaveformServiceModel.Configuration.ADCItemsSource;
            var colIndex = e.ColumnIndex;
            var rowIndex = e.RowIndex;
            if(colIndex == ADCValueFieldIndex && ADCItems[rowIndex].Value == 0)
            {
                e.Foreground = Brushes.Red;
            }
            else if(colIndex != ADCValueFieldIndex)
            {
                e.Background = Brushes.LightGray;
            }
        }

        private void visualGrid_OverlayBrushesNeeded(object sender, Telerik.Windows.Controls.VirtualGrid.OverlayBrushesEventArgs e)
        {
            e.Brushes.Add(Brushes.Red);
            e.Brushes.Add(Brushes.LightGray);
        }

        
    }
}
