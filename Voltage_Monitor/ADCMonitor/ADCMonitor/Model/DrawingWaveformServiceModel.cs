using GUIWaveform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static GUIWaveform.DrawingWaveformContext;

namespace ADCMonitor.Model
{
    public class DrawingWaveformServiceModel
    {
        public DrawingWaveformServiceModel()
        {
            context = new DrawingWaveformContext();
        }

        private readonly DrawingWaveformContext context;

        public DrawingConfiguration Configuration => context.DrawingConfig;

        public Image DrawingImage { get => context.RenderImage; set => context.RenderImage = value; }

        public Size ImageMinimumSize => new Size(Configuration.MinMW, Configuration.MinMH);

        public int HornizontalMaximum => Configuration.ADCItemsSource.Count * Configuration.TUP;

        public int HornizontalOffset { get => Configuration.XOffset; set => Configuration.XOffset = value; }
        
        public bool ShowMouseCursor { get => Configuration.MouseCursor.Show; set => Configuration.MouseCursor.Show = value; }

        public int TimingUnitPixel { get => Configuration.TUP; set => Configuration.TUP = value; }
        
        public int TimingUnitPixelMaximum { get => Configuration.MaxTUP; set => Configuration.MaxTUP = value; }

        public int TimingUnitPixelMinimum { get => Configuration.MinTUP; set => Configuration.MinTUP = value; }

        public void ImageRender(Point point)
        {
            var cursor = Configuration.MouseCursor;
            cursor.X = point.X;
            cursor.Y = point.Y;

            ImageRender(ERenderType.MouseCursor);
        }

        public void ImageRender(Size size)
        {
            Configuration.MW = size.Width;

            Configuration.MH = size.Height;

            ImageRender(ERenderType.All);
        }

        public void ImageRender(ERenderType renderType)
        {
            if (DrawingImage == null) return;
            context.Render(renderType);
        }

    }
}
