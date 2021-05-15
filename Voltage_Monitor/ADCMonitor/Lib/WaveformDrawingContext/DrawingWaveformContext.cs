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
            VoltageItemsSource = new VoltageItemList();
            DrawingConfig = new DrawingConfiguration();
        }

        public VoltageItemList VoltageItemsSource { get; }

        public DrawingConfiguration DrawingConfig { get; }

        public DrawingVisual drawingVisualA, drawingVisualB, drawingVisualC, drawingVisualD;

        public void Render(ImageSource imageSource, ERenderType renderType)
        {
            switch(renderType)
            {
                case ERenderType.All:
                    RenderA();
                    RenderB();
                    RenderC();
                    RenderD();
                    break;
                case ERenderType.Horizontal:
                    RenderB();
                    RenderC();
                    RenderD();
                    break;
            }
            RenderTargetBitmap bmp = new RenderTargetBitmap((int)DrawingConfig.MW, (int)DrawingConfig.MH, DrawingConfig.xdpi, DrawingConfig.ydpi, PixelFormats.Pbgra32);
            bmp.Render(drawingVisualA);
            bmp.Render(drawingVisualB);
            bmp.Render(drawingVisualC);
            bmp.Render(drawingVisualD);
            imageSource = bmp;
        }

        /// <summary>
        /// Drawing Voltage Bar
        /// </summary>
        private void RenderA()
        {
            drawingVisualA = new DrawingVisual();
            DrawingContext drawingContext = drawingVisualA.RenderOpen();

            drawingContext.Close();
        }

        /// <summary>
        /// Drawing Timer Bar
        /// </summary>
        private void RenderB()
        {
            drawingVisualB = new DrawingVisual();
            DrawingContext drawingContext = drawingVisualB.RenderOpen();

            drawingContext.Close();
        }

        /// <summary>
        /// Drawing Waveform
        /// </summary>
        private void RenderC()
        {
            drawingVisualC = new DrawingVisual();
            DrawingContext drawingContext = drawingVisualC.RenderOpen();

            drawingContext.Close();
        }

        /// <summary>
        /// Drawing Dynamic Mouse Cursor
        /// </summary>
        private void RenderD()
        {
            drawingVisualD = new DrawingVisual();
            DrawingContext drawingContext = drawingVisualD.RenderOpen();

            drawingContext.Close();
        }

        public override string ToString()
        {
            return VoltageItemsSource?.ToString();
        }
    }
}
