using EGClassroom.ViewModels;
using log4net;
using RawInput_dll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace EGClassroom
{
    public sealed  class MouseCapture
    {
        private static volatile MouseCapture instance; //singleton
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static string deviceHandle;
        public static string deviceMessage;
        private  readonly RawInput _rawinput;
        private static System.Windows.Interop.WindowInteropHelper _winHelper;
        const bool CaptureOnlyInForeground = true;
        private static int _boundEvents = 0;
        
        public MouseCapture() {
            if (_winHelper == null)
            {
                System.Windows.Window window = System.Windows.Application.Current.MainWindow;
                _winHelper = new System.Windows.Interop.WindowInteropHelper(window);
            }
            if (_rawinput == null)
            {
                _rawinput = new RawInput(_winHelper.Handle, CaptureOnlyInForeground);
                _rawinput.AddMessageFilter();
            }
        }

        /*
        private static object syncRoot = new Object();
        public static MouseCapture Instance
        {
            
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new MouseCapture();
                    }
                }

                return instance;
            }


        }*/

        public static bool IsStarted()
        {
            return _boundEvents == 1;
        }
        public static string Status()
        {
            if (_boundEvents == 1) return "STARTED";
            if (_boundEvents == 0) return "STOPPED";
            return (string.Format("ERROR: [{0}] EVENTS BOUND", _boundEvents));
        }
        
        private static void OnMouseClicked(object sender, RawInputEventArg e)
        { 
            deviceHandle = e.MouseClickEvent.DeviceHandle.ToString();
            deviceMessage = e.MouseClickEvent.Message.ToString();
            log.Debug("["+_boundEvents +"] "+ deviceHandle + ": " + deviceMessage);
            
            if (QuizViewModel.GetInQuizMode())
            {
                AnswersViewModel.AddAnswer(deviceHandle, deviceMessage);
            }
        }

        public   bool AddOnMouseClicked()
        {
           
            if (_boundEvents == 0)
            {
                _boundEvents += 1;
                _rawinput.MouseClicked += OnMouseClicked;
                log.Debug("[" + _boundEvents + "] Start capturing");

                return _boundEvents == 1;
            }
            return false;

          
        }
        public   void RemoveOnMouseClicked()
        {  
            if (_boundEvents > 0)
            {
                _boundEvents -= 1;
                _rawinput.MouseClicked -= OnMouseClicked;
                log.Debug("[" + _boundEvents + "] Stop capturing");
            }
                
            
        }
    }
}
