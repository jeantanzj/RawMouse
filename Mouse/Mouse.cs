using System;
using RawInput_dll;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Forms;


namespace Mouse
{
    public partial class Mouse : Form
    {
        private readonly RawInput _rawinput;
        
        const bool CaptureOnlyInForeground = true;
        // Todo: add checkbox to form when checked/uncheck create method to call that does the same as Keyboard ctor 

        public Mouse()
        {
            InitializeComponent();
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            _rawinput = new RawInput(Handle, CaptureOnlyInForeground);
           
            _rawinput.AddMessageFilter();   // Adding a message filter will cause keypresses to be handled
            //Win32.DeviceAudit();            // Writes a file DeviceAudit.txt to the current directory

            _rawinput.MouseClicked += OnMouseClicked;   
        }

        private void OnMouseClicked(object sender, RawInputEventArg e)
        {
            lbHandle.Text = e.MouseClickEvent.DeviceHandle.ToString();
            lbType.Text = e.MouseClickEvent.DeviceType;
            lbName.Text = e.MouseClickEvent.DeviceName;
            lbDescription.Text = e.MouseClickEvent.Name;
            lbMessage.Text = e.MouseClickEvent.Message.ToString();
        }
       

        private void Mouse_FormClosing(object sender, FormClosingEventArgs e)
        {
            _rawinput.MouseClicked -= OnMouseClicked;
        }

        private static void CurrentDomain_UnhandledException(Object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;

            if (null == ex) return;

            // Log this error. Logging the exception doesn't correct the problem but at least now
            // you may have more insight as to why the exception is being thrown.
            Debug.WriteLine("Unhandled Exception: " + ex.Message);
            Debug.WriteLine("Unhandled Exception: " + ex);
            MessageBox.Show(ex.Message);
        }
    }
}
