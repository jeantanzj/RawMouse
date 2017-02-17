using EGClassroom.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EGClassroom.Views
{
    /// <summary>
    /// Interaction logic for QuizView.xaml
    /// </summary>
    public partial class QuizView : UserControl
    {
        public QuizView()
        {
            InitializeComponent();
            this.DataContext = CompositeViewModel.Instance;
            this.Dispatcher.ShutdownStarted += Dispatcher_ShutdownStarted;
        }

        private void Dispatcher_ShutdownStarted(object sender, EventArgs e)
        {
            try
            {
                var obj = this.DataContext as QuizViewModel;
                obj.StopMouse();
                //do something to the mouse
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.Print("Q Could not close mouse input. Failing silently: " + err.Message);
            }
        }
    }
}
