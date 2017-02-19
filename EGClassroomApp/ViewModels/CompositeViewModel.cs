using EGClassroom.ViewModels.Commands;
using EGClassroom.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EGClassroom.ViewModels
{
    public class CompositeViewModel : BaseViewModel
    {
        public static AnswersViewModel answersVM { get; set; }
        public static RegisteredDevicesViewModel regDevicesVM { get; set; }
        public static QuizViewModel quizVM { get; set; }
        private RelayCommand _toRegisteredDeviceViewCommand;
        private RelayCommand _toAnswerViewCommand;
        private RelayCommand _toAnswerWindowViewCommand;
        private RelayCommand _toQuizViewAndToAnswerWindowViewCommand;
        private RelayCommand _toQuizViewCommand;
        private RelayCommand _resetAllCommand;
        private static object _selectedViewModel;
        private static object _answerWindowViewModel;
        
        
        private CompositeViewModel()
        {
            
            answersVM = new AnswersViewModel();
            regDevicesVM = new RegisteredDevicesViewModel();
            quizVM = new QuizViewModel(regDevicesVM.PPTWebAddress);
           
        }

        private static volatile CompositeViewModel _instance;
        private static object _syncObj = new object();
        public static CompositeViewModel Instance
        {
            get
            {
                lock (_syncObj)
                {
                    return _instance ?? (_instance = new CompositeViewModel());
                }
            }
        }
       

        public RelayCommand ResetAllCommand
        {
            get
            {
                return _resetAllCommand ?? (_resetAllCommand = new RelayCommand(param =>
                {
                    RegisteredDevicesViewModel.RegDevices.Clear();
                    AnswersViewModel.Answers.Clear();
                    answersVM.Records.Clear();
                    answersVM.QuestionID = 0;
                }));
            }
        }

        public object SelectedViewModel

        {
            get { return _selectedViewModel; }

            set { _selectedViewModel = value; OnPropertyChanged(); }

        }
        public object AnswerWindowViewModel
        {
            get { return _answerWindowViewModel; }

            set { _answerWindowViewModel = value; OnPropertyChanged(); }
        }
        public RelayCommand ToRegisteredDeviceViewCommand
        {
            get
            {

                return _toRegisteredDeviceViewCommand ?? (_toRegisteredDeviceViewCommand = new RelayCommand(param => goToRegisteredDeviceView()));
            }
        }

        public RelayCommand ToAnswerViewCommand
        {
            get
            {
                return _toAnswerViewCommand ?? (_toAnswerViewCommand = new RelayCommand(param => goToAnswerView()
                ));
            }
        }

        public RelayCommand ToAnswerWindowViewCommand
        {
            get
            {
                return _toAnswerWindowViewCommand ?? (_toAnswerWindowViewCommand = new RelayCommand(param => goToAnswerWindowView()
                ));
            }
        }

        public RelayCommand ToQuizViewAndToAnswerWindowViewCommand
        {
            get
            {
                return _toQuizViewAndToAnswerWindowViewCommand ?? (_toQuizViewAndToAnswerWindowViewCommand = new RelayCommand(param => {
                    goToQuizView();
                    goToAnswerWindowView();
                }
                ));
            }
        }

        public RelayCommand ToQuizViewCommand
        {
            get
            {
                return _toQuizViewCommand ?? (_toQuizViewCommand = new RelayCommand(param => goToQuizView()
                ));
            }
        }

        private void goToAnswerWindowView()
        {
            AnswerWindowViewModel = answersVM;
            OpenAnswerWindowIfClosed();


        }
        private void goToAnswerView()
        {
            OpenMainWindowIfClosed();
            SelectedViewModel = answersVM;
            

        }
        public void goToRegisteredDeviceView()
        {
            OpenMainWindowIfClosed();
            SelectedViewModel = regDevicesVM;
            
        }

        private void goToQuizView()
        {
            OpenMainWindowIfClosed();
            SelectedViewModel = quizVM;
            
        }

        private static bool IsWindowOpen<T>(string name = "") where T : Window
        {
            return string.IsNullOrEmpty(name)
               ? Application.Current.Windows.OfType<T>().Any()
               : Application.Current.Windows.OfType<T>().Any(w => w.Name.Equals(name));
        }

        private void OpenMainWindowIfClosed()
        {
            if (IsWindowOpen<MainWindow>())
            {
                Application.Current.Windows.OfType<MainWindow>().First().Activate();
                return;
            }
            MainWindow mwin = new MainWindow();
            mwin.DataContext = this;
            mwin.Show();

        }

        private void OpenAnswerWindowIfClosed()
        {
            if (IsWindowOpen<AnswerWindowView>())
            {
                Application.Current.Windows.OfType<AnswerWindowView>().First().Activate();
                return;
            }
            AnswerWindowView awin = new AnswerWindowView();
            awin.DataContext = this;
            awin.Show();
        }
    }
}
