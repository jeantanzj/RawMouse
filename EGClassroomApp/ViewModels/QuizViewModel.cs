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
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        //private string _pptFilePath = new RegisteredDevicesViewModel().PPTFilePath;
        private string _pptWebAddress = "";//new RegisteredDevicesViewModel().PPTWebAddress;
     
        public QuizViewModel(string pptWebAddress)
        {

            _pptWebAddress = pptWebAddress;
        }
        //public string PPTFilePath
        //{
        //    get { return _pptFilePath;  }
        //}
     

        public string PPTWebAddress
        {
            get { return _pptWebAddress; }
        }

        /** FOR BINDING WEB ADDRESS**/
        public static readonly DependencyProperty BindableSourceProperty =
                    DependencyProperty.RegisterAttached("BindableSource", typeof(object), typeof(QuizViewModel), new UIPropertyMetadata(null, BindableSourcePropertyChanged));

        public static object GetBindableSource(DependencyObject obj)
        {
            return (string)obj.GetValue(BindableSourceProperty);
        }

        public static void SetBindableSource(DependencyObject obj, object value)
        {
            obj.SetValue(BindableSourceProperty, value);
        }

        public static void BindableSourcePropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            WebBrowser browser = o as WebBrowser;
            if (browser == null) return;

            Uri uri = null;

            if (e.NewValue is string)
            {
                var uriString = e.NewValue as string;
                uri = string.IsNullOrWhiteSpace(uriString) ? null : new Uri(uriString);
            }
            else if (e.NewValue is Uri)
            {
                uri = e.NewValue as Uri;
            }
            try
            {
                browser.Source = uri;
            }
            catch(System.InvalidOperationException err)
            {
                log.Error(String.Format("Error parsing uri: {0}", uri));
            }
        }
/****/

    }
}
