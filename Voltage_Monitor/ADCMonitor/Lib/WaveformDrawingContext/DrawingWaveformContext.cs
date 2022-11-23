using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GUIWaveform
{
    public class DrawingWaveformContext
    {
        public enum ERenderType
        {
            All,
            Horizontal,
        }
        public DrawingWaveformContext()
        {
            DrawingConfig = new DrawingConfiguration();
        }

        public DrawingConfiguration DrawingConfig { get; }


        /// <summary>
        /// Drawing Voltage Bar
        /// </summary>
        private DrawingVisual drawingVisualA;

        /// <summary>
        /// Drawing Timer Bar
        /// </summary>
        private DrawingVisual drawingVisualB;

        /// <summary>
        /// Drawing Waveform
        /// </summary>
        private DrawingVisual drawingVisualC;

        /// <summary>
        /// Drawing Mouse Cursor
        /// </summary>
        private DrawingVisual drawingVisualD;

        /// <summary>
        /// Drawing Maximum and Minimum Voltage Sign
        /// </summary>
        private DrawingVisual drawingVisualE;


        /// <summary>
        /// 
        /// </summary>
        private DrawingVisual drawingVisualF;

        public void Render(System.Windows.Controls.Image image, ERenderType renderType)
        {
            DrawingConfig.RefreshData();
            switch (renderType)
            {
                case ERenderType.All:
                    RenderA();
                    RenderB();
                    RenderC();
                    //RenderD();
                    //RenderE();
                    break;
                case ERenderType.Horizontal:
                    RenderB();
                    RenderC();
                    //RenderD();
                    //RenderE();
                    break;
            }

            RenderTargetBitmap bmp = new RenderTargetBitmap((int)DrawingConfig.MW, (int)DrawingConfig.MH, DrawingConfig.xdpi, DrawingConfig.ydpi, PixelFormats.Pbgra32);
            
            if (drawingVisualC != null) bmp.Render(drawingVisualC); // C Waveform
            if (drawingVisualA != null) bmp.Render(drawingVisualA); // A Voltage Bar
            if(drawingVisualB != null) bmp.Render(drawingVisualB); // B Timebar
            
            if (drawingVisualD != null) bmp.Render(drawingVisualD); // D
            if (drawingVisualE != null) bmp.Render(drawingVisualE); // E

            image.Source = bmp;


        }

        /// <summary>
        /// Drawing Voltage Bar
        /// </summary> 
        private void RenderA()
        {
            const double thickness = 0.5;
            const double textXOffset = -30;
            const double textYOffset = -8;
            drawingVisualA = new DrawingVisual();
            DrawingContext dc = drawingVisualA.RenderOpen();
            Pen pen = new Pen(new SolidColorBrush(Colors.Black), thickness);


            Point p1 = new Point(DrawingConfig.VBW, DrawingConfig.TBH);
            Point p2 = DrawingConfig.OP;

            Point MaxVoltLinePoint = new Point(DrawingConfig.VBW, DrawingConfig.MaxVoltY);
            Point MaxVoltLineLeft = new Point(MaxVoltLinePoint.X - 5, MaxVoltLinePoint.Y);
            Point MaxVoltLineRight = new Point(MaxVoltLinePoint.X + 5, MaxVoltLinePoint.Y);

            Point MinVoltLinePoint = new Point(DrawingConfig.VBW, DrawingConfig.MinVoltY);
            Point MinVoltLineLeft = new Point(MinVoltLinePoint.X - 5, MinVoltLinePoint.Y);
            Point MinVoltLineRight = new Point(MinVoltLinePoint.X + 5, MinVoltLinePoint.Y);

            Point MaxVoltTextPoint = new Point(MaxVoltLineLeft.X + textXOffset, MaxVoltLineLeft.Y + textYOffset);
            Point MinVoltTextPoint = new Point(MinVoltLineLeft.X + textXOffset, MinVoltLineLeft.Y + textYOffset);
            

            dc.DrawRectangle(new SolidColorBrush(Colors.White), null, new Rect(new Point(0, p1.Y), p2));
            dc.DrawLine(pen, p1, p2);
            dc.DrawLine(pen, MaxVoltLineLeft, MaxVoltLineRight);
            dc.DrawLine(pen, MinVoltLineLeft, MinVoltLineRight);

            SolidColorBrush textColor = new SolidColorBrush(Colors.Black);
            dc.DrawText(GetFormattedText(DrawingConfig.ADCMaxVolt.ToString() + "V", textColor, 12), MaxVoltTextPoint);
            dc.DrawText(GetFormattedText(DrawingConfig.ADCMinVolt.ToString() + "V", textColor, 12), MinVoltTextPoint);

            dc.Close();
        }

        /// <summary>
        /// Drawing Timer Bar
        /// </summary>
        private void RenderB()
        {
            const double thickness = 0.5;
            drawingVisualB = new DrawingVisual();
            DrawingContext dc = drawingVisualB.RenderOpen();
            Pen pen = new Pen(new SolidColorBrush(Colors.Black), thickness);

            Point p1 = DrawingConfig.OP;
            Point p2 = new Point(DrawingConfig.VBW + DrawingConfig.TBL, DrawingConfig.MH - DrawingConfig.TBH);
            dc.DrawLine(pen, p1, p2);

            var adcItemsSource = DrawingConfig.ADCItemsSource;
            int FPI = DrawingConfig.FPI;
            int LPI = DrawingConfig.LPI;
            const double timingH = 5.0d;
            double timingTopY = DrawingConfig.OP.Y - (timingH / 2.0d);
            double timingBottomY = timingTopY + timingH;

            double lastTextRightX = double.MinValue;
            for (int adcItemIndex = FPI; adcItemIndex <= LPI; adcItemIndex++)
            {
                var adcItem = adcItemsSource.FirstOrDefault(item => item.Index == adcItemIndex);
                if (adcItem == null) continue;
                double timingX = adcItem.Index * DrawingConfig.TUP + DrawingConfig.VBW - DrawingConfig.XOffset;
                
                var text = $"{adcItemIndex * DrawingConfig.ADCCapturePeriod} ms";
                var formattedText = GetFormattedText(text, new SolidColorBrush(Colors.Black));
                var textLeftX = timingX - text.Length / 2 * 4;
                var textRightX = timingX + text.Length / 2 * 4;
                if (textRightX > lastTextRightX + 50)
                {
                    dc.DrawLine(pen, new Point(timingX, timingTopY), new Point(timingX, timingBottomY));
                    dc.DrawText(formattedText, new Point(textLeftX, timingBottomY));
                    lastTextRightX = textRightX;
                }
                

            }

            dc.Close();
        }

        /// <summary>
        /// Drawing Waveform
        /// </summary>
        private void RenderC()
        {
            const double thickness = 0.5;
            drawingVisualC = new DrawingVisual();
            DrawingContext dc = drawingVisualC.RenderOpen();
            Pen pen = new Pen(new SolidColorBrush(Colors.Black), thickness);

            var adcItemsSource = DrawingConfig.ADCItemsSource;
            int FPI = DrawingConfig.FPI;
            int LPI = DrawingConfig.LPI;

            List<Point> drawingPointItemsSource = new List<Point>(LPI - FPI + 1);

            for(int adcItemIndex = FPI; adcItemIndex <= LPI; adcItemIndex++)
            {
                var adcItem = adcItemsSource.FirstOrDefault(item => item.Index == adcItemIndex);
                if (adcItem == null) continue;
                double timingX = adcItem.Index * DrawingConfig.TUP + DrawingConfig.VBW - DrawingConfig.XOffset;

                var point = new Point(timingX, DrawingConfig.MinVoltY - adcItem.Value * DrawingConfig.PPB);
                drawingPointItemsSource.Add(point);
            }

            for(int idx = 0; idx < drawingPointItemsSource.Count; idx++)
            {
                var nextIndex = idx + 1;
                var curr = drawingPointItemsSource[idx];
                var next = drawingPointItemsSource.ElementAtOrDefault(nextIndex);
                if (nextIndex == drawingPointItemsSource.Count) break;
                dc.DrawLine(pen, curr, next);
            }

            dc.Close();
        }

        /// <summary>
        /// Drawing Mouse Cursor
        /// </summary>
        private void RenderD()
        {
            var adcItemsSource = DrawingConfig.ADCItemsSource;
            var cursorItemsSource = DrawingConfig.CursorItemsSource;
            if (cursorItemsSource.Count == 0)
                return;

            int FPI = DrawingConfig.FPI;
            int LPI = DrawingConfig.LPI;

            var drawingCursorItemsSource = cursorItemsSource.Where(item => item.ADCIndex >= FPI && item.ADCIndex <= LPI);

            if (drawingCursorItemsSource.Count() == 0)
                return;

            drawingVisualD = new DrawingVisual();
            DrawingContext dc = drawingVisualD.RenderOpen();
            const double thickness = 0.5;
            Pen pen = new Pen(new SolidColorBrush(Colors.Black), thickness);
            pen.DashStyle = DashStyles.Dash;

            List<Point> drawingPoint = new List<Point>(drawingCursorItemsSource.Count());
            foreach(var cursorItem in drawingCursorItemsSource)
            {
                var adcItem = adcItemsSource.SingleOrDefault(item => item.Index == cursorItem.ADCIndex);
                if (adcItem == null)
                    adcItem = adcItemsSource.LastOrDefault(item => item.Index < cursorItem.ADCIndex);

                double x = cursorItem.ADCIndex * DrawingConfig.TUP + DrawingConfig.VBW - DrawingConfig.XOffset;
                double y = DrawingConfig.OP.Y - adcItem.Value * DrawingConfig.PPB;

                var topPoint = new Point(x, DrawingConfig.TBH);
                var point = new Point(x, y);

                dc.DrawLine(pen, topPoint, point);

                // Drawing Text
                string time = $"{cursorItem.Time.ToString()}ms";

                topPoint.Offset(time.Length / 2.0d - topPoint.X, topPoint.Y - 10.0d);

                dc.DrawText(GetFormattedText(time,Brushes.Black), topPoint);
            }

            dc.Close();
        }

        /// <summary>
        /// Drawing Maximum and Minimum Voltage Sign
        /// </summary>
        private void RenderE()
        {
            var adcItemsSource = DrawingConfig.ADCItemsSource;
            int FPI = DrawingConfig.FPI;
            int LPI = DrawingConfig.LPI;

            var drawingAdcItems = adcItemsSource.Where(item => item.Index >= FPI && item.Index <= LPI);

            if (drawingAdcItems.Count() == 0) return;// Not Found In Range

            var maxValueAdcItem = drawingAdcItems.First(adcItem => adcItem.Value == drawingAdcItems.Max(item => item.Value));
            var minValueAdcItem = drawingAdcItems.First(adcItem => adcItem.Value == drawingAdcItems.Min(item => item.Value));

            drawingVisualE = new DrawingVisual();
            DrawingContext dc = drawingVisualE.RenderOpen();
            const double thickness = 0.5;
            Pen maxPen = new Pen(new SolidColorBrush(Colors.Blue), thickness);
            Pen minPen = new Pen(new SolidColorBrush(Colors.Red), thickness);

            double maxValuePointX = maxValueAdcItem.Index * DrawingConfig.TUP + DrawingConfig.VBW - DrawingConfig.XOffset;
            double maxValuePointY = DrawingConfig.OP.Y - maxValueAdcItem.Value * DrawingConfig.PPB;
            Point maxPoint = new Point(maxValuePointX, maxValuePointY);

            double minValuePointX = minValueAdcItem.Index * DrawingConfig.TUP + DrawingConfig.VBW - DrawingConfig.XOffset;
            double minValuePointY = DrawingConfig.OP.Y - minValueAdcItem.Value * DrawingConfig.PPB;
            Point minPoint = new Point(minValuePointX, minValuePointY);

            double maxADCVolt = DrawingConfig.ADCToVoltage(maxValueAdcItem.Value);
            double minADCVolt = DrawingConfig.ADCToVoltage(minValueAdcItem.Value);

            dc.DrawText(GetFormattedText($"{Math.Round(maxADCVolt, 2)} V", Brushes.Black), maxPoint);
            dc.DrawText(GetFormattedText($"{Math.Round(minADCVolt, 2)} V",Brushes.Black), minPoint);

            dc.Close();
        }

        /// <summary>
        /// Drawing Cursor When Mouse Enter
        /// </summary>
        private void RenderF()
        {
            bool isMouseEnter = true;
            Point MousePoint = new Point(0, 0);
            if (!isMouseEnter) return;


        }

        private FormattedText GetFormattedText(string msg, Brush brush, double fontSize = 8.0d)
        {
            return new FormattedText($"{msg}", 
                new System.Globalization.CultureInfo("en-us"), FlowDirection.LeftToRight,
                    new Typeface("Verdana"), fontSize, Brushes.Black);
        }

        public override string ToString()
        {
            return "";
        }
    }
}
