using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using EGClassroom.Models;
using System.Windows.Input;
using System.ComponentModel;

namespace EGClassroom.ViewModels.Interfaces
{
    public interface IAnswerViewModel
    {
        ObservableCollection<Answer> GetAnswers();
    }
}
