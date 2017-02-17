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
using System.Windows.Shapes;

namespace EGClassroom.Views
{
    /// <summary>
    /// Interaction logic for RegisteredDeviceView.xaml
    /// </summary>
    public partial class RegisteredDeviceView : UserControl
    {
        public RegisteredDeviceView()
        {
            InitializeComponent();
            
            this.Dispatcher.ShutdownStarted += Dispatcher_ShutdownStarted;
        }

        private void Dispatcher_ShutdownStarted(object sender, EventArgs e)
        {
            try
            {
                var obj = this.DataContext as RegisteredDevicesViewModel;
                obj.StopMouse();
                //do something to the mouse
            }
            catch(Exception err)
            {
                System.Diagnostics.Debug.Print("RegistedDeviceView Could not close mouse input. Failing silently: " + err.Message);
            }
            
        }
    }
}
