using EGClassroom.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EGClassroom.Models.Interfaces
{
    public interface IAnswer
    {
        int QuestionID { get;  }
        string DeviceID { get;  }
        string StudentName { get;  }
        string StudentAnswer { get;  }
        AnswerStatusEnum Status { get;}
       
    }
}
