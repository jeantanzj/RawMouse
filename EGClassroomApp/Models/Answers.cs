using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EGClassroom.Models
{
    public class Answers
    {
        public static IEnumerable<Answer> LoadAnswersFromDataSource()
        {
            var pricetmp = DateTime.Now.Second; // just to get some number
            var qtytmp = (DateTime.Now.Second + 5) * 1000; // just to get some number

            return new List<Answer>()
            {
                new Answer(){QuestionID=1, DeviceID="001", StudentName="John", StudentAnswer="A", Status=ViewModels.AnswerStatusEnum.PENDING},
                new Answer(){QuestionID=1, DeviceID="002", StudentName="Jane", StudentAnswer="C", Status=ViewModels.AnswerStatusEnum.PENDING},
                new Answer(){QuestionID=1, DeviceID="003", StudentName="Jenny", StudentAnswer="A", Status=ViewModels.AnswerStatusEnum.PENDING},
            };
        }
    }
}
