using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EGClassroom.ViewModels.Interfaces;
using System.Windows.Input;
using System.Collections.ObjectModel;
using EGClassroom.Models;
using EGClassroom.ViewModels.Commands;
using System.ComponentModel;
using System.Windows.Threading;
using EGClassroom.ViewModels;
using System.Windows.Controls;
using log4net;
using System.Reflection;
using System.Collections.Specialized;
using System.Windows.Forms;

namespace EGClassroom.ViewModels
{
    public class AnswersViewModel : BaseViewModel, IAnswerViewModel
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static ObservableCollection<Answer> _answers;
        private RelayCommand _exportAnswersCommand;
        private RelayCommand _receiveInputCommand;
        private RelayCommand _stopInputCommand;
        //private ICommand _loadCommand;

        private static Dictionary<string, string> _mapping;
        private static int _questionId = 0;
        private static bool _inQuizMode = false;


        private MouseCapture _mc;

       
        public AnswersViewModel()
        {
            //_loadCommand = new LoadAnswersCommand();
            _answers = GetAnswers();
            AddOnAnswers_CollectionChanged();
            _mapping = new Dictionary<string, string>() {
                { "RI_MOUSE_LEFT_BUTTON_DOWN", "A" },
                { "RI_MOUSE_MIDDLE_BUTTON_DOWN", "B" },
                { "RI_MOUSE_RIGHT_BUTTON_DOWN","C"},
                { "RI_MOUSE_BUTTON_4_DOWN", "D" }, // D -- some alternatives since these buttons are uncommon
                { "RI_MOUSE_BUTTON_5_DOWN","D"},
                { "RI_MOUSE_WHEEL","D" } };
            _mc = new MouseCapture();
            //_mc = MouseCapture.Instance;
            StopMouse();
          
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
        public static ObservableCollection<Answer> Answers { get { return _answers; } }

        public ObservableCollection<Answer> GetAnswers()
        {
            if (_answers == null)
            {
                _answers = new ObservableCollection<Answer>();
            }
            //if ( _answers.Count ==0)
            //{
            //    _loadCommand.Execute(_answers);
            //}

            return _answers;
        }

        private object _eventLock = new object();
        private void AddOnAnswers_CollectionChanged()
        {

            lock (_eventLock)
            {
                _answers.CollectionChanged -= Answers_CollectionChanged;
                _answers.CollectionChanged += Answers_CollectionChanged;
            }

        }
        private void RemoveOnAnswers_CollectionChanged()
        {
            lock (_eventLock)
            {
                _answers.CollectionChanged -= Answers_CollectionChanged;
            }

        }
        void Answers_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("NumAnswers");
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
                if (value != _questionId)
                {
                    _questionId = value;
                    OnPropertyChanged();
                }
            }
        }

        public int NumAnswers
        {
            get
            {              
                int numAns = (from ans in _answers where ans.QuestionID == _questionId select ans).Count();
                System.Diagnostics.Debug.Print("numAns: " + numAns);
                return numAns;
            }

        }

        public static void AddAnswer(string deviceHandle, string deviceMessage)
        {
           
            int existsAnswer = (from ans in _answers where ans.QuestionID == _questionId && ans.DeviceID == deviceHandle select ans).Count();
            if (existsAnswer == 0)
            {
                string studentName = getStudentName(deviceHandle);
                string studentAnswer = getStudentAnswer(deviceMessage);
                _answers.Insert(0, new Answer() { QuestionID = _questionId, DeviceID = deviceHandle, StudentName = studentName, StudentAnswer = studentAnswer });
                log.Debug(String.Format("Inserted: {0}, {1}, {2}, {3} ", _questionId, deviceHandle, studentName, studentAnswer));

            };
        }

        private static string getStudentName(string deviceID)
        {

            var sname = from dev in RegisteredDevicesViewModel.RegDevices where dev.DeviceID == deviceID select dev.StudentName;
            if (sname.Count() == 0)
            {
                RegisteredDevicesViewModel.doRegisterMouseClick(deviceID);
                sname = from dev in RegisteredDevicesViewModel.RegDevices where dev.DeviceID == deviceID select dev.StudentName;
            }
            return sname.First().ToString();

        }
        private static string getStudentAnswer(string message)
        {

            string o = "";
            _mapping.TryGetValue(message, out o);
            return o;

        }

        public RelayCommand ExportAnswersCommand
        {
            get
            {
               
                    return _exportAnswersCommand ?? (_exportAnswersCommand = new RelayCommand(param =>
                    {
                        showChooseCSVDialog();
                    }));
                
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
        private void showChooseCSVDialog()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = "Comma-Separated Values|*.csv";
            saveFileDialog.Filter = "Comma-Separated Values|*.csv";
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            DialogResult result = saveFileDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                WriteToFile.CollectionToCSV(Answers, @saveFileDialog.FileName);
            }
        }
    }
}
