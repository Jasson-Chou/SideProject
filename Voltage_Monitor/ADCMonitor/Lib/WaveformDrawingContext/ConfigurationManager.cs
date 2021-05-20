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
        public string ConfigFileName { get; private set; }

        public ConfigurationManager(string configName)
        {
            this.ConfigFileName = configName;
        }

        public string GetPropertyItemValue(string key)
        {
            var FindKey = LoadPropertyItems()?.FirstOrDefault(item => item.Attribute("key").Value == key);
            var value = FindKey?.Attribute("value").Value;
            currLoadConfig = null;
            return value;
        }

        public void SetPropertyItem(string key, string value)
        {
            var FindKey = LoadPropertyItems()?.FirstOrDefault(item => item.Attribute("key").Value == key);
            FindKey?.Attribute("value").SetValue(value);
            currLoadConfig?.Save(ConfigFileName);
            currLoadConfig = null;
        }

        private XElement currLoadConfig { get; set; }

        private IEnumerable<XElement> LoadPropertyItems()
        {
            if (!File.Exists(ConfigFileName))
                throw new FileNotFoundException(ConfigFileName);
            currLoadConfig = XElement.Load(ConfigFileName);
            var propertyEle = currLoadConfig?.Element("property");
            var itemEles = propertyEle?.Elements("item");
            return itemEles;
        }
        

    }
}
