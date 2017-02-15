using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using EGClassroom.Models;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Threading;

namespace EGClassroom.ViewModels.Commands
{
    public class LoadAnswersCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public void Execute(object parameter)
        {
            var loadedAnswers = parameter as ObservableCollection<Answer>;
            if (loadedAnswers != null)
            {
                //TaskFactory taskfac = new TaskFactory();
                //taskfac.StartNew(() =>
                //    {
                //        while (true)
                //        {
                var answersList = Answers.LoadAnswersFromDataSource();
                            App.Current.Dispatcher.Invoke(new Action(() =>
                            {
                                loadedAnswers.Clear();
                                foreach (var ans in answersList)
                                    loadedAnswers.Add(ans);
                            }));

            //                Thread.Sleep(2000);
            //            }
            //        });
           }
        }
    }
}
