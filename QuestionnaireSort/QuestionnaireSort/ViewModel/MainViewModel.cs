using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using QuestionnaireSort.Model;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace QuestionnaireSort.ViewModel
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
            if (IsInDesignMode)
            {
                // Code runs in Blend --> create design time data.
            }
            else
            {
                // Code runs "for real"
            }

            IsEnabledSelecting1 = true;
            IsEnabledSelecting2 = false;
            IsEnabledSelecting3 = false;
            SortManager = new SortManager();
        }

        public SortManager SortManager { get; }

        private bool _isEnabledSelecting1;

        public bool IsEnabledSelecting1
        {
            get { return _isEnabledSelecting1; }
            set { _isEnabledSelecting1 = value; base.RaisePropertyChanged(); }
        }

        private bool _isEnabledSelecting2;

        public bool IsEnabledSelecting2
        {
            get { return _isEnabledSelecting2; }
            set { _isEnabledSelecting2 = value; base.RaisePropertyChanged(); }
        }

        private bool _isEnabledSelecting3;

        public bool IsEnabledSelecting3
        {
            get { return _isEnabledSelecting3; }
            set { _isEnabledSelecting3 = value; base.RaisePropertyChanged(); }
        }

        private bool _isEnabledSelecting4;

        public bool IsEnabledSelecting4
        {
            get { return _isEnabledSelecting4; }
            set { _isEnabledSelecting4 = value; base.RaisePropertyChanged(); }
        }

        public RelayCommand Selected1Command => new RelayCommand(async() =>
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.FileName = "選擇人事檔(CSV)"; // Default file name
            openFileDialog.DefaultExt = ".csv"; // Default file extension
            openFileDialog.Filter = "CSV (.csv)|*.csv"; // Filter files by extension
                                                  // Show open file dialog box
            Nullable<bool> result = openFileDialog.ShowDialog();
            if(result == true)
            {
                //Check File
                var fileName = openFileDialog.FileName;

                Console.WriteLine($"Selected File Name => {fileName}");
                Selected1Text = fileName;

                SortManager.Selected1FileName = fileName;

                if(await Task.Run(() => SortManager.IsOkSelected1_File()))
                {
                    Console.WriteLine("Check Selected Done");
                    IsEnabledSelecting2 = true;
                    IsEnabledSelecting3 = false;
                    IsEnabledSelecting4 = false;
                }
                else
                {
                    Console.WriteLine(SortManager.IsOkSelected1_ErrMsg);
                    //Error
                    IsEnabledSelecting2 = false;
                    IsEnabledSelecting3 = false;
                    IsEnabledSelecting4 = false;

                    return;
                }

                await Task.Run(() => SortManager.CollectFile1Datas());

            }
        });

        public RelayCommand Selected2Command => new RelayCommand(async() =>
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.FileName = "選擇問卷檔(CSV)"; // Default file name
            openFileDialog.DefaultExt = ".csv"; // Default file extension
            openFileDialog.Filter = "CSV (.csv)|*.csv"; // Filter files by extension
                                                        // Show open file dialog box
            Nullable<bool> result = openFileDialog.ShowDialog();
            if (result == true)
            {
                //Check File
                var fileName = openFileDialog.FileName;

                Console.WriteLine($"Selected File Name => {fileName}");
                Selected2Text = fileName;

                SortManager.Selected2FileName = fileName;

                if (SortManager.IsOkSelected2_File())
                {
                    Console.WriteLine("Check Selected Done");
                    IsEnabledSelecting2 = true;
                    IsEnabledSelecting3 = true;
                    IsEnabledSelecting4 = false;
                }
                else
                {
                    Console.WriteLine(SortManager.IsOkSelected2_ErrMsg);
                    //Error
                    IsEnabledSelecting2 = true;
                    IsEnabledSelecting3 = false;
                    IsEnabledSelecting4 = false;

                    return;
                }

                await Task.Run(() => SortManager.CollectFile2Datas());
            }

        });

        public RelayCommand Selected3Command => new RelayCommand(() =>
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                if(result == System.Windows.Forms.DialogResult.OK)
                {
                    Selected3Text = dialog.SelectedPath;
                    IsEnabledSelecting2 = true;
                    IsEnabledSelecting3 = true;
                    IsEnabledSelecting4 = true;
                }
            }

        });

        public RelayCommand StartConvertCommand => new RelayCommand(() =>
        {
            // Check ID Is All Found
            SortManager.CheckIDsIsAllFound();

            // 修正OK IDs 的場別
            SortManager.FixedOkIDsDep();

            // 寫出已填寫的IDs Sort by Dep
            SortManager.WriteOKFile2Datas_SortByDep(Selected3Text, "已填寫名單.csv");

            // 寫出錯誤的IDs
            SortManager.WriteUnKnownIDs_SortByID(Selected3Text, "已填寫但工號錯誤名單.csv");

            // 寫出尚未填寫的IDs
            SortManager.WriteUnDoIDs_SortByDep(Selected3Text, "尚未填寫名單.csv");

            // 名子正確但工號不正確修正
            SortManager.WriteFixedIDs(Selected3Text, "已填寫工號不正確名單(已修正工號).csv");

            MessageBox.Show(App.Current.MainWindow, "Convert Done!");

        });

        private string _selected1Text;

        public string Selected1Text
        {
            get { return _selected1Text; }
            set { _selected1Text = value; base.RaisePropertyChanged(); }
        }

        private string _selected2Text;

        public string Selected2Text
        {
            get { return _selected2Text; }
            set { _selected2Text = value; base.RaisePropertyChanged(); }
        }

        private string _selected3Text;

        public string Selected3Text
        {
            get { return _selected3Text; }
            set { _selected3Text = value; base.RaisePropertyChanged(); }
        }

        private string _selected4Text;

        public string Selected4Text
        {
            get { return _selected4Text; }
            set { _selected4Text = value; base.RaisePropertyChanged(); }
        }

    }
}