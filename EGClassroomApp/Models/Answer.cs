using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EGClassroom.Models.Interfaces;
using System.ComponentModel;
using EGClassroom.ViewModels;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

namespace EGClassroom.Models
{
    public class Answer : BaseModel,IAnswer
    {
        private int _questionID;
        private string _deviceID;
        private string _studentName;
        private string _studentAnswer;
        private ViewModels.AnswerStatusEnum _status;

        public int QuestionID
        {
            get
            {
                return _questionID;
            }

            set
            {
                _questionID = value;
                OnPropertyChanged();
            }
        }

        public string DeviceID
        {
            get
            {
                return _deviceID;
            }

             set
            {
                _deviceID = value;
                OnPropertyChanged();
            }
        }

        public string StudentName
        {
            get
            {
                return _studentName;
            }

            private set
            {
                _studentName = value;
                OnPropertyChanged();
            }
        }

        public void setStudentName(ObservableCollection<RegisteredDevice> regDevices)
        {
            StudentName = (from dev in regDevices where dev.DeviceID == _deviceID select dev.Name).FirstOrDefault();
        }

        public string StudentAnswer
        {
            get
            {
                return _studentAnswer;
            }

             set
            {
                _studentAnswer = value;
                OnPropertyChanged();
            }
        }

        public AnswerStatusEnum Status
        {
            get
            {
                return _status;
            }

             set
            {
                _status = value;
                OnPropertyChanged();
            }
        }

        public override string ToString()
        {
            return String.Format("{0}, {1}, {2}, {3} ", _questionID, _deviceID, _studentName, _studentAnswer); 
        }


    }

}
