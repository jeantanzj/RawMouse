using EGClassroom.ViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Input;
using EGClassroom.ViewModels.Commands;
using EGClassroom.Models;

using System.Windows;
using log4net;
using System.Reflection;

namespace EGClassroom.ViewModels
{
    public class RegisteredDevicesViewModel : BaseViewModel, IRegisteredDeviceViewModel
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static ObservableCollection<RegisteredDevice> _regDevices;
        private ICommand _loadDevicesCommand;
        private RelayCommand _registerCommand;
        private RelayCommand _chooseLocalFileCommand;

        //private RelayCommand _choosePPTCommand;
        //private static string _pptFilePath = "...";
        private static string _pptWebAddress = "https://docs.google.com/presentation/d/1xjzWvL0Rk7pqMZBrdfdgMPwBbij3HO6Xod-A0GwpsAg/present#slide=id.p";
        private static string _messages = "No messages";
        private MouseCapture _mc;

        public RegisteredDevicesViewModel( ){
            _loadDevicesCommand = new LoadDevicesCommand();
            _regDevices = GetRegisteredDevices();
            //_mc = MouseCapture.Instance;
            _mc = new MouseCapture();
            StartMouse();
        }


        public static ObservableCollection<RegisteredDevice> RegDevices
        {
            get { return _regDevices; }
        }
        public ObservableCollection<RegisteredDevice> GetRegisteredDevices()
        {
            if (_regDevices == null) _regDevices = new ObservableCollection<RegisteredDevice>();
            if ( _regDevices.Count ==0)
            {
                _loadDevicesCommand.Execute(_regDevices);
            }
            return _regDevices;
        }

        //public string PPTFilePath
        //{
        //    get { return _pptFilePath;  }
        //    set {
        //        if (value != _pptFilePath)
        //        {
        //            _pptFilePath = value;
        //            OnPropertyChanged();
        //        }
        //    }
        //}
        public string PPTWebAddress
        {
            get { return _pptWebAddress; }
            set
            {
                if (value != _pptWebAddress)
                {
                    _pptWebAddress = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Messages
        {
            get { return _messages;  }
            set
            {
                if (value != _messages)
                {
                    _messages = value;
                    OnPropertyChanged();
                }
            }
        }


       
        public RelayCommand RegisterCommand
        {
            get
            {
                return _registerCommand ?? (_registerCommand = new RelayCommand(
              param => doRegisterMouseClick() ));
            }
        }




        public RelayCommand ChooseLocalFileCommand
        {
            get
            {
                return _chooseLocalFileCommand ?? (_chooseLocalFileCommand = new RelayCommand(
              param => showChooseLocalFileDialog()));
            }
        }

        private void showChooseLocalFileDialog()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = "Portable Document Format|*.pdf";
            dlg.Filter = "Portable Document Format|*.pdf";


            Nullable<bool> result = dlg.ShowDialog();

            if (result.HasValue && result.Value)
            {
                PPTWebAddress = String.Format("file:///{0}",dlg.FileName);
            }

        }
        internal void StopMouse()
        {
            _mc.RemoveOnMouseClicked();
        }
        internal bool StartMouse()
        {
            _mc.RemoveOnMouseClicked();
            return _mc.AddOnMouseClicked();
        }

        public void Reset()
        {
            RegDevices.Clear();
            return;
        }
        public static void doRegisterMouseClick(string deviceID)
        {
            
            RegisteredDevice device = new RegisteredDevice() { DeviceID = deviceID, Name = "Student_" + _regDevices.Count };
            if (_regDevices.Count() == 0) device.Role = RoleEnum.TEACHER;
            _regDevices.Add(device);
            log.Debug("Registered: " + device);

        }
        private void doRegisterMouseClick()
        {
           
           if (string.IsNullOrEmpty(MouseCapture.deviceHandle)) return;
           int existsDevice = (from dev in _regDevices where dev.DeviceID == MouseCapture.deviceHandle select dev).Count();
           if (existsDevice == 0)
            {
                doRegisterMouseClick(MouseCapture.deviceHandle);
            }
        }



    }
}
