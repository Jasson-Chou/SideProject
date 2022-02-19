using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GUIWaveform
{
    internal class ConfigurationManager
    {
        private const string ViewerProperty = "viewerproperty";
        private const string MCUProperty = "mcuproperty";

        public string ConfigFileName { get; private set; }

        public ConfigurationManager(string configName)
        {
            this.ConfigFileName = configName;
        }

        public string GetViewerPropertyItemValue(string key)
        {
            var FindKey = LoadViewerPropertyItems()?.FirstOrDefault(item => item.Attribute("key").Value == key);
            var value = FindKey?.Attribute("value").Value;
            currLoadConfig = null;
            return value;
        }

        public void SetViewerPropertyItem(string key, string value)
        {
            var FindKey = LoadViewerPropertyItems()?.FirstOrDefault(item => item.Attribute("key").Value == key);
            FindKey?.Attribute("value").SetValue(value);
            currLoadConfig?.Save(ConfigFileName);
            currLoadConfig = null;
        }

        public string GetMCUPropertyItemValue(string key)
        {
            var FindKey = LoadMCUPropertyItems()?.FirstOrDefault(item => item.Attribute("key").Value == key);
            var value = FindKey?.Attribute("value").Value;
            currLoadConfig = null;
            return value;
        }

        public void SetMCUPropertyItem(string key, string value)
        {
            var FindKey = LoadMCUPropertyItems()?.FirstOrDefault(item => item.Attribute("key").Value == key);
            FindKey?.Attribute("value").SetValue(value);
            currLoadConfig?.Save(ConfigFileName);
            currLoadConfig = null;
        }

        private XElement currLoadConfig { get; set; }

        private IEnumerable<XElement> LoadViewerPropertyItems()
        {
            if (!File.Exists(ConfigFileName))
                throw new FileNotFoundException(ConfigFileName);
            currLoadConfig = XElement.Load(ConfigFileName);
            var propertyEle = currLoadConfig?.Element(ViewerProperty);
            var itemEles = propertyEle?.Elements("item");
            return itemEles;
        }

        private IEnumerable<XElement> LoadMCUPropertyItems()
        {
            if (!File.Exists(ConfigFileName))
                throw new FileNotFoundException(ConfigFileName);
            currLoadConfig = XElement.Load(ConfigFileName);
            var propertyEle = currLoadConfig?.Element(MCUProperty);
            var itemEles = propertyEle?.Elements("item");
            return itemEles;
        }

        public override string ToString()
        {
            return XElement.Load(ConfigFileName).ToString();
        }

    }
}
