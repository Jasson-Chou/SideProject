using Microsoft.VisualStudio.TestTools.UnitTesting;
using GUIWaveform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace GUIWaveform.Tests
{
    [TestClass()]
    public class DrawingConfigurationTests
    {
        [TestMethod()]
        public void DrawingConfigurationTest()
        {
            DrawingConfiguration drawingConfiguration = new DrawingConfiguration();
            Debug.WriteLine($"{nameof(drawingConfiguration.VBW)} = {drawingConfiguration.VBW}");
        }

        [TestMethod()]
        public void SaveTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void LoadTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ToStringTest()
        {
            Assert.Fail();
        }
    }
}