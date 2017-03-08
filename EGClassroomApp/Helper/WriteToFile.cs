using EGClassroom.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EGClassroom.Helper
{
    public class WriteToFile
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static void CollectionToCSV(ObservableCollection<Answer> answers, string filePath)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("QuestionID,DeviceID,StudentName,StudentAnswer");
            foreach(Answer ans in answers)
            {
                sb.AppendLine(String.Format("{0},{1},{2},{3}",ans.QuestionID, ans.DeviceID, ans.StudentName, ans.StudentAnswer));
            }
            using(System.IO.StreamWriter writer = new System.IO.StreamWriter(filePath, false))
            {
                writer.WriteLine(sb.ToString());
            }
            log.Info("Exported answers to csv located at: " + filePath);
        }

    }
}
