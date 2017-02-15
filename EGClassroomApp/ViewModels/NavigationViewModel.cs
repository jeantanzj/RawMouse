using EGClassroom.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EGClassroom.ViewModels
{
    public class NavigationViewModel: BaseViewModel
    {
        private RelayCommand _toRegisteredDeviceViewCommand;
        private RelayCommand _toAnswerViewCommand;
        private RelayCommand _toQuizViewCommand;
        private static object _selectedViewModel;
       
        public NavigationViewModel()
        {
            // goToRegisteredDeviceView();
            goToAnswerView();
        }
       

        public object SelectedViewModel

        {
            get { return _selectedViewModel; }

            set { _selectedViewModel = value; OnPropertyChanged(); }

        }

        public RelayCommand ToRegisteredDeviceViewCommand {
            get {
                
                return _toRegisteredDeviceViewCommand ?? (_toRegisteredDeviceViewCommand = new RelayCommand(param => goToRegisteredDeviceView() ));
            } }

        public RelayCommand ToAnswerViewCommand {
            get{
                return _toAnswerViewCommand ?? (_toAnswerViewCommand = new RelayCommand(param => goToAnswerView()
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

        private void goToAnswerView()
        {
            SelectedViewModel = new AnswersViewModel();
            
        }
        private void goToRegisteredDeviceView()
        {
            SelectedViewModel = new RegisteredDevicesViewModel();
            
        }

        private  void goToQuizView()
        {
            SelectedViewModel = new QuizViewModel();
        }


    }
}
