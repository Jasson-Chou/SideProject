using ADCMonitor.Model;
using ADCMonitor.View;
using GlobalPath;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Docking;
using Telerik.Windows.Controls.SplashScreen;
using Telerik.Windows.Input.Touch;

namespace ADCMonitor
{
    /// <summary>
    /// App.xaml 的互動邏輯
    /// </summary>
    public partial class App : Application
    {

        public DrawingWaveformServiceModel WaveformServiceModel { get; private set; }

        public bool DebugEnabled { get; private set; }

        public App()
        {
            InitializeAssemblyFiles();
        }

        private void InitializeAssemblyFiles()
        {
            AppDomain.CurrentDomain.AssemblyResolve += (object sender, ResolveEventArgs args) =>
            {
                string assemblyFile = new AssemblyName(args.Name).Name;
                assemblyFile += ".dll";

                string absoluteFolder = new FileInfo((new System.Uri(Assembly.GetExecutingAssembly().CodeBase)).LocalPath).Directory.FullName; //current exe path
                absoluteFolder += @"\Libs"; //add the DLLs search folder
                string targetPath = Path.Combine(absoluteFolder, assemblyFile);
                string resourceName = "ADCMonitor." + assemblyFile;

                try
                {
                    Assembly assembly = null;
                    if (File.Exists(targetPath))
                    {
                        assembly = Assembly.LoadFile(targetPath);
                    }
                    else
                    {
                        using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
                        {
                            if (stream != null)
                            {
                                byte[] assemblyData = new byte[stream.Length];
                                stream.Read(assemblyData, 0, assemblyData.Length);
                                assembly = Assembly.Load(assemblyData);
                                stream.Close();
                            }
                        }
                    }
                    return assembly;
                }
                catch (Exception)
                {
                    return null;
                }
            };
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var dataContext = (SplashScreenDataContext)RadSplashScreenManager.SplashScreenDataContext;
            //dataContext.ImagePath = "";
            dataContext.Content = "Loading Application";
            dataContext.Footer = "This is the footer.";

            RadSplashScreenManager.Show();

            WaveformServiceModel = new DrawingWaveformServiceModel();

            RadSplashScreenManager.Close();

            TouchManager.IsTouchEnabled = false;
            TouchManager.IsEnabled = false;

            StyleManager.ApplicationTheme = new Windows11Theme(Windows11Palette.ColorVariation.Light);

            base.OnStartup(e);

            ArgumentsHandler(e.Args);
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            MainWindow = new MainWindow();
            MainWindow.Show();
        }

        private void ArgumentsHandler(string[] args)
        {
            if (args.Contains("--debug")) DebugEnabled = true;
        }
    }
}
