using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            ADCItemsSource = new ADCItemList();
            DrawingConfig = new DrawingConfiguration();
        }

        public ADCItemList ADCItemsSource { get; }

        public DrawingConfiguration DrawingConfig { get; }

        private DrawingVisual drawingVisualA, drawingVisualB, drawingVisualC, drawingVisualD, drawingVisualE;

        public void Render(System.Windows.Controls.Image image, ERenderType renderType)
        {
            switch (renderType)
            {
                case ERenderType.All:
                    RenderA();
                    RenderB();
                    RenderC();
                    RenderD();
                    RenderE();
                    break;
                case ERenderType.Horizontal:
                    RenderB();
                    RenderC();
                    RenderD();
                    RenderE();
                    break;
            }

            RenderTargetBitmap bmp = new RenderTargetBitmap((int)DrawingConfig.MW, (int)DrawingConfig.MH, DrawingConfig.xdpi, DrawingConfig.ydpi, PixelFormats.Pbgra32);

            bmp.Render(drawingVisualA); // A
            bmp.Render(drawingVisualB); // B
            bmp.Render(drawingVisualC); // C
            bmp.Render(drawingVisualD); // D
            bmp.Render(drawingVisualE); // E

            image.Source = bmp;
        }

        /// <summary>
        /// Drawing Voltage Bar
        /// </summary> 
        private void RenderA()
        {
            drawingVisualA = new DrawingVisual();
            DrawingContext dc = drawingVisualA.RenderOpen();
            dc.DrawRectangle(null, new Pen(new SolidColorBrush(Colors.Red), 1.0d), new System.Windows.Rect( new System.Windows.Point(10 , 10),new System.Windows.Size(800, 600)));
            dc.Close();
        }

        /// <summary>
        /// Drawing Timer Bar
        /// </summary>
        private void RenderB()
        {
            drawingVisualB = new DrawingVisual();
            DrawingContext dc = drawingVisualB.RenderOpen();

            dc.Close();
        }

        /// <summary>
        /// Drawing Waveform
        /// </summary>
        private void RenderC()
        {
            drawingVisualC = new DrawingVisual();
            DrawingContext dc = drawingVisualC.RenderOpen();

            dc.Close();
        }

        /// <summary>
        /// Drawing Dynamic Mouse Cursor
        /// </summary>
        private void RenderD()
        {
            drawingVisualD = new DrawingVisual();
            DrawingContext dc = drawingVisualD.RenderOpen();

            dc.Close();
        }

        /// <summary>
        /// Drawing Max Voltage Sign
        /// </summary>
        private void RenderE()
        {
            drawingVisualE = new DrawingVisual();
            DrawingContext dc = drawingVisualD.RenderOpen();

            dc.Close();
        }

        public override string ToString()
        {
            return ADCItemsSource?.ToString();
        }
    }
}
