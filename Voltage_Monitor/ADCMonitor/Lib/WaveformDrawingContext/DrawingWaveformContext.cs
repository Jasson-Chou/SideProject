using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ConstrainedExecution;
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
            blackPen = new Pen(new SolidColorBrush(Colors.Black), 1.5);
            blackPen_dash = new Pen(new SolidColorBrush(Colors.Black), 1.5);
            blackPen_dash.DashStyle = DashStyles.Dash;
            bluePen = new Pen(new SolidColorBrush(Colors.Blue), 1.5);
            bluePen_dash = new Pen(new SolidColorBrush(Colors.Blue), 1.5);
            bluePen_dash.DashStyle = DashStyles.Dash;

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
        ///  Drawing Cursor When Mouse Enter
        /// </summary>
        private DrawingVisual drawingVisualF;


        private readonly Pen blackPen;
        private readonly Pen blackPen_dash;
        private readonly Pen bluePen;
        private readonly Pen bluePen_dash;

        private RenderTargetBitmap Bitmap;

        public void Render(System.Windows.Controls.Image image, ERenderType renderType)
        {
            DrawingConfig.RefreshData();

            if (Bitmap == null || Bitmap.PixelWidth != (int)DrawingConfig.MW || Bitmap.PixelHeight != (int)DrawingConfig.MH
                || Bitmap.DpiX != DrawingConfig.xdpi || Bitmap.DpiY != DrawingConfig.ydpi)
            {
                Bitmap = new RenderTargetBitmap((int)DrawingConfig.MW, (int)DrawingConfig.MH, DrawingConfig.xdpi, DrawingConfig.ydpi, PixelFormats.Pbgra32);
                image.Source = Bitmap;
            }
            else
            {
                Bitmap.Clear();
            }

            switch (renderType)
            {
                case ERenderType.All:
                    RenderC();
                    RenderA();
                    RenderB();
                    //RenderD();
                    //RenderE();
                    RenderF();
                    break;
                case ERenderType.Horizontal:
                    RenderC();
                    RenderB();
                    //RenderD();
                    //RenderE();
                    RenderF();
                    break;
            }

            
            
            Bitmap.Render(drawingVisualC); // C Waveform
            Bitmap.Render(drawingVisualA); // A Voltage Bar
            Bitmap.Render(drawingVisualB); // B Timebar
            
            //if (drawingVisualD != null) Bitmap.Render(drawingVisualD); // D
            //if (drawingVisualE != null) Bitmap.Render(drawingVisualE); // E

            Bitmap.Render(drawingVisualF); // F Drawing Cursor When Mouse Enter


        }

        /// <summary>
        /// Drawing Voltage Bar
        /// </summary> 
        private void RenderA()
        {
            const double textXOffset = -30;
            const double textYOffset = -8;
            if(drawingVisualA == null) drawingVisualA = new DrawingVisual();
            DrawingContext dc = drawingVisualA.RenderOpen();
            Clear(dc);

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
            dc.DrawLine(blackPen, p1, p2);
            dc.DrawLine(blackPen, MaxVoltLineLeft, MaxVoltLineRight);
            dc.DrawLine(blackPen, MinVoltLineLeft, MinVoltLineRight);

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
            if(drawingVisualB == null) drawingVisualB = new DrawingVisual();
            DrawingContext dc = drawingVisualB.RenderOpen();
            Clear(dc);

            Point p1 = DrawingConfig.OP;
            Point p2 = new Point(DrawingConfig.VBW + DrawingConfig.TBL, DrawingConfig.MH - DrawingConfig.TBH);
            dc.DrawLine(blackPen, p1, p2);

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
                var formattedText = GetFormattedText(text, new SolidColorBrush(Colors.Black), 12);
                var textLeftX = timingX - text.Length / 2 * 4;
                var textRightX = timingX + text.Length / 2 * 4;
                if (textRightX > lastTextRightX + 60)
                {
                    dc.DrawLine(blackPen, new Point(timingX, timingTopY), new Point(timingX, timingBottomY));
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
            if(drawingVisualC == null) drawingVisualC = new DrawingVisual();
            DrawingContext dc = drawingVisualC.RenderOpen();
            Clear(dc);

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
                dc.DrawLine(blackPen, curr, next);
            }

            dc.Close();
        }

        /// <summary>
        /// Drawing Mouse Cursor
        /// </summary>
        private void RenderD()
        {
            if(drawingVisualD == null) drawingVisualD = new DrawingVisual();
            DrawingContext dc = drawingVisualD.RenderOpen();
            Clear(dc);

            var adcItemsSource = DrawingConfig.ADCItemsSource;
            var cursorItemsSource = DrawingConfig.CursorItemsSource;
            if (cursorItemsSource.Count == 0) return;

            int FPI = DrawingConfig.FPI;
            int LPI = DrawingConfig.LPI;

            var drawingCursorItemsSource = cursorItemsSource.Where(item => item.ADCIndex >= FPI && item.ADCIndex <= LPI);

            if (drawingCursorItemsSource.Count() == 0) return;

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

                dc.DrawLine(blackPen_dash, topPoint, point);

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
            drawingVisualE = null;
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
            if(drawingVisualF == null) drawingVisualF = new DrawingVisual();
            DrawingContext dc = drawingVisualF.RenderOpen();
            Clear(dc);

            var config = DrawingConfig;
            bool isMouseEnter = config.MouseCursor != null;
            
            if (!isMouseEnter) return;

            var MP_X = config.MouseCursor.X;
            var MP_Y = config.MouseCursor.Y;
            
            if (MP_X < config.VBW || MP_X > config.VBW + config.TBL) return;
            if (MP_Y > config.MinVoltY || MP_Y < config.MaxVoltY) return;

            var targetX = MP_X + config.XOffset - config.VBW;
            var adcIndex = (int)(targetX / config.TUP + (targetX % config.TUP > config.TUP / 2 ? 1 : 0));
            if (adcIndex >= DrawingConfig.ADCItemsSource.Count) return;
            var adcItem = DrawingConfig.ADCItemsSource.Single(item => item.Index == adcIndex);
            var drawingX = adcItem.Index * DrawingConfig.TUP + DrawingConfig.VBW - DrawingConfig.XOffset;
            var drawingTopY = config.MaxVoltY - 10;
            
            var mouseHeight = Math.Abs(MP_Y - config.MinVoltY);
            var volt = config.ADCToVoltage((ushort)(mouseHeight / config.PPB));
            
            
            var topText = $"{Math.Round(config.ADCToVoltage(adcItem.Value), 3, MidpointRounding.AwayFromZero)} V @ {adcIndex * config.ADCCapturePeriod} ms";
            var leftText = $"{Math.Round(volt, 3, MidpointRounding.AwayFromZero)} V";

            var topTextFormat = GetFormattedText(topText, new SolidColorBrush(Colors.Black), 12);
            var leftTextFormat = GetFormattedText(leftText, new SolidColorBrush(Colors.Blue), 12);
            
            dc.DrawText(topTextFormat, new Point(drawingX - (topText.Length / 2 * 8.0d), drawingTopY - 10));
            dc.DrawText(leftTextFormat, new Point(DrawingConfig.VBW - 50, MP_Y - 8));

            dc.DrawLine(bluePen_dash, new Point(drawingX, config.OP.Y), new Point(drawingX, drawingTopY));
            dc.DrawLine(bluePen_dash, new Point(drawingX, MP_Y), new Point(config.VBW, MP_Y));
            dc.Close();
        }

        private System.Globalization.CultureInfo cultureInfo;
        private Typeface typeface;

        private FormattedText GetFormattedText(string msg, Brush brush, double fontSize = 8.0d)
        {
            if (cultureInfo == null) cultureInfo = new System.Globalization.CultureInfo("en-us");
            if (typeface == null) typeface = new Typeface("Verdana");
            return new FormattedText(msg, cultureInfo, FlowDirection.LeftToRight, typeface, fontSize, brush);
        }

        private Rect clearRect = new Rect(0, 0, 0, 0);
        private SolidColorBrush clearBrush;
        private void Clear(DrawingContext dc)
        {
            if (clearRect.Width != DrawingConfig.MW) clearRect.Width = DrawingConfig.MW;
            if (clearRect.Height != DrawingConfig.MH) clearRect.Height = DrawingConfig.MH;
            if(clearBrush == null) clearBrush = new SolidColorBrush(Colors.Transparent);
            dc.DrawRectangle(clearBrush, null, clearRect);
        }

        public override string ToString()
        {
            return "";
        }
    }
}
