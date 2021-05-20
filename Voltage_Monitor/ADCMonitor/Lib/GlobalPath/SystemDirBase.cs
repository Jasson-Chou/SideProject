using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalPath
{
    internal class SystemFolderName
    {
        public const string LibsFolder = "Libs";

        public const string ConfigsFolder = "Configs";
    }

    public class SystemFolderFileName
    {
        public static string LibsFolder => Path.Combine ( AppDomain.CurrentDomain.BaseDirectory,  SystemFolderName.LibsFolder);

        public static string ConfigsFolder => Path.Combine(AppDomain.CurrentDomain.BaseDirectory ,  SystemFolderName.ConfigsFolder);

        public static string GUIWaveformConfiguration => Path.Combine(ConfigsFolder, "GUIWaveform.config");
    }
}
