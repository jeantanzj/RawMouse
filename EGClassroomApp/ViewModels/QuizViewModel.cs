using EGClassroom.Models;
using EGClassroom.ViewModels.Commands;
using EGClassroom.ViewModels.Interfaces;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EGClassroom.ViewModels
{
    public class QuizViewModel: BaseViewModel, IQuizViewModel
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        //private string _pptFilePath = new RegisteredDevicesViewModel().PPTFilePath;
        private string _pptWebAddress = "";//new RegisteredDevicesViewModel().PPTWebAddress;
        private RelayCommand _receiveInputCommand;
        private RelayCommand _stopInputCommand;
        private static int _questionId = 0;
        private static bool _inQuizMode = false;
        private MouseCapture _mc;

       

        public QuizViewModel(string pptWebAddress)
        {
            _mc = new MouseCapture();
            //_mc = MouseCapture.Instance;
            StopMouse();
            _pptWebAddress = pptWebAddress;
        }
        //public string PPTFilePath
        //{
        //    get { return _pptFilePath;  }
        //}
        internal void StopMouse()
        {
            _mc.RemoveOnMouseClicked();
        }
        internal bool StartMouse()
        {
            _mc.RemoveOnMouseClicked();
            return _mc.AddOnMouseClicked();
        }

        public bool InQuizMode
        {
            get
            {
                return _inQuizMode;
            }
            set
            {
                if (value != _inQuizMode)
                {
                    _inQuizMode = value;
                    OnPropertyChanged();
                }
            }
        }
        public static bool GetInQuizMode()
        {
            return _inQuizMode;
        }

        public static int GetQuestionID()
        {
            return _questionId;
        }
        public int QuestionID
        {
            get { return _questionId; }
            set
            {
                if(value != _questionId)
                {
                    _questionId = value;
                    OnPropertyChanged();
                }
            }
        }
        public RelayCommand ReceiveInputCommand
        {
            get
            {
                return _receiveInputCommand ?? (_receiveInputCommand = new RelayCommand(
                 param =>
                 {  
                     if (StartMouse() == true)
                     {
                         
                         QuestionID += 1;
                         InQuizMode = true;
                         log.Debug(String.Format("Start getting answers for qn {0} ", _questionId));
                         return;
                     }
                 }));
            }
        }

      

        public RelayCommand StopInputCommand
        {
            get
            {
                return _stopInputCommand ?? (_stopInputCommand = new RelayCommand(param => {
                    StopMouse();
                    InQuizMode = false;
                }));
            }
        }

        public string PPTWebAddress
        {
            get { return _pptWebAddress; }
        }

        /** FOR BINDING WEB ADDRESS**/
        public static readonly DependencyProperty BindableSourceProperty =
                    DependencyProperty.RegisterAttached("BindableSource", typeof(object), typeof(QuizViewModel), new UIPropertyMetadata(null, BindableSourcePropertyChanged));

        public static object GetBindableSource(DependencyObject obj)
        {
            return (string)obj.GetValue(BindableSourceProperty);
        }

        public static void SetBindableSource(DependencyObject obj, object value)
        {
            obj.SetValue(BindableSourceProperty, value);
        }

        public static void BindableSourcePropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            WebBrowser browser = o as WebBrowser;
            if (browser == null) return;

            Uri uri = null;

            if (e.NewValue is string)
            {
                var uriString = e.NewValue as string;
                uri = string.IsNullOrWhiteSpace(uriString) ? null : new Uri(uriString);
            }
            else if (e.NewValue is Uri)
            {
                uri = e.NewValue as Uri;
            }
            try
            {
                browser.Source = uri;
            }
            catch(System.InvalidOperationException err)
            {
                log.Error(String.Format("Error parsing uri: {0}", uri));
            }
        }
/****/

    }
}
