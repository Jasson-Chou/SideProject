using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GUIConsole.Model;
using System;
using System.Threading.Tasks;
using System.Timers;

namespace GUIConsole.ViewModel
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
        /// 
        public MainViewModel()
        {
            if (IsInDesignMode)
            {
                // Code runs in Blend --> create design time data.
            }
            else
            {
                // Code runs "for real"
                MainMenu();
            }
        }

        public async void MainMenu()
        {
            bool isExit = false;
            MainMenuModel MenuModel = new MainMenuModel();
            do
            {
                ConsoleWriteLine(MenuModel.ToString());
                string input = await AsyncInput();
                if(int.TryParse(input, out int input_int))
                {
                    if(MenuModel.TryGetMenuType(input_int, out MainMenuModel.Menu menu))
                    {
                        switch(menu)
                        {
                            case MainMenuModel.Menu.M1:
                                ConsoleClear();
                                ConsoleWriteLine($"User Select {MainMenuModel.Menu.M1}");
                                //To Do...
                                ConsoleWriteLine($"Exit {MainMenuModel.Menu.M1}");

                                break;
                            case MainMenuModel.Menu.M2:
                                ConsoleClear();
                                ConsoleWriteLine($"User Select {MainMenuModel.Menu.M2}");
                                //To Do...
                                ConsoleWriteLine($"Exit {MainMenuModel.Menu.M2}");
                                
                                break;
                            case MainMenuModel.Menu.M3:
                                ConsoleClear();
                                ConsoleWriteLine($"User Select {MainMenuModel.Menu.M3}");
                                //To Do...
                                ConsoleWriteLine($"Exit {MainMenuModel.Menu.M3}");

                                break;
                            case MainMenuModel.Menu.Exit:
                                int sec = 5;
                                ConsoleWriteLine("5 Second to exit application.");
                                do
                                {
                                    ConsoleWriteLine($"{sec} Second...");
                                    await Task.Delay(1000);
                                    --sec;
                                } while (sec > 0);
                                App.Current.Shutdown(0);
                                break;
                        }
                    }
                    else
                    {
                        ConsoleWriteLine($"[User Input Error]=Unknown Menu Index[{input}]");
                        ConsoleWriteLine($"Try Again!");
                    }
                }
                else
                {
                    ConsoleWriteLine($"[User Input Error]={input}");
                    ConsoleWriteLine($"Try Again!");
                }
                ConsoleWriteLine($"");
            } while (!isExit);
        }




        private bool _isBusy;

        public bool IsBusy
        {
            get { return _isBusy; }
            set { _isBusy = value; base.RaisePropertyChanged(); base.RaisePropertyChanged(() => SendCommand); }
        }

        private volatile bool _isSend = false;

        public RelayCommand SendCommand => new RelayCommand(() =>
        {
#if DEBUG
            DebugWriteLine($"User Input = [{UserInput}]");
#endif
            _isSend = true;
            IsBusy = true;

        }, () => !IsBusy);

        private string _userInput;

        public string UserInput
        {
            get { return _userInput; }
            set { _userInput = value; base.RaisePropertyChanged(); }
        }

        private async Task<string> AsyncInput()
        {
            string input = string.Empty;
            IsBusy = false;
            do
            {
                await Task.Delay(20);
            } while (!_isSend);
            _isSend = false;
            input = UserInput;
            UserInput = string.Empty;
            return input;
        }

        private string _consoleMessage;

        public string ConsoleMessage
        {
            get { return _consoleMessage; }
            set { _consoleMessage = value; base.RaisePropertyChanged(); }
        }

        public void ConsoleWrite(string msg)
        {
            ConsoleMessage += msg;
        }

        public void ConsoleWriteLine(string msg)
        {
            ConsoleWrite(msg + Environment.NewLine);
        }

        public void ConsoleClear()
        {
            ConsoleMessage = string.Empty;
        }


#if DEBUG
        private void DebugWriteLine(string msg)
        {
            System.Diagnostics.Debug.WriteLine(msg);
        }
#endif
    }
}