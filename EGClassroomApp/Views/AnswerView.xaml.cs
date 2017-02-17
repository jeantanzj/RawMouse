using EGClassroom.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EGClassroom.Views
{
    /// <summary>
    /// Interaction logic for AnswerView.xaml
    /// </summary>
    public partial class AnswerView : UserControl
    {
        public AnswerView()
        {
            InitializeComponent();
            this.Dispatcher.ShutdownStarted += Dispatcher_ShutdownStarted;
        }

        private void Dispatcher_ShutdownStarted(object sender, EventArgs e)
        {
            try
            {
                var obj = this.DataContext as AnswersViewModel;
                obj.StopMouse();
                //do something to the mouse
                //MouseCapture.RemoveOnMouseClicked();
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.Print("AnswerView Could not close mouse input. Failing silently: " + err.Message);
            }
        }
    }
}
