using ADCMonitor.View.Debug;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GUIWaveform;
using System.Diagnostics;

namespace ADCMonitor.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
        }

        public RelayCommand TestCommand => new RelayCommand(() =>
        {
#if DEBUG
            DrawingWaveformContext drawingWaveformContext = new DrawingWaveformContext();
            Debug.WriteLine(drawingWaveformContext.DrawingConfig.ToString());
#endif
        });

        private DebugWindow window = null;
        public RelayCommand DebugWindowRelayCommand => new RelayCommand(() =>
        {
            if(window == null)
                window = new DebugWindow();
            if (window != null && !window.IsOpen)
                window.Show();
            else if (window != null && window.IsOpen)
                window.Focus();
        }, () => ((App)App.Current).DebugEnabled);
    }
}