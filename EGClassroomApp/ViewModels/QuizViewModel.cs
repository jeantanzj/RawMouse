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
        /*The Quiz View turned out to more easily use data from the answers view model and registered device view model;
         * so this is empty*/
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public void Reset()
        {
            return;
        }

      

    }
}
