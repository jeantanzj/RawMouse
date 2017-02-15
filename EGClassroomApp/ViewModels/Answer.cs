using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EGClassroom.Models.Interfaces;
using System.ComponentModel;

namespace EGClassroom.Models
{
    public class Answer : IAnswer//, INotifyPropertyChanged
    {
        public int QuestionID
        {
            get; set;
        }

        public string DeviceID
        {
            get; set;
        }

        public string StudentName
        {
            get; set;
        }

        public string StudentAnswer
        {
            get; set;
        }
        
       
    }
}
