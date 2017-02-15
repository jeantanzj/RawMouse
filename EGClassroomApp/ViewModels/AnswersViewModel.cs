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
        //private ICommand _loadCommand;

        private static Dictionary<string, string> _mapping;

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
            //mc = new MouseCapture();
            _mc = new MouseCapture();
            _mc.RemoveOnMouseClicked();
          
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


        public int NumAnswers
        {
            get
            {
                int questionId = QuizViewModel.GetQuestionID();
                int numAns = (from ans in _answers where ans.QuestionID == questionId select ans).Count();
                System.Diagnostics.Debug.Print("numAns: " + numAns);
                return numAns;
            }

        }

        public static void AddAnswer(string deviceHandle, string deviceMessage)
        {
            int questionId = QuizViewModel.GetQuestionID();
            int existsAnswer = (from ans in _answers where ans.QuestionID == questionId && ans.DeviceID == deviceHandle select ans).Count();
            if (existsAnswer == 0)
            {
                string studentName = getStudentName(deviceHandle);
                string studentAnswer = getStudentAnswer(deviceMessage);
                _answers.Insert(0, new Answer() { QuestionID = questionId, DeviceID = deviceHandle, StudentName = studentName, StudentAnswer = studentAnswer });
                log.Debug(String.Format("Inserted: {0}, {1}, {2}, {3} ", questionId, deviceHandle, studentName, studentAnswer));

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
