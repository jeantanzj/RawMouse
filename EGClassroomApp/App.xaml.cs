using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;


namespace EGClassroom
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                MainWindow win = new MainWindow();
                ViewModels.CompositeViewModel vm = new ViewModels.CompositeViewModel();
                win.DataContext = vm;
                vm.goToRegisteredDeviceView();
                win.Show();
                
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }
    }
}
