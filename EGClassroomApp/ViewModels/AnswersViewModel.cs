using System;
using System.Collections.Generic;
using System.Linq;
using EGClassroom.ViewModels.Interfaces;
using System.Collections.ObjectModel;
using EGClassroom.Models;
using EGClassroom.ViewModels.Commands;
using System.ComponentModel;
using log4net;
using System.Reflection;
using System.Collections.Specialized;
using System.Windows.Forms;
using System.IO;
using EGClassroom.Helper;

namespace EGClassroom.ViewModels
{
    public class AnswersViewModel : BaseViewModel, IAnswerViewModel
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static ObservableCollection<Answer> _answers;
        private static ObservableCollection<Record> _records;
        private static List<string> _correctAnswers;
        private RelayCommand _exportAnswersCommand;
        private RelayCommand _receiveInputCommand;
        private RelayCommand _stopInput_A_Command;
        private RelayCommand _stopInput_B_Command;
        private RelayCommand _stopInput_C_Command;
        //private ICommand _loadCommand;

        private static Dictionary<string, string> _mapping;
        private static int _questionId = 0;
        private static bool _inQuizMode = false;
        private static string _currentCorrectAnswer = null;


        private MouseCapture _mc;

       
        public AnswersViewModel()
        {
            //_loadCommand = new LoadAnswersCommand();
            _answers = Answers;
            _answers.CollectionChanged += _answers_CollectionChanged;
            _mapping = new Dictionary<string, string>() {
                { "RI_MOUSE_LEFT_BUTTON_DOWN", "A" },
                { "RI_MOUSE_MIDDLE_BUTTON_DOWN", "B" },
                { "RI_MOUSE_RIGHT_BUTTON_DOWN","C"}
            };
            //{ "RI_MOUSE_BUTTON_4_DOWN", "D" }, // D -- some alternatives since these buttons are uncommon
            //{ "RI_MOUSE_BUTTON_5_DOWN","D"},
            //{ "RI_MOUSE_WHEEL","D" } };
            _correctAnswers = new List<string>();
            _mc = new MouseCapture();
            //_mc = MouseCapture.Instance;
            StopMouse();
          
        }

        private  void _answers_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateNumAnswers();
            UpdateChartDetails();
            if (e.NewItems != null)
                foreach (Answer item in e.NewItems)
                    item.PropertyChanged += answer_PropertyChanged;

            if (e.OldItems != null)
                foreach (Answer item in e.OldItems)
                    item.PropertyChanged -= answer_PropertyChanged;
        }

        private  void answer_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged("Answers");
        }

#region Members
        public static ObservableCollection<Answer> Answers
        {
            get
            {
                return _answers ?? (_answers = new ObservableCollection<Answer>());
            }
        }

        public ObservableCollection<Record> Records
        {
            get
            {
                return _records ?? (_records = new ObservableCollection<Record>());
            }

            set
            {
                _records = value;
                OnPropertyChanged();
            }
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

        public string CurrentCorrectAnswer
        {
            get
            {
                return _currentCorrectAnswer;
            }

            set
            {
                _currentCorrectAnswer = value;
                OnPropertyChanged();
            }
        }

#endregion Members


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
            Answers.Clear();
            Records.Clear();
            _correctAnswers.Clear();
            InQuizMode = false;
            QuestionID = 0;
        }

        private void UpdateNumAnswers()
        {
            OnPropertyChanged("NumAnswers");
        }
        public static void AddAnswer(string deviceHandle, string deviceMessage)
        {
            if (!_mapping.ContainsKey(deviceMessage)) return;
            int existsAnswer = (from ans in _answers where ans.QuestionID == _questionId && ans.DeviceID == deviceHandle select ans).Count();
           
            if (existsAnswer == 0)
            {
                RegisteredDevice device = (from dev in RegisteredDevicesViewModel.RegDevices
                                           where dev.DeviceID == deviceHandle
                                           select dev).FirstOrDefault();
                if (device == null)
                {
                    RegisteredDevicesViewModel.doRegisterMouseClick(deviceHandle);
                }
                else if (device.Role == RoleEnum.TEACHER) {
                    return;
                }

                string studentAnswer = getStudentAnswer(deviceMessage);
                Answer ans = new Answer() { QuestionID = _questionId, DeviceID = deviceHandle, StudentAnswer = studentAnswer };
                ans.setStudentName(RegisteredDevicesViewModel.RegDevices);
                _answers.Insert(0, ans );
                log.Debug("Inserted: " + ans);

            };
        }

        private static string existsStudent(string deviceID)
        {

            var sname = from dev in RegisteredDevicesViewModel.RegDevices where dev.DeviceID == deviceID select dev.Name;
            if (sname.Count() == 0)
            {
                RegisteredDevicesViewModel.doRegisterMouseClick(deviceID);
                sname = from dev in RegisteredDevicesViewModel.RegDevices where dev.DeviceID == deviceID select dev.Name;
            }
            return sname.First().ToString();

        }
        private static string getStudentAnswer(string message)
        {

            string o = "";
            _mapping.TryGetValue(message, out o);
            return o;

        }
        private void checkAnswersAgainst(string correctAnswer)
        {
            _correctAnswers.Add(correctAnswer);
            CurrentCorrectAnswer = "The correct answer was: " + correctAnswer;

            _answers.Where(ans => ans.QuestionID == _questionId & ans.StudentAnswer == correctAnswer).ToList().ForEach(
                 f =>
                 {
                     f.Status = AnswerStatusEnum.CORRECT;
                 }
             );
            _answers.Where(ans => ans.QuestionID == _questionId & ans.StudentAnswer != correctAnswer).ToList().ForEach(
                 f =>
                 {
                     f.Status = AnswerStatusEnum.INCORRECT;
                 }
            );

            computeRecords();
            UpdateChartDetails();
        }

        private void computeRecords()
        {
            var recs = _answers.GroupBy(grp => new { grp.DeviceID })
                .Select(g => new {
                    g.Key.DeviceID,
                    ResultsString = string.Join(",", g.OrderBy(h=>h.QuestionID).Select(h => h.StudentAnswer)) })
                .Join(RegisteredDevicesViewModel.RegDevices, a => a.DeviceID, b => b.DeviceID,
                    (a, b) => new { Name=b.Name, ResultsString = a.ResultsString, ImagePath = b.ImagePath })
                .OrderBy(i => i.Name);

             
            foreach (var rec in recs)
            {
                Record oldRecord = _records.Where(r => r.Name == rec.Name).Select(r => r).SingleOrDefault();
                if(oldRecord == null)
                {
                    oldRecord = new Record() { Name = rec.Name, ResultsString = rec.ResultsString , Image = rec.ImagePath};
                    _records.Add(oldRecord);
                }
                else
                {
                    oldRecord.ResultsString = rec.ResultsString;
                }
            }

            for (int i = 0; i < _records.Count(); i++) 
            {
                int total = _correctAnswers.Count();
                int numCorrect = total - CountDifferences(_records[i].ResultsString.Split(',').ToList(), _correctAnswers);
                _records[i].Score = string.Format("{0} of {1} ({2:p2})", numCorrect, total, (float)numCorrect / total);
            }
        }
        private int CountDifferences(List<string> x, List<string> y)
        {
            return (x.Zip(y, (a, b) => a.Equals(b) ? 0 : 1).Sum());
        }

        private void UpdateChartDetails()
        {
            OnPropertyChanged("ChartData");
        }
        public  IEnumerable<ChartDetail> GetChartDetails()
        {
            string c = null;
            try
            {
                c = _correctAnswers.ElementAt(_questionId - 1);
            }
            catch(ArgumentOutOfRangeException ex)
            {
                //fail silently
            }
           return _answers.Where(x => x.QuestionID == _questionId).GroupBy(x => x.StudentAnswer).OrderBy(x=>x.Key).Select(g => 
                new ChartDetail { Key = g.Key, Value=g.Count(), IsCorrect = g.Key == c});
        }

        public IEnumerable<ChartDetail> ChartData
        {
            get
            {
                return GetChartDetails();
            }
           
        }

        #region commands
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
                         CurrentCorrectAnswer = null;
                         UpdateNumAnswers();
                         log.Debug(String.Format("Start getting answers for qn {0} ", _questionId));
                         return;
                     }
                 }));
            }
        }

        public RelayCommand StopInput_A_Command
        {
            get
            {
                return _stopInput_A_Command ?? (_stopInput_A_Command = new RelayCommand(param => {
                    StopMouse();
                    InQuizMode = false;
                    checkAnswersAgainst("A");
                }));
            }
        }
        public RelayCommand StopInput_B_Command
        {
            get
            {
                return _stopInput_B_Command ?? (_stopInput_B_Command = new RelayCommand(param => {
                    StopMouse();
                    InQuizMode = false;
                    checkAnswersAgainst("B");
                }));
            }
        }
        public RelayCommand StopInput_C_Command
        {
            get
            {
                return _stopInput_C_Command ?? (_stopInput_C_Command = new RelayCommand(param => {
                    StopMouse();
                    InQuizMode = false;
                    checkAnswersAgainst("C");
                }));
            }
        }

      
        #endregion commands


        #region Dialog
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
#endregion Dialog

 
}
