using EGClassroom.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace EGClassroom.ViewModels.Commands
{
    public class LoadDevicesCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public void Execute(object parameter)
        {
            var regList = parameter as ObservableCollection<RegisteredDevice>;
            if(regList != null)
            {
                var loadedList = RegisteredDevices.LoadDevices();
                foreach (var device in loadedList)
                {
                    regList.Add(device);
                }
            }
        }
    }
}
