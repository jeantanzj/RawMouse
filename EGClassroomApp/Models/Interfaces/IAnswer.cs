using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EGClassroom.Models.Interfaces
{
    public interface IAnswer
    {
        int QuestionID { get; set; }
        string DeviceID { get; set; }
        string StudentName { get; set; }
        string StudentAnswer { get; set; }
        
       
    }
}
